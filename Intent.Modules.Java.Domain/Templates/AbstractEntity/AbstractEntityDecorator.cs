using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.AbstractEntity
{
    [IntentManaged(Mode.Merge)]
    public abstract class AbstractEntityDecorator : ITemplateDecorator
    {
        public int Priority { get; protected set; } = 0;

        public virtual IEnumerable<string> ClassAnnotations() => null;

        public virtual string Fields() => null;

        public virtual string Methods() => null;
    }
}