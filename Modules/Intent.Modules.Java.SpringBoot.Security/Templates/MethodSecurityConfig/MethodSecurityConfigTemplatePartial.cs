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

namespace Intent.Modules.Java.SpringBoot.Security.Templates.MethodSecurityConfig
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class MethodSecurityConfigTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Security.MethodSecurityConfig";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public MethodSecurityConfigTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootSecurity);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"MethodSecurityConfig",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private string GetClassInheritance()
        {
            return ExecutionContext.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => $" extends {ImportType("org.springframework.security.config.annotation.method.configuration.GlobalMethodSecurityConfiguration")}",
                Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => string.Empty,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string GetMethodSecurityConfigurationAnnotationName()
        {
            return ExecutionContext.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => "EnableGlobalMethodSecurity",
                Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => "EnableMethodSecurity",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}