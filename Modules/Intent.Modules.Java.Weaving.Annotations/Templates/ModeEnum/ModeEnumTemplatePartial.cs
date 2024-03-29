using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Weaving.Annotations.Templates.ModeEnum
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ModeEnumTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Weaving.Annotations.ModeEnum";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ModeEnumTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"Mode",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

    }
}