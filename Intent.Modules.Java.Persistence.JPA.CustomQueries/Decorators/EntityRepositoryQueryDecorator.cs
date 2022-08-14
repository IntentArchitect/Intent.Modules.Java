using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.Persistence.JPA.CustomQueries.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates;
using Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using Intent.RoslynWeaver.Attributes;
using WhereClauseCriteriaType = Intent.Java.Persistence.JPA.CustomQueries.Api.ColumnModelStereotypeExtensions.ColumnSettings.WhereClauseCriteriaTypeOptionsEnum;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class EntityRepositoryQueryDecorator : EntityRepositoryDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.CustomQueries.EntityRepositoryQueryDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly EntityRepositoryTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public EntityRepositoryQueryDecorator(EntityRepositoryTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override IEnumerable<string> GetMembers()
        {
            static string GetMappingPath(ColumnModel columnModel, string tableNameOrAlias)
            {
                var path = columnModel.InternalElement.MappedElement.Path
                    .Where(x => x.Specialization != GeneralizationModel.SpecializationType)
                    .Select(x => x.Name.ToCamelCase());

                return $"{tableNameOrAlias}.{string.Join('.', path)}";
            }

            static (IReadOnlyList<(string Name, string Alias)> Tables, IReadOnlyList<string> IncludedColumns, IReadOnlyList<string> WhereClauses) GetData(CustomQueryModel model)
            {
                var tables = new List<(string Name, string Alias)>();
                var includedColumns = new List<string>();
                var whereClauses = new List<string>();

                void Visit(IElement element, string tableNameOrAlias = null)
                {
                    switch (element.SpecializationTypeId)
                    {
                        case CustomQueryModel.SpecializationTypeId:
                            {
                                var model = element.AsCustomQueryModel();
                                var tableName = model.InternalElement.ParentElement.Name.ToPascalCase();
                                var alias = model.GetQuerySettings()?.Alias();

                                tableNameOrAlias = !string.IsNullOrWhiteSpace(alias)
                                    ? alias
                                    : tableName;

                                tables.Add((tableName, alias));
                                break;
                            }
                        case JoinedTableModel.SpecializationTypeId:
                            {
                                var model = element.AsJoinedTableModel();
                                var tableName = $"{tableNameOrAlias}.{model.InternalElement.MappedElement?.Element.Name.ToCamelCase()}";
                                var alias = model.GetJoinSettings()?.Alias();

                                tableNameOrAlias = !string.IsNullOrWhiteSpace(alias)
                                    ? alias
                                    : tableName;

                                tables.Add((tableName, alias));
                                break;
                            }
                        case ColumnModel.SpecializationTypeId:
                            {
                                var model = element.AsColumnModel();

                                if (model.GetColumnSettings().IncludeInResult())
                                {
                                    includedColumns.Add($"{GetMappingPath(model, tableNameOrAlias)} as {model.Name}");
                                }

                                if (!string.IsNullOrWhiteSpace(model.GetColumnSettings().WhereClauseCriteriaType().Value))
                                {
                                    whereClauses.Add(model.GetColumnSettings().WhereClauseCriteriaType().AsEnum() switch
                                    {
                                        WhereClauseCriteriaType.Parameter => $"{GetMappingPath(model, tableNameOrAlias)} = :{model.GetColumnSettings().WhereClauseParameterCriteria().Name}",
                                        WhereClauseCriteriaType.Custom => $"{GetMappingPath(model, tableNameOrAlias)} = {model.GetColumnSettings().WhereClauseCustomCriteria()}",
                                        _ => throw new ArgumentOutOfRangeException()
                                    });
                                }

                                break;
                            }
                    }

                    foreach (var childElement in element.ChildElements.OrderBy(x => x.IsColumnModel() ? 0 : 1))
                    {
                        Visit(childElement, tableNameOrAlias);
                    }
                }

                Visit(model.InternalElement);

                return (tables, includedColumns, whereClauses);
            }

            foreach (var model in new ClassExtensionsModel(_template.Model.InternalElement).CustomQueries.Where(x => x.IsMapped))
            {
                var distinct = model.GetQuerySettings().Distinct();
                var (tables, selectColumns, whereClauses) = GetData(model);

                var lines = new List<string>();

                if (selectColumns.Count > 0)
                {
                    lines.Add($"select{(distinct ? " distinct" : string.Empty)}");

                    for (var i = 0; i < selectColumns.Count; i++)
                    {
                        var isLastItem = i == selectColumns.Count - 1;
                        lines.Add($"{selectColumns[i]}{(!isLastItem ? "," : string.Empty)}");
                    }
                }
                else
                {
                    // If no columns are included in the result, then we select the entity itself
                    var table = tables[0];
                    lines.Add($"select {(!string.IsNullOrWhiteSpace(table.Alias) ? table.Alias : table.Name)}");
                }

                lines.Add($"from {string.Join(" join ", tables.Select(table => $"{table.Name}{(!string.IsNullOrWhiteSpace(table.Alias) ? $" {table.Alias}" : string.Empty)}"))}");

                if (whereClauses.Count > 0)
                {
                    lines.Add($"where {string.Join(" and ", whereClauses)}");
                }

                var query = $"\"{string.Join($" \" +{Environment.NewLine}            \"", lines)}\"";

                // If no columns are included in the result, then we return the entity itself
                var returnType = selectColumns.Count > 0
                    ? _template.GetQueryViewName(model)
                    : _template.GetDomainModelName(model.Mapping.Element.AsClassModel());

                var methodReturnType = model.GetQuerySettings().ReturnsCollection()
                    ? $"{_template.ImportType("java.util.List")}<{returnType}>"
                    : $"{_template.ImportType("java.util.Optional")}<{returnType}>";

                var parameters = model.Parameters
                    .Select(parameter => $"@{_template.ImportType("org.springframework.data.repository.query.Param")}(\"{parameter.Name}\") {_template.GetTypeName(parameter)} {parameter.Name}");

                yield return @$"@{_template.ImportType("org.springframework.data.jpa.repository.Query")}({query})
    {methodReturnType} {model.Name}({string.Join(", ", parameters)});";
            }

            foreach (var member in base.GetMembers())
            {
                yield return member;
            }
        }
    }
}