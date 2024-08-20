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

            var dtoModel = operationModel.Parameters.FirstOrDefault(x => x.TypeReference?.Element?.IsDTOModel() == true)?.TypeReference?.Element?.AsDTOModel();
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
            var dtoModel = operationModel.Parameters.FirstOrDefault(x => x.TypeReference.Element.IsDTOModel()).TypeReference.Element.AsDTOModel();
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
            codeLines.AddRange(GetDTOPropertyAssignments(domainTypeCamelCased, dtoParam.Name.ToCamelCase(), domainModel.InternalElement, dtoModel.InternalElement));
            codeLines.Add($"{repositoryFieldName}.save({domainTypeCamelCased});");

            _template.JavaFile.AddImport("java.util.stream.Collectors");
            _template.JavaFile.AddImport("java.util.function.Function");
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

        private IEnumerable<JavaStatement> GetDTOPropertyAssignments(string entityVarName, string dtoVarName, IElement domainModel, IElement dtoModel)
        {
            if (string.IsNullOrEmpty(entityVarName))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(entityVarName));
            }

            var codeLines = new List<JavaStatement>();
            var dtoModelClass = dtoModel.AsDTOModel();
            foreach (var field in dtoModelClass.Fields)
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
                            codeLines.Add($"{GetUpdateMethodName(targetType)}({entityVarExpr}, {dtoVarName});");

                            var @class = _template.JavaFile.Classes.First();
                            if (@class.FindMethod(GetUpdateMethodName(targetType)) == null)
                            {
                                @class.AddMethod("void",
                                    GetUpdateMethodName(targetType),
                                    method =>
                                    {
                                        var domainVarName = domainModel.Name.ToCamelCase();
                                        method
                                            .Private()
                                            .Static()
                                            .AddParameter(domainModel.Name.ToPascalCase(), domainVarName)
                                            .AddParameter(_template.GetTypeName(dtoModelClass.InternalElement), "dto")
                                            .AddStatement($"var {domainVarName}{attributeName.ToPascalCase()} = {domainVarName}.get{attributeName.ToPascalCase()}();")
                                            .AddStatement($"var dto{attributeName.ToPascalCase()} = dto.get{attributeName.ToPascalCase()}();")
                                            .AddStatements(GetDTOPropertyAssignments($"{domainVarName}{attributeName.ToPascalCase()}", $"dto{attributeName.ToPascalCase()}", targetType, (IElement)field.TypeReference.Element));
                                        if (field.TypeReference.IsNullable)
                                        {
                                            method.InsertStatement(0, new JavaStatementBlock($"if (dto == null)")
                                                .AddStatement($"{domainVarName}.set{attributeName.ToPascalCase()}(null);"));
                                        }
                                    });
                            }
                        }
                        else
                        {
                            if (field.TypeReference.IsNullable)
                            {
                                codeLines.Add(new JavaStatementBlock($"if ({dtoVarName}.get{field.Name.ToPascalCase()}() != null)")
                                    .AddStatement($"{GetUpdateListMethodName(targetType)}({entityVarExpr}.get{attributeName.ToPascalCase()}(), {dtoVarName}.get{field.Name.ToPascalCase()}());"));
                            }
                            else
                            {
                                codeLines.Add(
                                    $"{GetUpdateListMethodName(targetType)}({entityVarExpr}.get{attributeName.ToPascalCase()}(), {dtoVarName}.get{field.Name.ToPascalCase()}());");
                            }

                            var @class = _template.JavaFile.Classes.First();
                            if (@class.FindMethod(GetUpdateListMethodName(targetType)) == null)
                            {
                                @class.AddMethod("void",
                                    GetUpdateListMethodName(targetType),
                                    method =>
                                    {
                                        var domainListVarName = $"existing{targetType.Name.ToPascalCase().Pluralize()}";
                                        var dtoListVarName = $"updated{targetType.Name.ToPascalCase().Pluralize()}";
                                        var domainSingleVarName = $"exist{targetType.Name.ToPascalCase()}";
                                        var dtoSingleVarName = $"update{targetType.Name.ToPascalCase()}";
                                        method
                                            .Private()
                                            .Static()
                                            .AddParameter($"List<{targetType.Name.ToPascalCase()}>", domainListVarName)
                                            .AddParameter($"List<{_template.GetTypeName((IElement)field.TypeReference.Element)}>", dtoListVarName)
                                            .AddStatement(
                                                $"var {dtoSingleVarName}Map = {dtoListVarName}.stream().collect(Collectors.toMap({_template.GetTypeName((IElement)field.TypeReference.Element)}::getId, Function.identity()));")
                                            .AddStatement(new JavaStatementBlock($"for ({targetType.Name.ToPascalCase()} {domainSingleVarName} : {domainListVarName}.stream().toList())")
                                                .AddStatement($"var {dtoSingleVarName} = {dtoSingleVarName}Map.get({domainSingleVarName}.getId());")
                                                .AddStatement(new JavaStatementBlock($"if ({dtoSingleVarName} != null)")
                                                    .AddStatements(GetDTOPropertyAssignments(domainSingleVarName, dtoSingleVarName, targetType, (IElement)field.TypeReference.Element))
                                                    .AddStatement($"{dtoSingleVarName}Map.remove({domainSingleVarName}.getId());"))
                                                .AddStatement(new JavaStatementBlock("else")
                                                    .AddStatement($"{domainListVarName}.remove({domainSingleVarName});")))
                                            .AddStatement(new JavaStatementBlock($"for (var new{targetType.Name.ToPascalCase()} : {dtoSingleVarName}Map.values())")
                                                .AddStatement($"{domainListVarName}.add({GetCreateMethodName(targetType)}(new{targetType.Name.ToPascalCase()}));"));
                                    });
                            }

                            if (@class.FindMethod(x => x.Name == GetCreateMethodName(targetType) &&
                                                       x.Parameters.First().Type == _template.GetTypeName((IElement)field.TypeReference.Element)) == null)
                            {
                                @class.AddMethod(_template.GetTypeName(targetType),
                                    GetCreateMethodName(targetType),
                                    method =>
                                    {
                                        var domainVarName = targetType.Name.ToCamelCase();
                                        method
                                            .Private()
                                            .Static()
                                            .AddParameter(_template.GetTypeName((IElement)field.TypeReference.Element), "dto")
                                            .AddStatement($"var {domainVarName} = new {targetType.Name.ToPascalCase()}();")
                                            .AddStatements(new CreateImplementationStrategy(_template, _application).GetDTOPropertyAssignments(domainVarName, $"dto", targetType, ((IElement)field.TypeReference.Element).ChildElements.Where(x => x.IsDTOFieldModel()).Select(x => x.AsDTOFieldModel()).ToList()))
                                            .AddStatement($"return {domainVarName};");
                                    });
                            }
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
        
        private string GetUpdateMethodName(ICanBeReferencedType classModel)
        {
            return $"update{classModel.Name.ToPascalCase()}";
        }
        
        private string GetUpdateListMethodName(ICanBeReferencedType classModel)
        {
            return $"update{classModel.Name.ToPascalCase().Pluralize()}";
        }
    }
}
