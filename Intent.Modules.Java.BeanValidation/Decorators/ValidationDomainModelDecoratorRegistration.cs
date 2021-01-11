using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.BeanValidation.Decorators
{
    [Description(ValidationDomainModelDecorator.DecoratorId)]
    public class ValidationDomainModelDecoratorRegistration : DecoratorRegistration<DomainModelTemplate, DomainModelDecorator>
    {
        public override DomainModelDecorator CreateDecoratorInstance(DomainModelTemplate template, IApplication application)
        {
            return new ValidationDomainModelDecorator(template);
        }

        public override string DecoratorId => ValidationDomainModelDecorator.DecoratorId;
    }
}