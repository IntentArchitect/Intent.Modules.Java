using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.RoslynWeaver.Attributes;
using Intent.Engine;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class JpaDomainModelDecorator : DomainModelDecorator, IDeclareImports
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.JpaDomainModelDecorator";

        private readonly DomainModelTemplate _template;
        private readonly IApplication _application;
        private ICollection<string> _imports = new List<string>();

        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public JpaDomainModelDecorator(DomainModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-data-jpa"));
            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-jdbc"));
            _template.AddDependency(new JavaDependency("com.h2database", "h2"));
        }

        private string Use(string fullyQualifiedType, string import = null)
        {
            import = import ?? fullyQualifiedType;
            if (!_imports.Contains(import))
            {
                _imports.Add(import);
            }

            return fullyQualifiedType.Split('.').Last();
        }

        public override string ClassAnnotations()
        {
            return $@"
@Entity
@Table(name = ""{(_template.Model.HasTable() ? _template.Model.GetTable().Name() : _template.Model.Name.ToPluralName().ToSnakeCase())}"")";
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
            annotations.Add($@"@Column({string.Join(", ", columnSettings)})");

            return string.Join(@"
    ", annotations);
        }

        public override string BeforeField(AssociationEndModel thatEnd)
        {
            if (!thatEnd.IsNavigable)
            {
                throw new InvalidOperationException("Cannot call this method if associationEnd is not navigable.");
            }
            var annotations = new List<string>();
            var sourceEnd = thatEnd.OtherEnd();
            if (!sourceEnd.IsCollection && !thatEnd.IsCollection) // one-to-one
            {
                annotations.Add($"@OneToOne(optional = {thatEnd.IsNullable.ToString().ToLower()}{(sourceEnd.IsNullable ? "" : ", orphanRemoval = true")})");
                if (thatEnd.IsTargetEnd())
                {
                    annotations.Add($"@JoinColumn(name=\"{thatEnd.Name.ToSnakeCase()}_id\", nullable = {thatEnd.IsNullable.ToString().ToLower()})");
                }
            }
            else if (!sourceEnd.IsCollection && thatEnd.IsCollection) // one-to-many
            {
                var settings = new List<string>();
                if (sourceEnd.IsNavigable)
                {
                    settings.Add($"mappedBy=\"{sourceEnd.Name.ToCamelCase()}\"");
                }

                if (!sourceEnd.IsNullable)
                {
                    settings.Add("orphanRemoval = true");
                }
                annotations.Add($"@OneToMany({string.Join(", ", settings)})");
            }
            else if (sourceEnd.IsCollection && !thatEnd.IsCollection) // many-to-one
            {
                annotations.Add($"@ManyToOne(optional = {thatEnd.IsNullable.ToString().ToLower()})");
                //if (thatEnd.IsTargetEnd())
                //{
                annotations.Add($"@JoinColumn(name=\"{thatEnd.Name.ToSnakeCase()}_id\", nullable = {thatEnd.IsNullable.ToString().ToLower()})");
                //}
            }
            else if (sourceEnd.IsCollection && thatEnd.IsCollection) // many-to-many
            {
                annotations.Add($"@ManyToMany");
                if (thatEnd.IsTargetEnd())
                {
                    annotations.Add($@"@JoinTable(
            name = ""{sourceEnd.Element.Name.ToSnakeCase()}_{thatEnd.Element.Name.ToPluralName().ToSnakeCase()}"",
            joinColumns = {{ @JoinColumn(name = ""{sourceEnd.Element.Name.ToSnakeCase()}_id"") }},
            inverseJoinColumns = {{ @JoinColumn(name = ""{thatEnd.Element.Name.ToSnakeCase()}_id"") }}
    )");
                }
            }

            return string.Join(@"
    ", annotations);
        }

        public IEnumerable<string> DeclareImports()
        {
            return _imports.Concat(new[]
            {
                "javax.persistence.*",
            }).Distinct();

        }
    }
}