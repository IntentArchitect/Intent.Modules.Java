using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Intent.Engine;
using Intent.Java.Persistence.JPA.Queries.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates;
using Intent.Modules.Java.Persistence.JPA.Queries.Templates;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class EntityRepositoryQueryDecorator : EntityRepositoryDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.Queries.EntityRepositoryQueryDecorator";

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
            static QueryData GetQueryData(QueryModel query, EntityRepositoryTemplate template)
            {
                var tables = new List<(string Name, string Alias)>();
                var columns = new List<string>();
                var whereClauses = new List<string>();
                var annotatedParameters = new List<string>();
                var parameters = new List<string>();
                var queryProjection = query.TypeReference.Element.AsQueryProjectionModel();

                // Get "root" table:
                {
                    var tableName = query.InternalElement.ParentElement.Name.ToPascalCase();
                    var tableAlias = query.GetQuerySettings()?.TableAlias();

                    var table = (Name: tableName, Alias: !string.IsNullOrWhiteSpace(tableAlias) ? tableAlias : tableName.ToCamelCase());
                    tables.Add(table);
                }

                foreach (var column in queryProjection?.Columns ?? Enumerable.Empty<ColumnModel>())
                {
                    if (!column.InternalElement.IsMapped)
                    {
                        continue;
                    }

                    var queryPath = new List<string>
                    {
                        tables[0].Alias
                    };

                    foreach (var path in column.InternalElement.MappedElement.Path)
                    {
                        if (path.Element is IAssociationEnd associationEnd &&
                            associationEnd.TypeReference.IsCollection)
                        {
                            var table = (Name: $"{string.Join('.', queryPath.Append(associationEnd.Name.ToCamelCase()))}", Alias: associationEnd.Name.Singularize().ToCamelCase());
                            if (!tables.Contains(table))
                            {
                                tables.Add(table);
                            }

                            queryPath.Clear();
                            queryPath.Add(table.Alias);

                            continue;
                        }

                        if (path.Element.SpecializationType == GeneralizationModel.SpecializationType)
                        {
                            continue;
                        }

                        queryPath.Add(path.Name.ToCamelCase());
                    }

                    columns.Add($"{string.Join('.', queryPath)} as {column.Name.ToCamelCase()}");
                }

                foreach (var parameter in query.Parameters)
                {
                    var excludeFromParamList = parameter.GetParameterSettings()?.ExcludeFromParameterList() == true;
                    if (!excludeFromParamList)
                    {
                        var annotation = $"@{template.ImportType("org.springframework.data.repository.query.Param")}(\"{parameter.Name}\") ";

                        parameters.Add($"{template.GetTypeName(parameter)} {parameter.Name}");
                        annotatedParameters.Add($"{annotation}{template.GetTypeName(parameter)} {parameter.Name}");
                    }

                    if (!parameter.InternalElement.IsMapped)
                    {
                        continue;
                    }

                    var queryPath = new List<string>
                    {
                        tables[0].Alias
                    };

                    foreach (var path in parameter.InternalElement.MappedElement.Path)
                    {
                        if (path.Element is IAssociationEnd associationEnd &&
                            associationEnd.TypeReference.IsCollection)
                        {
                            var table = (Name: $"{string.Join('.', queryPath.Append(associationEnd.Name.ToCamelCase()))}", Alias: associationEnd.Name.Singularize().ToCamelCase());
                            if (!tables.Contains(table))
                            {
                                tables.Add(table);
                            }

                            queryPath.Clear();
                            queryPath.Add(table.Alias);

                            continue;
                        }

                        if (path.Element.SpecializationType == GeneralizationModel.SpecializationType)
                        {
                            continue;
                        }

                        queryPath.Add(path.Name.ToCamelCase());
                    }

                    var equals = excludeFromParamList
                        ? parameter.GetParameterSettings().Value()
                        : $":{parameter.Name}";
                    whereClauses.Add($"{string.Join('.', queryPath)} = {equals}");
                }

                return new QueryData(tables, columns, whereClauses, parameters, annotatedParameters);
            }

            foreach (var queryModel in new ClassExtensionModel(_template.Model.InternalElement).Queries)
            {
                var (tables, selectColumns, whereClauses, parameters, annotatedParameters) = GetQueryData(queryModel, _template);
                var distinct = queryModel.GetQuerySettings().Distinct();
                var returnType = queryModel.TypeReference.IsCollection
                    ? _template.GetTypeName(queryModel, $"{_template.ImportType("java.util.List")}<{{0}}>")
                    : $"{_template.ImportType("java.util.Optional")}<{_template.GetTypeName(queryModel)}>";

                // If we're returning the domain object and all parameters are simple included ones,
                // then we can just make a method with an @Query annotation which is conventionally
                // interpreted by JPA as a query filtering by each parameter.
                if (queryModel.TypeReference.Element.Id == _template.Model.Id &&
                    queryModel.Parameters.All(x => x.GetParameterSettings()?.ExcludeFromParameterList() != true))
                {
                    yield return $"{returnType} {queryModel.Name}({string.Join(", ", parameters)});";
                    continue;
                }

                var lines = new List<string>();

                if (queryModel.TypeReference.Element.IsClassModel())
                {
                    lines.Add($"select {tables[0].Alias}");
                }
                else
                {
                    lines.Add($"select{(distinct ? " distinct" : string.Empty)}");

                    for (var i = 0; i < selectColumns.Count; i++)
                    {
                        var isLastItem = i == selectColumns.Count - 1;
                        lines.Add($"{selectColumns[i]}{(!isLastItem ? "," : string.Empty)}");
                    }
                }

                lines.Add($"from {string.Join(" join ", tables.Select(table => $"{table.Name} {table.Alias}"))}");

                if (whereClauses.Count > 0)
                {
                    lines.Add($"where {string.Join(" and ", whereClauses)}");
                }

                var query = $"\"{string.Join($" \" +{Environment.NewLine}            \"", lines)}\"";

                yield return @$"@{_template.ImportType("org.springframework.data.jpa.repository.Query")}({query})
    {returnType} {queryModel.Name}({string.Join(", ", annotatedParameters)});";
            }

            foreach (var member in base.GetMembers())
            {
                yield return member;
            }
        }

        private record struct QueryData(
            IReadOnlyList<(string Name, string Alias)> Tables,
            IReadOnlyList<string> SelectColumns,
            IReadOnlyList<string> WhereClauses,
            IReadOnlyList<string> Parameters,
            IReadOnlyList<string> AnnotatedParameters);
    }
}