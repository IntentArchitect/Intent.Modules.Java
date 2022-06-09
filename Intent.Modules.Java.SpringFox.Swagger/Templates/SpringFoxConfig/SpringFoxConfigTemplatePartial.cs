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

namespace Intent.Modules.Java.SpringFox.Swagger.Templates.SpringFoxConfig
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class SpringFoxConfigTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringFox.Swagger.SpringFoxConfig";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public SpringFoxConfigTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency("io.springfox", "springfox-boot-starter", "3.0.0"));
            AddDependency(new JavaDependency("io.springfox", "springfox-swagger-ui", "3.0.0"));
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"SpringFoxConfig",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

    }
}