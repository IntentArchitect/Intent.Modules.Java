using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Validation.Decorators
{
    [Description(ValidationRestControllerDecorator.DecoratorId)]
    public class ValidationRestControllerDecoratorRegistration : DecoratorRegistration<RestControllerTemplate, RestControllerDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public override RestControllerDecorator CreateDecoratorInstance(RestControllerTemplate template, IApplication application)
        {
            return new ValidationRestControllerDecorator(template, application);
        }

        public override string DecoratorId => ValidationRestControllerDecorator.DecoratorId;
    }
}