using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using Intent.Utils;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Java.Services.CRUD.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class CrudServiceFactoryExtension : FactoryExtensionBase
    {
        public override string Id => "Intent.Java.Services.CRUD.CrudServiceFactoryExtension";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        /// <summary>
        /// This is an example override which would extend the
        /// <see cref="ExecutionLifeCycleSteps.Start"/> phase of the Software Factory execution.
        /// See <see cref="FactoryExtensionBase"/> for all available overrides.
        /// </summary>
        /// <remarks>
        /// It is safe to update or delete this method.
        /// </remarks>
        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            var template = application.FindTemplateInstance<ServiceImplementationTemplate>(TemplateDependency.OnTemplate(ServiceImplementationTemplate.TemplateId));
            if (template != null)
            {
                var strategies = new List<IImplementationStrategy>
                {
                    new GetAllImplementationStrategy(template, application),
                    new GetByIdImplementationStrategy(template, application),
                    new CreateImplementationStrategy(template, application),
                    new UpdateImplementationStrategy(template, application),
                    new DeleteImplementationStrategy(template, application)
                };

                foreach (var operation in template.Model.Operations)
                {
                    var matchedStrategies = strategies.Where(strategy => strategy.IsMatch(operation)).ToArray();
                    if (matchedStrategies.Length == 1)
                    {
                        template.JavaFile.AfterBuild(file => matchedStrategies[0].ApplyStrategy(operation));
                    }
                    else if (matchedStrategies.Length > 1)
                    {
                        Logging.Log.Warning($@"Multiple CRUD implementation strategies were found that can implement this service operation [{template.Model.Name}, {operation.Name}]");
                        Logging.Log.Debug($@"Strategies: {string.Join(", ", matchedStrategies.Select(s => s.GetType().Name))}");
                    }
                }
            }
        }
    }
}