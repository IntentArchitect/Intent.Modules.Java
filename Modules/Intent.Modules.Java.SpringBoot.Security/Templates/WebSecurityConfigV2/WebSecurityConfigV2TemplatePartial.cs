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

namespace Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfigV2
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class WebSecurityConfigV2Template : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Security.WebSecurityConfigV2";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public WebSecurityConfigV2Template(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootSecurity);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
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
            return ExecutionContext.Settings.GetSpringBoot().TargetVersion().IsV2_7_5();
        }
    }
}