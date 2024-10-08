﻿using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Persistence.JPA;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using JetBrains.Annotations;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;
using ParameterModel = Intent.Modelers.Services.Api.ParameterModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class CreateImplementationStrategy : IImplementationStrategy
    {
        private readonly ServiceImplementationTemplate _template;
        private readonly IApplication _application;

        public CreateImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }
        
        public bool IsMatch(OperationModel operationModel)
        {
            if (operationModel.Parameters.Count != 1)
            {
                return false;
            }

            // We seriously need a better way to check for a surrogate key here. This is not a good approach.
            if (operationModel.TypeReference.Element != null
                && !_template.GetTypeInfo(operationModel.TypeReference).IsPrimitive
                && !operationModel.TypeReference.HasGuidType())
            {
                return false;
            }

            var dtoModel = operationModel.Parameters.First().TypeReference.Element.AsDTOModel();
            if (dtoModel == null)
            {
                return false;
            }

            var domainModel = dtoModel.Mapping?.Element?.AsClassModel();
            if (domainModel == null)
            {
                return false;
            }
            
            // Support for composite primary keys not implemented:
            if (domainModel.GetPrimaryKeys().PrimaryKeys.Count > 1)
            {
                return false;
            }
            
            var lowerOperationName = operationModel.Name.ToLower();
            return new[] { "post", "create", "add" }.Any(x => lowerOperationName.Contains(x));
        }

        public void ApplyStrategy(OperationModel operationModel)
        {
            var dtoModel = operationModel.Parameters.First().TypeReference.Element.AsDTOModel();
            var domainModel = dtoModel.Mapping.Element.AsClassModel();
            var domainType = _template.TryGetTypeName(DomainModelTemplate.TemplateId, domainModel, out var result)
                ? result
                : domainModel.Name;
            var domainTypeCamelCased = domainType.ToCamelCase();
            var repositoryTypeName = _template.GetTypeName(EntityRepositoryTemplate.TemplateId, domainModel);
            var repositoryFieldName = repositoryTypeName.ToCamelCase();
            var dtoParam = operationModel.Parameters.First();
            
            var codeLines = new JavaStatementAggregator();
            codeLines.Add($@"var {domainTypeCamelCased} = new {domainType}();");
            codeLines.AddRange(GetDTOPropertyAssignments(domainTypeCamelCased, dtoParam.Name.ToCamelCase(), domainModel.InternalElement, dtoModel.Fields));
            codeLines.Add($"{repositoryFieldName}.save({domainTypeCamelCased});");
            
            if (operationModel.TypeReference.Element != null)
            {
                var idField = domainModel.GetPrimaryKeys().PrimaryKeys.SingleOrDefault()?.Name.ToCamelCase() ?? "Id";
            
                codeLines.Add($"return {domainTypeCamelCased}.get{idField.ToPascalCase()}();");
            }

            _template.JavaFile.AddImport("java.util.stream.Collectors");
            var @class = _template.JavaFile.Classes.First();
            if (@class.Fields.All(p => p.Type != repositoryTypeName))
            {
                @class.AddField(_template.ImportType(repositoryTypeName), repositoryFieldName);
            }
            var method = @class.FindMethod(m => m.Name.Equals(operationModel.Name, StringComparison.OrdinalIgnoreCase));
            method.Annotations.Where(p => p.Name.Contains("IntentIgnoreBody")).ToList().ForEach(x => method.Annotations.Remove(x));
            method.Statements.Clear();
            method.AddStatements(codeLines.ToList());
        }

        internal IEnumerable<JavaStatement> GetDTOPropertyAssignments(string entityVarName, string dtoVarName, IElement domainModel, IList<DTOFieldModel> dtoFields)
        {
            if (string.IsNullOrEmpty(entityVarName))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(entityVarName));
            }
            
            var codeLines = new List<JavaStatement>();
            foreach (var field in dtoFields)
            {
                if (field.Mapping?.Element == null
                    && domainModel.ChildElements.All(p => p.Name != field.Name))
                {
                    codeLines.Add($"// Warning: No matching field found for {field.Name}");
                    continue;
                }

                var entityVarExpr = entityVarName;
                switch (field.Mapping?.Element?.SpecializationTypeId)
                {
                    default:
                        var mappedPropertyName = field.Mapping?.Element?.Name ?? "<null>";
                        codeLines.Add($"// Warning: No matching type for Domain: {mappedPropertyName} and DTO: {field.Name}");
                        break;
                    case null:
                    case AttributeModel.SpecializationTypeId:
                        var attribute = (field.Mapping?.Element
                                        ?? domainModel.ChildElements.First(p => p.Name == field.Name)).AsAttributeModel();
                        if (!attribute.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                        {
                            codeLines.Add($"{entityVarExpr}.{attribute.Setter()}({dtoVarName}.{field.Getter()}());");
                            break;
                        }

                        break;
                    case AssociationTargetEndModel.SpecializationTypeId:
                    {
                        var association = field.Mapping.Element.AsAssociationTargetEndModel();
                        var targetType = association.Element as IElement;
                        var attributeName = association.Name.ToPascalCase();

                        if (association.Association.AssociationType == AssociationType.Aggregation)
                        {
                            codeLines.Add($@"#warning Field not a composite association: {field.Name.ToPascalCase()}");
                            break;
                        }

                        if (association.Multiplicity is Multiplicity.One or Multiplicity.ZeroToOne)
                        {
                            
                            codeLines.Add($"{entityVarExpr}.set{attributeName.ToPascalCase()}({GetCreateMethodName(targetType)}({dtoVarName}.get{field.Name.ToPascalCase()}()));");
                        }
                        else
                        {
                            if (field.TypeReference.IsNullable)
                            {
                                codeLines.Add(new JavaStatementBlock($"if ({dtoVarName}.get{field.Name.ToPascalCase()}() != null)")
                                    .AddStatement($"{entityVarExpr}.set{attributeName.ToPascalCase()}(dto.get{field.Name.ToPascalCase()}().stream().map(x -> {GetCreateMethodName(targetType)}(x)).collect(Collectors.toList()));"));
                            }
                            else
                            {
                                codeLines.Add($"{entityVarExpr}.set{attributeName.ToPascalCase()}(dto.get{field.Name.ToPascalCase()}().stream().map(x -> {GetCreateMethodName(targetType)}(x)).collect(Collectors.toList()));");
                            }
                        }

                        var @class = _template.JavaFile.Classes.First();
                        if (@class.FindMethod(x => x.Name == GetCreateMethodName(targetType) &&
                                                   x.Parameters.First().Type == _template.GetTypeName((IElement)field.TypeReference.Element)) == null)
                        {
                            @class.AddMethod(_template.GetTypeName(targetType),
                                GetCreateMethodName(targetType),
                                method =>
                                {
                                    var dtoType = _template.GetTypeName((IElement)field.TypeReference.Element);
                                    var domainVarName = targetType.Name.ToCamelCase();
                                    method
                                        .Private()
                                        .Static()
                                        .AddParameter(dtoType, "dto")
                                        .AddStatement($"var {domainVarName} = new {targetType.Name.ToPascalCase()}();")
                                        .AddStatements(GetDTOPropertyAssignments(domainVarName, $"dto", targetType,
                                            ((IElement)field.TypeReference.Element).ChildElements.Where(x => x.IsDTOFieldModel()).Select(x => x.AsDTOFieldModel()).ToList()))
                                        .AddStatement($"return {domainVarName};");

                                    if (field.TypeReference.IsNullable)
                                    {
                                        method.InsertStatement(0, new JavaStatementBlock($@"if (dto == null)")
                                            .AddStatement($@"return null;"));
                                    }
                                });
                        }
                    }
                        break;
                }
            }

            return codeLines;
        }
        
        private string GetCreateMethodName(ICanBeReferencedType classModel)
        {
            return $"create{classModel.Name.ToPascalCase()}";
        }
    }
}