using System;
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

            var lowerDomainName = domainModel.Name.ToLower();
            var lowerOperationName = operationModel.Name.ToLower();
            return new[]
                {
                    "post",
                    $"post{lowerDomainName}",
                    "create",
                    $"create{lowerDomainName}",
                    $"add{lowerDomainName}",
                }
                .Contains(lowerOperationName);
        }

        public void ApplyStrategy(OperationModel operationModel)
        {
            var dtoModel = operationModel.Parameters.First().TypeReference.Element.AsDTOModel();
            var domainModel = dtoModel.Mapping.Element.AsClassModel();
            var domainType = _template.TryGetTypeName(DomainModelTemplate.TemplateId, domainModel, out var result)
                ? result
                : domainModel.Name;
            var domainTypeCamelCased = domainType.ToCamelCase();
            var repositoryTypeName = _template.GetTypeName(EntityRepositoryTemplate.TemplateId, domainModel).ToCamelCase();
            
            var codeLines = new JavaStatementAggregator();
            codeLines.Add($@"var {domainTypeCamelCased} = new {domainType}();");
            codeLines.AddRange(GetDTOPropertyAssignments(domainTypeCamelCased, "dto", domainModel.InternalElement, dtoModel.Fields));
            
            codeLines.Add($"{repositoryTypeName}.save({domainTypeCamelCased});");
            
            if (operationModel.TypeReference.Element != null)
            {
                var idField = domainModel.GetPrimaryKeys().PrimaryKeys.SingleOrDefault()?.Name.ToCamelCase() ?? "Id";
            
                codeLines.Add($"return {domainTypeCamelCased}.get{idField.ToPascalCase()}();");
            }

            var @class = _template.JavaFile.Classes.First();
            var method = @class.FindMethod(m => m.Name.Equals(operationModel.Name, StringComparison.OrdinalIgnoreCase));
            method.Statements.Clear();
            method.AddStatements(codeLines.ToList());
        }

        // public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        // {
        //     var domainType = _template.GetDomainTypeName(domainModel);
        //     var domainTypeCamelCased = domainType.ToCamelCase();
        //     var repositoryFieldName = _decorator.GetRepositoryDependency(domainModel).Name;
        //
        //     var statements = new List<string>
        //     {
        //         $"var {domainTypeCamelCased} = new {domainType}();"
        //     };
        //
        //     statements.AddRange(GetPropertyAssignments(domainModel, operationModel.Parameters.Single(), domainTypeCamelCased));
        //
        //     statements.Add($"{repositoryFieldName}.save({domainTypeCamelCased});");
        //
        //     if (operationModel.TypeReference.Element != null)
        //     {
        //         var idField = domainModel.GetPrimaryKeys().PrimaryKeys.SingleOrDefault()?.Name.ToCamelCase() ?? "Id";
        //
        //         statements.Add($"return {domainTypeCamelCased}.get{idField}();");
        //     }
        //
        //     return string.Join(@"
        // ", statements);
        // }

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
                                    .AddStatement($"{entityVarExpr}.set{attributeName.ToPascalCase()}({GetCreateMethodName(targetType, attributeName)}({dtoVarName}.get{field.Name.ToPascalCase()}));"));
                            }
                            else
                            {
                                codeLines.Add($"{entityVarExpr}.set{attributeName.ToPascalCase()}({GetCreateMethodName(targetType, attributeName)}({dtoVarName}.get{field.Name.ToPascalCase()}));");
                            }
                        }
                        else
                        {
                            if (field.TypeReference.IsNullable)
                            {
                                codeLines.Add(new JavaStatementBlock($"if ({dtoVarName}.get{field.Name.ToPascalCase()}() != null)")
                                    .AddStatement($"{entityVarExpr}.set{attributeName.ToPascalCase()}(dto.get{field.Name.ToPascalCase()}().stream().map(x -> {GetCreateMethodName(targetType, attributeName)}(x)).collect(Collectors.toList()));"));
                            }
                            else
                            {
                                codeLines.Add($"{entityVarExpr}.set{attributeName.ToPascalCase()}(dto.get{field.Name.ToPascalCase()}().stream().map(x -> {GetCreateMethodName(targetType, attributeName)}(x)).collect(Collectors.toList()));");
                            }
                        }

                        var @class = _template.JavaFile.Classes.First();
                        @class.AddMethod(_template.GetTypeName(targetType),
                            GetCreateMethodName(targetType, attributeName),
                            method => method.Private()
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
        
        private string GetCreateMethodName(ICanBeReferencedType classModel, [CanBeNull] string attributeName)
        {
            return $"create{classModel.Name.ToPascalCase()}";
        }
        
        // private IEnumerable<string> GetPropertyAssignments(
        //     ClassModel domainModel,
        //     ParameterModel operationParameterModel,
        //     string variableName)
        // {
        //     var dto = operationParameterModel.TypeReference.Element.AsDTOModel();
        //     foreach (var dtoField in dto.Fields)
        //     {
        //         var domainAttribute = domainModel.Attributes.FirstOrDefault(p => p.Name.Equals(dtoField.Name, StringComparison.OrdinalIgnoreCase));
        //         if (domainAttribute == null)
        //         {
        //             yield return $"// Warning: No matching field found for {dtoField.Name}";
        //             continue;
        //         }
        //
        //         if (domainAttribute.Type.Element.Id != dtoField.TypeReference.Element.Id)
        //         {
        //             yield return $"// Warning: No matching type for Domain: {domainAttribute.Name} and DTO: {dtoField.Name}";
        //             continue;
        //         }
        //
        //         yield return $"{variableName}.{domainAttribute.Setter()}({operationParameterModel.Name}.{dtoField.Getter()}());";
        //     }
        // }
    }
}