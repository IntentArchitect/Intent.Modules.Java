using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.JsonResponse
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class JsonResponseTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.JsonResponse";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public JsonResponseTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"JsonResponse",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        public override bool CanRunTemplate()
        {
            return ExecutionContext
                .MetadataManager
                .Services(ExecutionContext.GetApplicationConfig().Id)
                .GetServiceModels()
                .SelectMany(s => s.Operations)
                .Any(p => p.GetHttpSettings()?.ReturnTypeMediatype()?.IsApplicationJson() == true);
        }
    }
}