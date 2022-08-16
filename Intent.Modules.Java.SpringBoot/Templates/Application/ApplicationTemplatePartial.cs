using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Maven.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.Application
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ApplicationTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Application";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ApplicationTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter", "2.3.1.RELEASE"));
            AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-web", "2.3.1.RELEASE"));
        }

        public override void BeforeTemplateExecution()
        {
            base.BeforeTemplateExecution();
            ExecutionContext.EventDispatcher.Publish(new MavenProjectInheritanceRequest("org.springframework.boot", "spring-boot-starter-parent", "2.3.1.RELEASE"));
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"Application",
                package: OutputTarget.GetPackage(),
                relativeLocation: OutputTarget.GetPackageFolderPath()
            );
        }

    }
}