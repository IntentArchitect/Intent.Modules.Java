using Intent.Engine;
using Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryProjection;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class QueryServiceInterfaceDecorator : ServiceInterfaceDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Persistence.JPA.Queries.QueryServiceInterfaceDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly ServiceInterfaceTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public QueryServiceInterfaceDecorator(ServiceInterfaceTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddTypeSource(QueryProjectionTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
        }
    }
}