using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
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
    public class UpdateImplementationStrategy : IImplementationStrategy
    {
        private readonly ServiceImplementationTemplate _template;
        private readonly IApplication _application;

        public UpdateImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }
        
        public bool IsMatch(OperationModel operationModel)
        {
            if (operationModel.Parameters.Count != 2)
            {
                return false;
            }
            
            if (!operationModel.Parameters.Any(p => p.Name.Contains("id", StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (operationModel.TypeReference.Element != null)
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
            return new[] { "put", "update" }.Any(x => lowerOperationName.Contains(x));
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
            var idParam = operationModel.Parameters.First(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));
            var dtoParam = operationModel.Parameters.First(p => !p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));
            
            var codeLines = new JavaStatementAggregator();
            codeLines.Add($@"var {domainTypeCamelCased} = {repositoryFieldName}.findById({idParam.Name.ToCamelCase()}).get();");
            codeLines.AddRange(GetDTOPropertyAssignments(domainTypeCamelCased, dtoParam.Name.ToCamelCase(), domainModel.InternalElement, dtoModel.Fields));
            codeLines.Add($"{repositoryFieldName}.save({domainTypeCamelCased});");

            var @class = _template.JavaFile.Classes.First();
            if (@class.Fields.All(p => p.Type != repositoryTypeName))
            {
                @class.AddField(_template.ImportType(repositoryTypeName), repositoryFieldName);
            }
            var method = @class.FindMethod(m => m.Name.Equals(operationModel.Name, StringComparison.OrdinalIgnoreCase));
            method.Statements.Clear();
            method.AddStatements(codeLines.ToList());
        }
        
        private IEnumerable<JavaStatement> GetDTOPropertyAssignments(string entityVarName, string dtoVarName, IElement domainModel, IList<DTOFieldModel> dtoFields)
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
                        var attribute = field.Mapping?.Element
                                        ?? domainModel.ChildElements.First(p => p.Name == field.Name);
                        if (!attribute.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                        {
                            codeLines.Add($"{entityVarExpr}.set{attribute.Name.ToPascalCase()}({dtoVarName}.{field.Name.ToCamelCase()});");
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
                            
                            if (field.TypeReference.IsNullable)
                            {
                                codeLines.Add(new JavaStatementBlock($"if ({dtoVarName}.get{field.Name.ToPascalCase()}() != null)")
                                    .AddStatement($"{entityVarExpr}.set{attributeName.ToPascalCase()}({GetUpdateMethodName(targetType)}({dtoVarName}.get{field.Name.ToPascalCase()}));"));
                            }
                            else
                            {
                                codeLines.Add($"{entityVarExpr}.set{attributeName.ToPascalCase()}({GetUpdateMethodName(targetType)}({dtoVarName}.get{field.Name.ToPascalCase()}));");
                            }
                        }
                        else
                        {
                            if (field.TypeReference.IsNullable)
                            {
                                codeLines.Add(new JavaStatementBlock($"if ({dtoVarName}.get{field.Name.ToPascalCase()}() != null)")
                                    .AddStatement($"{entityVarExpr}.set{attributeName.ToPascalCase()}(dto.get{field.Name.ToPascalCase()}().stream().map(x -> {GetUpdateMethodName(targetType)}(x)).collect(Collectors.toList()));"));
                            }
                            else
                            {
                                codeLines.Add($"{entityVarExpr}.set{attributeName.ToPascalCase()}(dto.get{field.Name.ToPascalCase()}().stream().map(x -> {GetUpdateMethodName(targetType)}(x)).collect(Collectors.toList()));");
                            }
                        }

                        var @class = _template.JavaFile.Classes.First();
                        @class.AddMethod(_template.GetTypeName(targetType),
                            GetUpdateMethodName(targetType),
                            method => method
                                .Private()
                                .Static()
                                .AddParameter(_template.GetTypeName((IElement)field.TypeReference.Element), "dto")
                                .AddStatement($"var {entityVarName} = new {targetType.Name.ToPascalCase()}();")
                                .AddStatements(GetDTOPropertyAssignments(entityVarName, $"dto", targetType,
                                    ((IElement)field.TypeReference.Element).ChildElements.Where(x => x.IsDTOFieldModel()).Select(x => x.AsDTOFieldModel()).ToList()))
                                .AddStatement($"return {entityVarName};"));
                    }
                        break;
                }
            }

            return codeLines;
        }
        
        private string GetUpdateMethodName(ICanBeReferencedType classModel)
        {
            return $"update{classModel.Name.ToPascalCase()}";
        }
    }
}
