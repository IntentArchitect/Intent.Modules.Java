using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [Description(QueryServiceImplementationDecorator.DecoratorId)]
    public class QueryServiceImplementationDecoratorRegistration : DecoratorRegistration<ServiceImplementationTemplate, ServiceImplementationDecorator>
    {
        public override ServiceImplementationDecorator CreateDecoratorInstance(ServiceImplementationTemplate template, IApplication application)
        {
            return new QueryServiceImplementationDecorator(template, application);
        }

        public override string DecoratorId => QueryServiceImplementationDecorator.DecoratorId;
    }
}