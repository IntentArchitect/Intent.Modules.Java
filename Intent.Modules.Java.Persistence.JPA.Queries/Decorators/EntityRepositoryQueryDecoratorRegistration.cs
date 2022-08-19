using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators
{
    [Description(EntityRepositoryQueryDecorator.DecoratorId)]
    public class EntityRepositoryQueryDecoratorRegistration : DecoratorRegistration<EntityRepositoryTemplate, EntityRepositoryDecorator>
    {
        public override EntityRepositoryDecorator CreateDecoratorInstance(EntityRepositoryTemplate template, IApplication application)
        {
            return new EntityRepositoryQueryDecorator(template, application);
        }

        public override string DecoratorId => EntityRepositoryQueryDecorator.DecoratorId;
    }
}