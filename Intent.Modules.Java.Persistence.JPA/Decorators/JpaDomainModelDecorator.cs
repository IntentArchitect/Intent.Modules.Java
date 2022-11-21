using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Java.Persistence.JPA.Api;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Metadata.RDBMS.Api.Indexes;
using Intent.Modules.Metadata.RDBMS.Settings;
using Intent.RoslynWeaver.Attributes;
using static Intent.Java.Persistence.JPA.Api.AssociationSourceEndModelStereotypeExtensions.AssociationJPASettings;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class JpaDomainModelDecorator : DomainModelDecorator, IDeclareImports
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

        public override string ClassAnnotations()
        {
            return $@"
@{_template.ImportType("javax.persistence.Entity")}
@{_template.ImportType("javax.persistence.Table")}({string.Join(", ", GetTableAttributes())})";
        }

        private IEnumerable<string> GetTableAttributes()
        {
            var attributes = new List<string>();

            attributes.Add($@"name = ""{(_template.Model.HasTable() ? _template.Model.GetTable().Name() : _template.Model.Name.ToPluralName().ToSnakeCase())}""");

            if (!string.IsNullOrEmpty(_template.Model.GetTable()?.Schema()))
            {
                attributes.Add($@"schema = ""{_template.Model.GetTable().Schema()}""");
            }
            
            if (_template.Model.Attributes.Any(x => x.HasIndex()) || _template.Model.GetIndexes().Any())
            {
                var stereotypeIndexes = _template.Model.Attributes
                    .Where(x => x.HasIndex())
                    .GroupBy(x => x.GetIndex().UniqueKey() ?? "IX_" + _template.Model.Name + "_" + x.Name)
                    .ToList();

                var elementIndexes = _template.Model.GetIndexes()
                    .ToList();

                var indexList = new List<string>();
                
                foreach (var stereotypeIndex in stereotypeIndexes)
                {
                    indexList.Add($"@{_template.ImportType("javax.persistence.Index")}(name = \"{stereotypeIndex.Key}\", columnList = \"{string.Join(",", stereotypeIndex.Select(c => c.Name.ToSnakeCase()))}\")");
                }

                foreach (var elementIndex in elementIndexes)
                {
                    indexList.Add($"@{_template.ImportType("javax.persistence.Index")}(name = \"{elementIndex.Name}\", columnList = \"{string.Join(",", elementIndex.KeyColumns.Select(c => c.Name.ToSnakeCase()))}\")");
                }

                const string newLine = @",
        ";
                attributes.Add($@"indexes = {string.Join(newLine, indexList)}");
            }
            
            return attributes;
        }

        public override string BeforeField(AttributeModel model)
        {
            var annotations = new List<string>();
            var columnSettings = new List<string>();

            if (model.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase) && !HasInheritedClass())
            {
                var (type, columnType) = _application.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
                {
                    DatabaseSettings.KeyTypeOptionsEnum.Guid => (_template.ImportType("java.util.UUID"), "uuid"),
                    DatabaseSettings.KeyTypeOptionsEnum.Long => ("Long", null),
                    DatabaseSettings.KeyTypeOptionsEnum.Int => ("Integer", null),
                    _ => throw new ArgumentOutOfRangeException()
                };

                annotations.Add($"@Id");
                annotations.Add($"@GeneratedValue(strategy = GenerationType.AUTO)");
                columnSettings.Add($@"columnDefinition = ""{columnType}""");
            }
            else
            {
                var columnName = !string.IsNullOrWhiteSpace(model.GetColumn()?.Name())
                    ? model.GetColumn().Name()
                    : model.Name.ToSnakeCase();
                columnSettings.Add($"name = \"{columnName}\"");
                if (_template.GetTypeName(model.TypeReference) == "String" && model.GetTextConstraints()?.MaxLength() != null)
                {
                    columnSettings.Add($"length = {model.GetTextConstraints().MaxLength()}");
                }

                if (!model.TypeReference.IsNullable)
                {
                    columnSettings.Add("nullable = false");
                }
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
                    annotations.Add($@"@{_template.ImportType("javax.persistence.JoinTable")}(
            name = ""{sourceEnd.Element.Name.ToSnakeCase()}_{thatEnd.Element.Name.ToPluralName().ToSnakeCase()}"",
            joinColumns = {{ @{_template.ImportType("javax.persistence.JoinColumn")}(name = ""{sourceEnd.Element.Name.ToSnakeCase()}_id"") }},
            inverseJoinColumns = {{ @{_template.ImportType("javax.persistence.JoinColumn")}(name = ""{thatEnd.Element.Name.ToSnakeCase()}_id"") }}
    )");
                }
            }

            return string.Join(@"
    ", annotations);
        }

//         public override string GetAdditionalFields()
//         {
//             if (HasInheritedClass())
//             {
//                 return string.Empty;
//             }
//             
//             var (type, columnType) = _application.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
//             {
//                 DatabaseSettings.KeyTypeOptionsEnum.Guid => (_template.ImportType("java.util.UUID"), "uuid"),
//                 DatabaseSettings.KeyTypeOptionsEnum.Long => ("Long", null),
//                 DatabaseSettings.KeyTypeOptionsEnum.Int => ("Integer", null),
//                 _ => throw new ArgumentOutOfRangeException()
//             };
//
//             var columnAnnotation = columnType != null
//                 ? @$"
//     @{_template.ImportType("javax.persistence.Column")}(columnDefinition = ""{columnType}"")"
//                 : string.Empty;
//                 
//             return $@"
//     @Id
//     @GeneratedValue(strategy = GenerationType.AUTO){columnAnnotation}
//     private {type} id;";
//         }
//
//         public override string GetAdditionalMethods()
//         {
//             if (HasInheritedClass())
//             {
//                 return string.Empty;
//             }
//             
//             var (type, columnType) = _application.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
//             {
//                 DatabaseSettings.KeyTypeOptionsEnum.Guid => (_template.ImportType("java.util.UUID"), "uuid"),
//                 DatabaseSettings.KeyTypeOptionsEnum.Long => ("Long", null),
//                 DatabaseSettings.KeyTypeOptionsEnum.Int => ("Integer", null),
//                 _ => throw new ArgumentOutOfRangeException()
//             };
//             
//             return $@"
//     public {type} getId() {{
//         return id;
//     }}
//
//     public void setId({type} id) {{
//         this.id = id;
//     }}
//
//     public boolean isNew() {{
//         return this.id == null;
//     }}
// ";
//         }

        public IEnumerable<string> DeclareImports()
        {
            if (HasInheritedClass())
            {
                yield break;
            }
            
            yield return "javax.persistence.GeneratedValue";
            yield return "javax.persistence.GenerationType";
            yield return "javax.persistence.Id";
            yield return "javax.persistence.MappedSuperclass";
        }
        
        private bool HasInheritedClass()
        {
            return _template.Model.ParentClass != null || 
                   _template.TryGetTypeName("Domain.AbstractEntity", out var abstractTemplateName);
        }
    }
}