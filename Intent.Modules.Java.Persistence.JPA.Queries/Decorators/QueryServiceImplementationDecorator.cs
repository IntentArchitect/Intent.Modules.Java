using Intent.Engine;
using Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryResult;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class QueryServiceImplementationDecorator : ServiceImplementationDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.Queries.QueryServiceImplementationDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly ServiceImplementationTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public QueryServiceImplementationDecorator(ServiceImplementationTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddTypeSource(QueryResultTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
        }
    }
}