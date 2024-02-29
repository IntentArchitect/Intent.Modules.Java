using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Decorators
{
    [Description(JpaDomainModelDecorator.DecoratorId)]
    public class JpaDomainModelDecoratorRegistration : DecoratorRegistration<DomainModelTemplate, DomainModelDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public override DomainModelDecorator CreateDecoratorInstance(DomainModelTemplate template, IApplication application)
        {
            return new JpaDomainModelDecorator(template, application);
        }

        public override string DecoratorId => JpaDomainModelDecorator.DecoratorId;
    }
}