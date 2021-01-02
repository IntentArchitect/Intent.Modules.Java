using System;
using System.Collections.Generic;
using System.Text;
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

        public JpaDomainModelDecorator(DomainModelTemplate template)
        {
            _template = template;
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
            if (_template.GetTypeName(model.TypeReference) == "String")
            {
                annotations.Add("@Size(max = 60)");
                columnSettings.Add("length = 60");
            }

            if (model.Name.ToLower().EndsWith("email"))
            {
                annotations.Add("@Email");
            }

            if (!model.TypeReference.IsNullable)
            {
                annotations.Add("@NotNull");
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
            name = {sourceEnd.Element.Name.ToSnakeCase()}_{thatEnd.Element.Name.ToSnakeCase()},
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
            yield return "lombok.Data";
            yield return "lombok.EqualsAndHashCode";
            yield return "lombok.NonNull";
            yield return "javax.persistence.*";
            yield return "javax.validation.constraints.Email";
            yield return "javax.validation.constraints.NotNull";
            yield return "javax.validation.constraints.Pattern";
            yield return "javax.validation.constraints.Size";
        }
    }
}