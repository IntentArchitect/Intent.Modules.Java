using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.Domain.Templates.AbstractEntity;
using Intent.Modules.Metadata.RDBMS.Settings;
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

        [IntentManaged(Mode.Fully)]
        private readonly AbstractEntityTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public JpaAbstractEntityDecorator(AbstractEntityTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-data-jpa"));
        }

        public override IEnumerable<string> ClassAnnotations()
        {
            yield return "@MappedSuperclass";
        }

        public override string Fields()
        {
            var (type, columnType) = _application.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
            {
                DatabaseSettings.KeyTypeOptionsEnum.Guid => (_template.ImportType("java.util.UUID"), "uuid"),
                DatabaseSettings.KeyTypeOptionsEnum.Long => ("Long", null),
                DatabaseSettings.KeyTypeOptionsEnum.Int => ("Integer", null),
                _ => throw new ArgumentOutOfRangeException()
            };

            var columnAnnotation = columnType != null
                ? @$"
    @{_template.ImportType("javax.persistence.Column")}(columnDefinition = ""{columnType}"")"
                : string.Empty;

            return $@"
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO){columnAnnotation}
    private {type} id;

    public {type} getId() {{
        return id;
    }}

    public void setId({type} id) {{
        this.id = id;
    }}

    public boolean isNew() {{
        return this.id == null;
    }}
";
        }

        public IEnumerable<string> DeclareImports()
        {
            yield return "javax.persistence.GeneratedValue";
            yield return "javax.persistence.GenerationType";
            yield return "javax.persistence.Id";
            yield return "javax.persistence.MappedSuperclass";
        }
    }
}