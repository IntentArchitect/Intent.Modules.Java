using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Services.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.DataTransferModel
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public abstract class DataTransferModelDecorator : ITemplateDecorator
    {
        public int Priority { get; protected set; } = 0;

        public virtual IEnumerable<string> GetClassAnnotations() => Enumerable.Empty<string>();

        public virtual IEnumerable<string> GetFieldAnnotations(DTOFieldModel field) => Enumerable.Empty<string>();

        public virtual string Methods() => null;
    }
}