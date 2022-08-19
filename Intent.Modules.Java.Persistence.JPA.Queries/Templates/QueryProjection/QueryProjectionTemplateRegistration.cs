using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.Persistence.JPA.Queries.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.FilePerModel", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryProjection
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class QueryProjectionTemplateRegistration : FilePerModelTemplateRegistration<QueryProjectionModel>
    {
        private readonly IMetadataManager _metadataManager;

        public QueryProjectionTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => QueryProjectionTemplate.TemplateId;

        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, QueryProjectionModel model)
        {
            return new QueryProjectionTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<QueryProjectionModel> GetModels(IApplication application)
        {
            return _metadataManager.Domain(application).GetQueryProjectionModels();
        }
    }
}