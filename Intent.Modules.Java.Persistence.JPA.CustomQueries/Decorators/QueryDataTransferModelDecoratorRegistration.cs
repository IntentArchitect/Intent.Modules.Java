using System.ComponentModel;
using Intent.Engine;
using Intent.Modules.Common.Registrations;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorRegistration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Decorators
{
    [Description(QueryDataTransferModelDecorator.DecoratorId)]
    public class QueryDataTransferModelDecoratorRegistration : DecoratorRegistration<DataTransferModelTemplate, DataTransferModelDecorator>
    {
        public override DataTransferModelDecorator CreateDecoratorInstance(DataTransferModelTemplate template, IApplication application)
        {
            return new QueryDataTransferModelDecorator(template, application);
        }

        public override string DecoratorId => QueryDataTransferModelDecorator.DecoratorId;
    }
}