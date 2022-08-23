using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Validation.Decorators
{
    [Description(DtoValidationDecorator.DecoratorId)]
    public class DtoValidationDecoratorRegistration : DecoratorRegistration<DataTransferModelTemplate, DataTransferModelDecorator>
    {
        public override DataTransferModelDecorator CreateDecoratorInstance(DataTransferModelTemplate template, IApplication application)
        {
            return new DtoValidationDecorator(template, application);
        }

        public override string DecoratorId => DtoValidationDecorator.DecoratorId;
    }
}