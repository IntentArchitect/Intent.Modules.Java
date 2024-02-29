using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Java.Persistence.JPA.Queries.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryProjection
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class QueryProjectionTemplate : JavaTemplateBase<Intent.Java.Persistence.JPA.Queries.Api.QueryProjectionModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Persistence.JPA.Queries.QueryProjection";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public QueryProjectionTemplate(IOutputTarget outputTarget, Intent.Java.Persistence.JPA.Queries.Api.QueryProjectionModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }
    }
}