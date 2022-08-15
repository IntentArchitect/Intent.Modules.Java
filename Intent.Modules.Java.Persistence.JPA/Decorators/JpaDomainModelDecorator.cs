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
using Intent.RoslynWeaver.Attributes;

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

        public override string ClassAnnotations()
        {
            return $@"
@{_template.ImportType("javax.persistence.Entity")}
@{_template.ImportType("javax.persistence.Table")}(name = ""{(_template.Model.HasTable() ? _template.Model.GetTable().Name() : _template.Model.Name.ToPluralName().ToSnakeCase())}""{GetIndexes()})";
        }

        private string GetIndexes()
        {
            if (_template.Model.Attributes.Any(x => x.HasIndex()))
            {
                var indexes = _template.Model.Attributes
                    .Where(x => x.HasIndex())
                    .GroupBy(x => x.GetIndex().UniqueKey() ?? "IX_" + _template.Model.Name + "_" + x.Name);

                return $@", indexes = {{
    {string.Join(@",
    ", indexes.Select(x => $"@{_template.ImportType("javax.persistence.Index")}(name = \"{x.Key}\", columnList = \"{string.Join(",", x.Select(c => c.Name.ToCamelCase()))}\")"))}
}}";
            }

            return string.Empty;
        }

        public override string BeforeField(AttributeModel model)
        {
            var annotations = new List<string>();
            var columnSettings = new List<string>();
            columnSettings.Add($"name = \"{model.Name.ToSnakeCase()}\"");
            if (_template.GetTypeName(model.TypeReference) == "String" && model.GetTextConstraints()?.MaxLength() != null)
            {
                columnSettings.Add($"length = {model.GetTextConstraints().MaxLength()}");
            }

            if (!model.TypeReference.IsNullable)
            {
                columnSettings.Add("nullable = false");
            }
            annotations.Add($@"@{_template.ImportType("javax.persistence.Column")}({string.Join(", ", columnSettings)})");

            return string.Join(@"
    ", annotations);
        }

        public override string BeforeField(AssociationEndModel thatEnd)
        {
            var fetchType = thatEnd switch
            {
                AssociationSourceEndModel endModel => endModel.GetAssociationJPASettings()?.FetchType().Value,
                AssociationTargetEndModel endModel => endModel.GetAssociationJPASettings()?.FetchType().Value,
                _ => string.Empty
            };

            if (!string.IsNullOrWhiteSpace(fetchType))
            {
                fetchType = $"fetch = {_template.ImportType("javax.persistence.FetchType")}.{fetchType.ToUpperInvariant()}";
            }

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
                var settings = new List<string>
                {
                    $"optional = {thatEnd.IsNullable.ToString().ToLower()}",
                    $"cascade = {{ {_template.ImportType("javax.persistence.CascadeType")}.ALL }}"
                };

                if (!string.IsNullOrWhiteSpace(fetchType))
                {
                    settings.Add(fetchType);
                }

                annotations.Add($"@{_template.ImportType("javax.persistence.ManyToOne")}({string.Join(", ", settings)})");
                annotations.Add($"@{_template.ImportType("javax.persistence.JoinColumn")}(name=\"{thatEnd.Name.ToSnakeCase()}_id\", nullable = {thatEnd.IsNullable.ToString().ToLower()})");
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
    }
}