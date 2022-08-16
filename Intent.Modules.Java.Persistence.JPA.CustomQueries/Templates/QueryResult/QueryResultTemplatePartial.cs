using System.Collections.Generic;
using Intent.Engine;
using Intent.Java.Persistence.JPA.CustomQueries.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates.QueryResult
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class QueryResultTemplate : JavaTemplateBase<Intent.Java.Persistence.JPA.CustomQueries.Api.QueryResultModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Persistence.JPA.CustomQueries.QueryResult";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public QueryResultTemplate(IOutputTarget outputTarget, Intent.Java.Persistence.JPA.CustomQueries.Api.QueryResultModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}",
                package: this.GetPackage(),
                relativeLocation: this.GetPackageFolderPath()
            );
        }
    }
}