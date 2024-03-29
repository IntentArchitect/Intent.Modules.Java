using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Services.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.RestController
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public abstract class RestControllerDecorator : ITemplateDecorator
    {
        public int Priority { get; protected set; } = 0;

        public virtual IEnumerable<string> ControllerAnnotations() => Enumerable.Empty<string>();

        public virtual IEnumerable<string> OperationAnnotations(OperationModel operation) => Enumerable.Empty<string>();

        public virtual IEnumerable<string> ParameterAnnotations(ParameterModel parameter) => Enumerable.Empty<string>();
    }
}