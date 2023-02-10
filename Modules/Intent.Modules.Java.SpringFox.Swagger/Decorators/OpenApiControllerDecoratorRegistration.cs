using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.SpringFox.Swagger.Decorators
{
    [Description(OpenApiControllerDecorator.DecoratorId)]
    public class OpenApiControllerDecoratorRegistration : DecoratorRegistration<RestControllerTemplate, RestControllerDecorator>
    {
        public override RestControllerDecorator CreateDecoratorInstance(RestControllerTemplate template, IApplication application)
        {
            return new OpenApiControllerDecorator(template, application);
        }

        public override string DecoratorId => OpenApiControllerDecorator.DecoratorId;
    }
}