using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Cors.Decorators
{
    [Description(CorsRestControllerDecorator.DecoratorId)]
    public class CorsRestControllerDecoratorRegistration : DecoratorRegistration<RestControllerTemplate, RestControllerDecorator>
    {
        public override RestControllerDecorator CreateDecoratorInstance(RestControllerTemplate template, IApplication application)
        {
            return new CorsRestControllerDecorator(template, application);
        }

        public override string DecoratorId => CorsRestControllerDecorator.DecoratorId;
    }
}