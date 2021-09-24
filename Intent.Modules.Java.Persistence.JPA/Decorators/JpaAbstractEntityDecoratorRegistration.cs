using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Domain.Templates.AbstractEntity;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Decorators
{
    [Description(JpaAbstractEntityDecorator.DecoratorId)]
    public class JpaAbstractEntityDecoratorRegistration : DecoratorRegistration<AbstractEntityTemplate, AbstractEntityDecorator>
    {
        public override AbstractEntityDecorator CreateDecoratorInstance(AbstractEntityTemplate template, IApplication application)
        {
            return new JpaAbstractEntityDecorator(template, application);
        }

        public override string DecoratorId => JpaAbstractEntityDecorator.DecoratorId;
    }
}