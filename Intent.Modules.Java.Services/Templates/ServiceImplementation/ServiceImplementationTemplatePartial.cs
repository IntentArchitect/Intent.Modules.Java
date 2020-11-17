using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceImplementation
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ServiceImplementationTemplate : JavaTemplateBase<ServiceModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.ServiceImplementation";

        public ServiceImplementationTemplate(IOutputTarget outputTarget, ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency("org.projectlombok", "lombok", "1.18.12"));
            AddTemplateDependency("Intent.Java.SpringBoot.IntentAnnotations");
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}Impl",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        public string IsReadOnly(OperationModel operation)
        {
            return operation.GetTransactionOptions().IsReadOnly().ToString().ToLower();
        }

    }
}