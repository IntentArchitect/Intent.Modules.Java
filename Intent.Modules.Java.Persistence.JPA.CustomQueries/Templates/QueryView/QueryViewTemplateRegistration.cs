using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.Persistence.JPA.CustomQueries.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates.QueryView
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge)]
    public class QueryViewTemplateRegistration : FilePerModelTemplateRegistration<(CustomQueryModel Model, bool CanRun)>
    {
        private readonly IMetadataManager _metadataManager;

        public QueryViewTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => QueryViewTemplate.TemplateId;

        [IntentManaged(Mode.Ignore)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, (CustomQueryModel Model, bool CanRun) options)
        {
            return new QueryViewTemplate(outputTarget, options.Model, options.CanRun);
        }

        [IntentManaged(Mode.Ignore)]
        public override IEnumerable<(CustomQueryModel Model, bool CanRun)> GetModels(IApplication application)
        {
            var result = _metadataManager.Domain(application).GetCustomQueryModels()
                .Select(model => new
                {
                    Model = model,
                    ViewName = model.GetQuerySettings()?.ViewName()
                })
                .Where(element => !string.IsNullOrWhiteSpace(element.ViewName))
                .GroupBy(keySelector: element => element.ViewName)
                .SelectMany(grouping => grouping.Select((element, index) => (element.Model, index == 0)))
                .ToArray();

            return result;
        }
    }
}