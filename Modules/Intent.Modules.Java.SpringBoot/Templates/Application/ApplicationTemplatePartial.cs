using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Maven.Templates;
using Intent.Modules.Java.SpringBoot.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.Application
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public partial class ApplicationTemplate : JavaTemplateBase<object>, IJavaFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Application";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public ApplicationTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootStarter);
            AddDependency(JavaDependencies.SpringBootStarterWeb);

            JavaFile = new JavaFile(this.GetPackage(), this.GetFolderPath())
                .AddImport("org.springframework.boot.SpringApplication")
                .AddImport("org.springframework.boot.autoconfigure.SpringBootApplication")
                .AddClass($"Application", @class =>
                {
                    @class.AddAnnotation("SpringBootApplication");
                    @class.AddMethod("void", "main", method =>
                    {
                        method.Static();
                        method.AddParameter("String[]", "args", param => param.Final());
                        method.AddStatement($"SpringApplication.run(Application.class, args);");
                    });
                });
        }

        public override void BeforeTemplateExecution()
        {
            base.BeforeTemplateExecution();
            ExecutionContext.EventDispatcher.Publish(new MavenProjectInheritanceRequest("org.springframework.boot", "spring-boot-starter-parent", ExecutionContext.Settings.GetSpringBoot().TargetVersion().ToString()));
        }

        [IntentManaged(Mode.Fully)]
        public JavaFile JavaFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return JavaFile.GetConfig();
        }

        [IntentManaged(Mode.Fully)]
        public override string TransformText()
        {
            return JavaFile.ToString();
        }
    }
}