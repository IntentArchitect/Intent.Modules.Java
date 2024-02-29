using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfigV3
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class WebSecurityConfigV3Template : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Security.WebSecurityConfigV3";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public WebSecurityConfigV3Template(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootSecurity);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"WebSecurityConfig",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        public override bool CanRunTemplate()
        {
            return ExecutionContext.Settings.GetSpringBoot().TargetVersion().IsV3_1_3();
        }
    }
}