using Intent.Engine;
using Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates.QueryResult;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class QueryRestControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.CustomQueries.QueryRestControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public QueryRestControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddTypeSource(QueryResultTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
        }
    }
}