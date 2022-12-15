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

        private static readonly object LockObject = new();
        private static bool _onceOffInitializationComplete;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public JpaDomainModelDecorator(DomainModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override void BeforeTemplateExecution()
        {
            lock (LockObject)
            {
                if (_onceOffInitializationComplete)
                {
                    return;
                }

                _onceOffInitializationComplete = true;
            }

            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-data-jpa"));
            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-jdbc"));

            // https://github.com/vladmihalcea/hibernate-types#installation
            _template.AddDependency(new JavaDependency("com.vladmihalcea", "hibernate-types-55", "2.20.0"));

            switch (_template.ExecutionContext.Settings.GetDatabaseSettings().DatabaseProvider().AsEnum())
            {
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.H2:
                    _template.AddDependency(new JavaDependency("com.h2database", "h2"));
                    break;
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Mysql:
                    _template.AddDependency(new JavaDependency("com.mysql", "mysql-connector-j"));
                    _template.ApplyApplicationProperty("spring.datasource.url",
                        $"jdbc:mysql://localhost:3306/{_application.Name.ToCamelCase()}?useUnicode=true");
                    _template.ApplyApplicationProperty("spring.datasource.username",
                        $"{_application.Name.ToCamelCase()}");
                    _template.ApplyApplicationProperty("spring.datasource.password", "");
                    _template.ApplyApplicationProperty("spring.jpa.hibernate.ddl-auto", "update");
                    break;
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Postgresql:
                    _template.AddDependency(new JavaDependency("org.postgresql", "postgresql"));
                    _template.ApplyApplicationProperty("spring.datasource.url",
                        $"jdbc:postgresql://localhost:5432/{_application.Name.ToCamelCase()}");
                    _template.ApplyApplicationProperty("spring.datasource.username",
                        $"{_application.Name.ToCamelCase()}");
                    _template.ApplyApplicationProperty("spring.datasource.password", "");
                    _template.ApplyApplicationProperty("spring.jpa.hibernate.ddl-auto", "update");
                    break;
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.SqlServer:
                    _template.AddDependency(new JavaDependency("com.microsoft.sqlserver", "mssql-jdbc"));

                    // https://learn.microsoft.com/azure/developer/java/spring-framework/configure-spring-data-jpa-with-azure-sql-server#configure-spring-boot-to-use-azure-sql-database
                    _template.ApplyApplicationProperty("spring.datasource.url",
                        $"jdbc:sqlserver://localhost:1433;database={_application.Name.ToCamelCase()};encrypt=true;trustServerCertificate=true;loginTimeout=30;");
                    _template.ApplyApplicationProperty("spring.datasource.username",
                        $"{_application.Name.ToCamelCase()}");
                    _template.ApplyApplicationProperty("spring.datasource.password", "");
                    _template.ApplyApplicationProperty("spring.jpa.hibernate.ddl-auto", "update");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

            if (_template.Model.Attributes.Any(IsJsonColumn))
            {
                yield return $"@{_template.ImportType("org.hibernate.annotations.TypeDef")}(name = \"json\", typeClass = {_template.ImportType("com.vladmihalcea.hibernate.type.json.JsonType")}.class)";
            }

            if (TryGetSecondaryTableName(out _))
            {
                yield return $"@{_template.ImportType("javax.persistence.SecondaryTable")}(name = {_template.ClassName}.TABLE_NAME)";
            }
            if (_template.Model.HasTable() ||
                !_template.Model.IsAbstract &&
                _template.Model.GetParentClasses().All(x => x.IsAbstract))
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

        private static bool HasPrimaryKey(ClassModel model)
        {
            if (model == null)
            {
                return false;
            }

            return model.Attributes.Any(IsPrimaryKey) ||
                   model.GetParentClasses().Any(x => !x.IsAbstract || x.Attributes.Any(IsPrimaryKey));

            static bool IsPrimaryKey(AttributeModel attribute)
            {
                return attribute.HasPrimaryKey() || attribute.Name.Equals("id", StringComparison.OrdinalIgnoreCase);
            }
        }

        private bool TryGetSecondaryTableName(out string tableName)
        {
            // Table-per-class pattern as per:
            // https://thorben-janssen.com/hibernate-mix-inheritance-mappings/
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

            if (!HasPrimaryKey(_template.Model))
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
    @{_template.ImportType("javax.persistence.Column")}(columnDefinition = ""{columnType}"", name = ""id"", nullable = false)
    private {type} id;";
            }
        }

        public override string FieldAnnotations(AttributeModel model)
        {
            var annotations = new List<string>();
            var columnSettings = new List<string>();

            if (IsJsonColumn(model))
            {
                annotations.Add($"@{_template.ImportType("org.hibernate.annotations.Type")}(type = \"json\")");
            }

            if (model.HasPrimaryKey() || (
                    model.Name.Equals("id", StringComparison.OrdinalIgnoreCase) &&
                    !_template.Model.Attributes.Any(x => x.HasPrimaryKey()) &&
                    !HasPrimaryKey(_template.Model.ParentClass)))
            {
                annotations.Add($"@{_template.ImportType("javax.persistence.Id")}");
                annotations.Add($"@{_template.ImportType("javax.persistence.GeneratedValue")}(strategy = {_template.ImportType("javax.persistence.GenerationType")}.AUTO)");
            }

            if (!string.IsNullOrWhiteSpace(model.GetColumn()?.Type()))
            {
                columnSettings.Add($"columnDefinition = \"{model.GetColumn().Type()}\"");
            }
            else if (model.TypeReference.Element.Name.ToLowerInvariant() is "guid" or "uuid")
            {
                var columnDefinition = _application.Settings.GetDatabaseSettings().DatabaseProvider().AsEnum() switch
                {
                    DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.H2 => "uuid",
                    DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Mysql => "CHAR(36)",
                    DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Postgresql => "uuid",
                    DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.SqlServer => "uniqueidentifier",
                    _ => throw new ArgumentOutOfRangeException()
                };

                columnSettings.Add($"columnDefinition = \"{columnDefinition}\"");
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

        public override string FieldAnnotations(AssociationEndModel thatEnd)
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
                    $"optional = {thatEnd.IsNullable.ToString().ToLower()}"
                };

                if (IsCompositeOwner(thatEnd))
                {
                    settings.Add($"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}");
                }

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
                var settings = new List<string>();

                if (IsCompositeOwner(thatEnd))
                {
                    settings.Add($"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}");
                }

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
                    $"optional = {thatEnd.IsNullable.ToString().ToLower()}"
                };

                if (IsCompositeOwner(thatEnd))
                {
                    manyToOneSettings.Add($"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}");
                }

                if (!string.IsNullOrWhiteSpace(fetchType))
                {
                    manyToOneSettings.Add(fetchType);
                }

                joinColumnParams.Add($@"name = ""{thatEnd.Name.ToSnakeCase()}_id""");
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
                var settings = new List<string>();

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

            static bool IsCompositeOwner(AssociationEndModel associationEnd)
            {
                return associationEnd.Equals(associationEnd.Association.TargetEnd) &&
                       associationEnd.Association.SourceEnd.Multiplicity == Multiplicity.One;
            }
        }

        private bool IsJsonColumn(AttributeModel model)
        {
            return _application.Settings.GetDatabaseSettings().DatabaseProvider().AsEnum() switch
            {
                DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.H2 =>
                    model.GetColumn()?.Type() is "json",
                DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Mysql =>
                    model.GetColumn()?.Type() is "json",
                DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Postgresql =>
                    model.GetColumn()?.Type() is "json" or "jsonb",
                // EG: NVARCHAR(max) CHECK(ISJSON(<columnName>) = 1)
                DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.SqlServer =>
                    model.GetColumn()?.Type().Contains("ISJSON(") == true,
                _ => throw new ArgumentOutOfRangeException()
            };
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
            var primaryKeys = _template.Model.GetPrimaryKeys();
            if (primaryKeys.FromClass != null && primaryKeys.FromClass != _template.Model)
            {
                // Will be handled by base class where the primary id(s) is defined
                yield break;
            }

            var checks = primaryKeys.PrimaryKeys
                .Select(x => $"this.{x.Name.ToCamelCase()} == null")
                .ToArray();

            if (checks.Length == 0)
            {
                // Implicit primary key field name:
                checks = new[] { "this.id == null" };
            }

            if (_template.Model.ParentClass == null)
            {
                yield return @$"public boolean isNew() {{
        return {string.Join(@" &&
               ", checks)};
    }}";
            }
        }
    }
}