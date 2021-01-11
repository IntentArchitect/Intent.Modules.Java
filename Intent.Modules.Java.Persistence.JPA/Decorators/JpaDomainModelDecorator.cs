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
        private ICollection<string> _imports = new List<string>();

        public JpaDomainModelDecorator(DomainModelTemplate template)
        {
            _template = template;
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
@EqualsAndHashCode(callSuper = true)
@Entity
@Table(name = ""{_template.Model.Name.ToSnakeCase()}"")
@Data";
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

            return @"
    " + string.Join(@"
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
                annotations.Add($"@OneToMany({(sourceEnd.IsNullable ? "" : "orphanRemoval = true")})");
            }
            else if (sourceEnd.IsCollection && !thatEnd.IsCollection) // many-to-one
            {
                annotations.Add($"@ManyToOne(optional = {thatEnd.IsNullable.ToString().ToLower()})");
                if (thatEnd.IsTargetEnd())
                {
                    annotations.Add($"@JoinColumn(name=\"{thatEnd.Name.ToSnakeCase()}_id\", nullable = {thatEnd.IsNullable.ToString().ToLower()})");
                }
            }
            else if (sourceEnd.IsCollection && thatEnd.IsCollection) // many-to-many
            {
                annotations.Add($"@ManyToMany");
                if (thatEnd.IsTargetEnd())
                {
                    annotations.Add($@"@JoinTable(
            name = ""{sourceEnd.Element.Name.ToSnakeCase()}_{thatEnd.Element.Name.ToSnakeCase()}"",
            joinColumns = {{ @JoinColumn(name = ""{sourceEnd.Element.Name.ToSnakeCase()}_id"") }},
            inverseJoinColumns = {{ @JoinColumn(name = ""{thatEnd.Element.Name.ToSnakeCase()}_id"") }}
    )");
                }
            }

            return @"
    " + string.Join(@"
    ", annotations);
        }

        public IEnumerable<string> DeclareImports()
        {
            return _imports.Concat(new[]
            {
                "lombok.Data",
                "lombok.EqualsAndHashCode",
                "javax.persistence.*",
            }).Distinct();

        }
    }
}