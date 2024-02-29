using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.Enum
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EnumTemplate : JavaTemplateBase<Intent.Modules.Common.Types.Api.EnumModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.Enum";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public EnumTemplate(IOutputTarget outputTarget, Intent.Modules.Common.Types.Api.EnumModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name.ToPascalCase()}",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }
    }
}