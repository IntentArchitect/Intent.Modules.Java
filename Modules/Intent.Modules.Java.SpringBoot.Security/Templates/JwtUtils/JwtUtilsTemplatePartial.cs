using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Events;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.JwtUtils
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class JwtUtilsTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Security.JwtUtils";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public JwtUtilsTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootSecurity);
            AddDependency(JavaDependencies.JsonWebTokenJjwtApi);
            AddDependency(JavaDependencies.JsonWebTokenJjwtImpl);
            AddDependency(JavaDependencies.JsonWebTokenJjwtJackson);
        }

        public override void BeforeTemplateExecution()
        {
            ExecutionContext.EventDispatcher.Publish(new ApplicationPropertyRequest(
                name: "security.jwt.secret",
                value: "01234567890abcdef01234567890abcdef"));

            base.BeforeTemplateExecution();
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"JwtUtils",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

    }
}