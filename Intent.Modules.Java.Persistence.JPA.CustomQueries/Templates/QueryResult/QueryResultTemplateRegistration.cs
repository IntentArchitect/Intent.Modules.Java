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

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates.QueryResult
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class QueryResultTemplateRegistration : FilePerModelTemplateRegistration<QueryResultModel>
    {
        private readonly IMetadataManager _metadataManager;

        public QueryResultTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public override string TemplateId => QueryResultTemplate.TemplateId;

        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, QueryResultModel model)
        {
            return new QueryResultTemplate(outputTarget, model);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IEnumerable<QueryResultModel> GetModels(IApplication application)
        {
            return _metadataManager.Domain(application).GetQueryResultModels();
        }
    }
}