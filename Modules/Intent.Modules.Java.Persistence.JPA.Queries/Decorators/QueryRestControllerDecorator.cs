using Intent.Engine;
using Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryProjection;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class QueryRestControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.Queries.QueryRestControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public QueryRestControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddTypeSource(QueryProjectionTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
        }
    }
}