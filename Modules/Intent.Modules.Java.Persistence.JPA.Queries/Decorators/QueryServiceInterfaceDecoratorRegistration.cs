using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [Description(QueryServiceInterfaceDecorator.DecoratorId)]
    public class QueryServiceInterfaceDecoratorRegistration : DecoratorRegistration<ServiceInterfaceTemplate, ServiceInterfaceDecorator>
    {
        public override ServiceInterfaceDecorator CreateDecoratorInstance(ServiceInterfaceTemplate template, IApplication application)
        {
            return new QueryServiceInterfaceDecorator(template, application);
        }

        public override string DecoratorId => QueryServiceInterfaceDecorator.DecoratorId;
    }
}