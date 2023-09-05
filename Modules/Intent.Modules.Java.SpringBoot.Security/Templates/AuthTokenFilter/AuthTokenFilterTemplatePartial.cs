using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Events;
using Intent.Modules.Java.SpringBoot.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.AuthTokenFilter
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AuthTokenFilterTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Security.AuthTokenFilter";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public AuthTokenFilterTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootSecurity);
        }

        public override void BeforeTemplateExecution()
        {
            ExecutionContext.EventDispatcher.Publish(new UsesJwtSecurityEvent());

            base.BeforeTemplateExecution();
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"AuthTokenFilter",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private string JavaxJakarta()
        {
            return ExecutionContext.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => "javax",
                SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => "jakarta",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}