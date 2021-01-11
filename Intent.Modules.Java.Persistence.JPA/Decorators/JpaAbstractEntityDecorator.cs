using System.Collections.Generic;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.Domain.Templates.AbstractEntity;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class JpaAbstractEntityDecorator : AbstractEntityDecorator, IDeclareImports
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.JpaAbstractEntityDecorator";

        private readonly AbstractEntityTemplate _template;

        public JpaAbstractEntityDecorator(AbstractEntityTemplate template)
        {
            _template = template;
        }

        public override IEnumerable<string> ClassAnnotations()
        {
            yield return "@MappedSuperclass";
        }

        public override string Fields()
        {
            return @"
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @CreatedBy
    @Column(name = ""created_by"", length = 50)
    @JsonIgnore
    private String createdBy;

    @CreatedDate
    @Column(name = ""created_date"", updatable = false)
    @JsonIgnore
    private Instant createdDate = Instant.now();

    @LastModifiedBy
    @Column(name = ""last_modified_by"", length = 50)
    @JsonIgnore
    private String lastModifiedBy;

    @LastModifiedDate
    @Column(name = ""last_modified_date"")
    @JsonIgnore
    private Instant lastModifiedDate = Instant.now();
";
        }

        public IEnumerable<string> DeclareImports()
        {
            yield return "java.time.Instant";
            yield return "com.fasterxml.jackson.annotation.JsonIgnore";
            yield return "org.springframework.data.annotation.CreatedBy";
            yield return "org.springframework.data.annotation.CreatedDate";
            yield return "org.springframework.data.annotation.LastModifiedBy";
            yield return "org.springframework.data.annotation.LastModifiedDate";
            yield return "javax.persistence.Column";
            yield return "javax.persistence.GeneratedValue";
            yield return "javax.persistence.GenerationType";
            yield return "javax.persistence.Id";
            yield return "javax.persistence.MappedSuperclass";
            
        }
    }
}