using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Api;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceImplementation
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ServiceImplementationTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.ServiceImplementation";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ServiceImplementationTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency("org.projectlombok", "lombok", "1.18.12"));
            AddTemplateDependency("Intent.Java.SpringBoot.IntentAnnotations");
            AddTypeSource(DataTransferModelTemplate.TemplateId, type => $"{ImportType("java.util.List")}<{type}>");
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name.RemoveSuffix("Controller", "Service")}ServiceImpl",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        public string IsReadOnly(OperationModel operation)
        {
            return operation.GetTransactionOptions()?.IsReadOnly().ToString().ToLower() ??
                (operation.GetStereotype("Http Settings")?.GetProperty<string>("Verb") == "GET" ? "true" : "false");
        }

    }
}