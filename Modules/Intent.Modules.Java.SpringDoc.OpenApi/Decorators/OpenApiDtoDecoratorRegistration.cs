using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.SpringDoc.OpenApi.Decorators
{
    [Description(OpenApiDtoDecorator.DecoratorId)]
    public class OpenApiDtoDecoratorRegistration : DecoratorRegistration<DataTransferModelTemplate, DataTransferModelDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public override DataTransferModelDecorator CreateDecoratorInstance(DataTransferModelTemplate template, IApplication application)
        {
            return new OpenApiDtoDecorator(template, application);
        }

        public override string DecoratorId => OpenApiDtoDecorator.DecoratorId;
    }
}