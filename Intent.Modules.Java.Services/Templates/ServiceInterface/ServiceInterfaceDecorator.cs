using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceInterface
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public abstract class ServiceInterfaceDecorator : ITemplateDecorator
    {
        public int Priority { get; protected set; } = 0;

    }
}