using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Decorators
{
    [Description(QueryRestControllerDecorator.DecoratorId)]
    public class QueryRestControllerDecoratorRegistration : DecoratorRegistration<RestControllerTemplate, RestControllerDecorator>
    {
        public override RestControllerDecorator CreateDecoratorInstance(RestControllerTemplate template, IApplication application)
        {
            return new QueryRestControllerDecorator(template, application);
        }

        public override string DecoratorId => QueryRestControllerDecorator.DecoratorId;
    }
}