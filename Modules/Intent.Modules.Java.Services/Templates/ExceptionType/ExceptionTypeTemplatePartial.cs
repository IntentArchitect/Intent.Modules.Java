using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ExceptionType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ExceptionTypeTemplate : JavaTemplateBase<Intent.Modules.Java.Services.Api.ExceptionTypeModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.ExceptionType";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ExceptionTypeTemplate(IOutputTarget outputTarget, Intent.Modules.Java.Services.Api.ExceptionTypeModel model) : base(TemplateId, outputTarget, model)
        {
        }

        public override bool CanRunTemplate()
        {
            return Model.GetExceptionSettings()?.IsExternal() != true &&
                   base.CanRunTemplate();
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            var package = Model.GetExceptionSettings()?.IsExternal() == true
                ? Model.GetExceptionSettings().Package()
                : this.GetPackage();

            return new JavaFileConfig(
                className: $"{Model.Name}",
                package: package,
                relativeLocation: this.GetFolderPath()
            );
        }

    }
}