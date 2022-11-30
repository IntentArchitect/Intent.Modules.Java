using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.Persistence.JPA.Api;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Persistence.JPA.Settings;
using Intent.Modules.Java.Persistence.JPA.Templates;
using Intent.Modules.Metadata.RDBMS.Api.Indexes;
using Intent.Modules.Metadata.RDBMS.Settings;
using Intent.RoslynWeaver.Attributes;
using static Intent.Java.Persistence.JPA.Api.AssociationSourceEndModelStereotypeExtensions.AssociationJPASettings;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class JpaDomainModelDecorator : DomainModelDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.JpaDomainModelDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly DomainModelTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public JpaDomainModelDecorator(DomainModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-data-jpa"));
            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-jdbc"));
            _template.AddDependency(new JavaDependency("com.h2database", "h2"));
        }

        public override IEnumerable<string> ClassAnnotations()
        {
            if (_template.Model.IsAbstract)
            {
                yield return $"@{_template.ImportType("javax.persistence.MappedSuperclass")}";
            }
            else
            {
                yield return $"@{_template.ImportType("javax.persistence.Entity")}";
            }

            if (TryGetSecondaryTableName(out _))
            {
                yield return $"@{_template.ImportType("javax.persistence.SecondaryTable")}(name = {_template.ClassName}.TABLE_NAME)";

            }
            else if (_template.Model.GetParentClasses().All(x => !x.IsAbstract) &&
                     (!_template.Model.IsAbstract || _template.Model.HasTable()))
            {
                yield return $"@{_template.ImportType("javax.persistence.Table")}({string.Join(", ", GetTableAttributes())})";
            }

            if (_template.Model.Attributes.Count(x => x.HasPrimaryKey()) > 1)
            {
                yield return $"@{_template.ImportType("javax.persistence.IdClass")}({_template.GetCompositeIdName(_template.Model)}.class)";
            }

            if (!_template.Model.IsAbstract &&
                !DerivesFromNonAbstractClass(_template.Model) &&
                _template.Model.ChildClasses.Any())
            {
                yield return $"@{_template.ImportType("javax.persistence.Inheritance")}(strategy = {_template.ImportType("javax.persistence.InheritanceType")}.SINGLE_TABLE)";
            }
        }

        private static bool DerivesFromNonAbstractClass(ClassModel model)
        {
            while (model != null)
            {
                if (model.ParentClass?.IsAbstract == false)
                {
                    return true;
                }

                model = model.ParentClass;
            }

            return false;
        }

        private bool TryGetSecondaryTableName(out string tableName)
        {
            tableName = default;

            var model = _template.Model;
            if (model.IsAbstract ||
                !model.HasTable() ||
                !DerivesFromNonAbstractClass(model))
            {
                return false;
            }

            tableName = !string.IsNullOrWhiteSpace(model.GetTable().Name())
                ? model.GetTable().Name()
                : model.Name;

            tableName = ApplyTableNameConvention(tableName).ToSnakeCase();
            return true;
        }

        private IEnumerable<string> GetTableAttributes()
        {
            var tableName = _template.Model.HasTable()
                ? _template.Model.GetTable().Name()
                : ApplyTableNameConvention(_template.Model.Name).ToSnakeCase();

            yield return $@"name = ""{tableName}""";

            if (!string.IsNullOrEmpty(_template.Model.GetTable()?.Schema()))
            {
                yield return $@"schema = ""{_template.Model.GetTable().Schema()}""";
            }

            if (!_template.Model.Attributes.Any(x => x.HasIndex()) && !_template.Model.GetIndexes().Any())
            {
                yield break;
            }

            var stereotypeIndexes = _template.Model.Attributes
                .Where(x => x.HasIndex())
                .GroupBy(x => x.GetIndex().UniqueKey() ?? "IX_" + _template.Model.Name + "_" + x.Name)
                .ToList();

            var elementIndexes = _template.Model.GetIndexes()
                .ToList();

            var indexList = new List<string>();

            foreach (var stereotypeIndex in stereotypeIndexes)
            {
                indexList.Add(
                    $"@{_template.ImportType("javax.persistence.Index")}(name = \"{stereotypeIndex.Key}\", columnList = \"{string.Join(",", stereotypeIndex.Select(c => c.Name.ToSnakeCase()))}\")");
            }

            foreach (var elementIndex in elementIndexes)
            {
                indexList.Add(
                    $"@{_template.ImportType("javax.persistence.Index")}(name = \"{elementIndex.Name}\", columnList = \"{string.Join(",", elementIndex.KeyColumns.Select(c => c.Name.ToSnakeCase()))}\")");
            }

            const string newLine = @",
        ";
            yield return $@"indexes = {string.Join(newLine, indexList)}";
        }

        public override IEnumerable<string> Fields()
        {
            if (TryGetSecondaryTableName(out var tableName))
            {
                yield return $"static final String TABLE_NAME = \"{tableName}\";";
            }

            if (!_template.Model.Attributes.Any(x => x.HasPrimaryKey()) &&
                !_template.Model.GetParentClasses()
                    .Any(x => !x.IsAbstract || x.Attributes
                        .Any(a => a.HasPrimaryKey())))
            {
                // Return implicit primary key
                var (type, columnType) = _application.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
                {
                    DatabaseSettings.KeyTypeOptionsEnum.Guid => (_template.ImportType("java.util.UUID"), "uuid"),
                    DatabaseSettings.KeyTypeOptionsEnum.Long => ("Long", null),
                    DatabaseSettings.KeyTypeOptionsEnum.Int => ("Integer", null),
                    _ => throw new ArgumentOutOfRangeException()
                };

                yield return @$"@{_template.ImportType("javax.persistence.Id")}
    @{_template.ImportType("javax.persistence.GeneratedValue")}(strategy = {_template.ImportType("javax.persistence.GenerationType")}.AUTO)
    @{_template.ImportType("javax.persistence.Column")}(columnDefinition = ""{columnType}"")
    private {type} id;";
            }
        }

        public override string BeforeField(AttributeModel model)
        {
            var annotations = new List<string>();
            var columnSettings = new List<string>();

            if (model.HasPrimaryKey())
            {
                annotations.Add($"@{_template.ImportType("javax.persistence.Id")}");
                annotations.Add($"@{_template.ImportType("javax.persistence.GeneratedValue")}(strategy = {_template.ImportType("javax.persistence.GenerationType")}.AUTO)");
                if (model.TypeReference.Element.Name.ToLowerInvariant() is "guid" or "uuid")
                {
                    columnSettings.Add(@"columnDefinition = ""uuid""");
                }
            }

            var columnName = !string.IsNullOrWhiteSpace(model.GetColumn()?.Name())
                ? model.GetColumn().Name()
                : model.Name.ToSnakeCase();
            columnSettings.Add($"name = \"{columnName}\"");
            if (_template.GetTypeName(model.TypeReference) == "String" &&
                model.GetTextConstraints()?.MaxLength() != null)
            {
                columnSettings.Add($"length = {model.GetTextConstraints().MaxLength()}");
            }

            if (TryGetSecondaryTableName(out _))
            {
                columnSettings.Add("table = TABLE_NAME");
            }

            if (!model.TypeReference.IsNullable)
            {
                columnSettings.Add("nullable = false");
            }

            annotations.Add($@"@{_template.ImportType("javax.persistence.Column")}({string.Join(", ", columnSettings)})");

            const string newLine = @"
    ";
            return string.Join(newLine, annotations);
        }

        public override string BeforeField(AssociationEndModel thatEnd)
        {
            var fetchType = thatEnd switch
            {
                AssociationSourceEndModel endModel => endModel.GetAssociationJPASettings()?.FetchType().Value,
                AssociationTargetEndModel endModel => endModel.GetAssociationJPASettings()?.FetchType().Value,
                _ => null
            };

            fetchType = $"fetch = {_template.ImportType("javax.persistence.FetchType")}.{(fetchType ?? FetchTypeOptionsEnum.Lazy.ToString()).ToUpperInvariant()}";

            if (!thatEnd.IsNavigable)
            {
                throw new InvalidOperationException("Cannot call this method if associationEnd is not navigable.");
            }
            var annotations = new List<string>();
            var sourceEnd = thatEnd.OtherEnd();
            if (!sourceEnd.IsCollection && !thatEnd.IsCollection) // one-to-one
            {
                var settings = new List<string>
                {
                    $"optional = {thatEnd.IsNullable.ToString().ToLower()}",
                    $"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}"
                };

                if (sourceEnd.IsNullable)
                {
                    settings.Add("orphanRemoval = true");
                }

                if (!string.IsNullOrWhiteSpace(fetchType))
                {
                    settings.Add(fetchType);
                }

                annotations.Add($"@{_template.ImportType("javax.persistence.OneToOne")}({string.Join(", ", settings)})");
                if (thatEnd.IsTargetEnd())
                {
                    annotations.Add($"@{_template.ImportType("javax.persistence.JoinColumn")}(name=\"{thatEnd.Name.ToSnakeCase()}_id\", nullable = {thatEnd.IsNullable.ToString().ToLower()})");
                }
            }
            else if (!sourceEnd.IsCollection && thatEnd.IsCollection) // one-to-many
            {
                var settings = new List<string>
                {
                    $"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}"
                };

                if (sourceEnd.IsNavigable)
                {
                    settings.Add($"mappedBy=\"{sourceEnd.Name.ToCamelCase()}\"");
                }

                if (!sourceEnd.IsNullable)
                {
                    settings.Add("orphanRemoval = true");
                }

                if (!string.IsNullOrWhiteSpace(fetchType))
                {
                    settings.Add(fetchType);
                }

                annotations.Add($"@{_template.ImportType("javax.persistence.OneToMany")}({string.Join(", ", settings)})");
            }
            else if (sourceEnd.IsCollection && !thatEnd.IsCollection) // many-to-one
            {
                var joinColumnParams = new List<string>();
                var manyToOneSettings = new List<string>
                {
                    $"optional = {thatEnd.IsNullable.ToString().ToLower()}",
                    $"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}"
                };

                if (!string.IsNullOrWhiteSpace(fetchType))
                {
                    manyToOneSettings.Add(fetchType);
                }

                // This needs to match up exactly to any potential FK field's column name and that
                // required to dig into how any potential explicit FK was set on the designer.
                var cleanedFkName = thatEnd.Name.Replace("_", " ").ToLower().ToCamelCase().Replace(" ", "");
                joinColumnParams.Add($@"name = ""{cleanedFkName.ToSnakeCase()}_id""");
                joinColumnParams.Add($@"nullable = {thatEnd.IsNullable.ToString().ToLower()}");

                var hasAggregationForeignKey = _template.Model.Attributes.Any(p => p.Name.Equals((thatEnd.Name + "Id").Replace("_", ""), StringComparison.OrdinalIgnoreCase));
                if (hasAggregationForeignKey)
                {
                    annotations.Add($@"@{_template.ImportType("lombok.Setter")}({_template.ImportType("lombok.AccessLevel")}.NONE)");

                    joinColumnParams.Add($@"insertable = false");
                    joinColumnParams.Add($@"updatable = false");
                }

                annotations.Add($"@{_template.ImportType("javax.persistence.ManyToOne")}({string.Join(", ", manyToOneSettings)})");
                annotations.Add($"@{_template.ImportType("javax.persistence.JoinColumn")}({string.Join(", ", joinColumnParams)})");
            }
            else if (sourceEnd.IsCollection && thatEnd.IsCollection) // many-to-many
            {
                var settings = new List<string>
                {
                    $"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}"
                };

                if (!string.IsNullOrWhiteSpace(fetchType))
                {
                    settings.Add(fetchType);
                }

                annotations.Add($"@{_template.ImportType($"javax.persistence.ManyToMany")}({string.Join(", ", settings)})");
                if (thatEnd.IsTargetEnd())
                {
                    var sourceEndName = sourceEnd.Element.Name;
                    var thatEndName = ApplyTableNameConvention(thatEnd.Element.Name);

                    annotations.Add($@"@{_template.ImportType("javax.persistence.JoinTable")}(
            name = ""{sourceEndName.ToSnakeCase()}_{thatEndName.ToSnakeCase()}"",
            joinColumns = {{ @{_template.ImportType("javax.persistence.JoinColumn")}(name = ""{sourceEnd.Element.Name.ToSnakeCase()}_id"") }},
            inverseJoinColumns = {{ @{_template.ImportType("javax.persistence.JoinColumn")}(name = ""{thatEnd.Element.Name.ToSnakeCase()}_id"") }}
    )");
                }
            }

            return string.Join(@"
    ", annotations);
        }

        private string ApplyTableNameConvention(string tableName)
        {
            return _application.Settings.GetDatabaseSettings()?.TableNamingConvention()?.AsEnum() switch
            {
                DatabaseSettingsExtensions.TableNamingConventionOptionsEnum.Pluralized => tableName.Pluralize(),
                DatabaseSettingsExtensions.TableNamingConventionOptionsEnum.Singularized => tableName.Singularize(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override IEnumerable<string> Methods()
        {
            if (_template.Model.ParentClass == null)
            {
                yield return @"public boolean isNew() {{
        return this.id == null;
    }}";
            }
        }
    }
}