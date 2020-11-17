using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.RestController
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class RestControllerTemplate : JavaTemplateBase<ServiceModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.RestController";

        public RestControllerTemplate(IOutputTarget outputTarget, ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter", "2.3.1.RELEASE"));
            AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-web", "2.3.1.RELEASE"));
        }

        public string RootName => (Model.Name.EndsWith("Service") ? Model.Name.Substring(0, Model.Name.Length - "Service".Length) : Model.Name);

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{RootName}Resource",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        private string GetApiPath()
        {
            return $"/{RootName.ToKebabCase()}";
        }

        private string GetServiceInterfaceName()
        {
            return GetTypeName(ServiceInterfaceTemplate.TemplateId, Model);
        }

        private string GetParameters(OperationModel operation)
        {
            return string.Join(", ", operation.Parameters.Select(GetParameter)
                    .Concat(new []{ "@RequestHeader Map<String, String> headers" }))
                ;
        }

        private string GetParameter(ParameterModel parameter)
        {
            return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? ", required = false" : "")}) {GetTypeName(parameter)} {parameter.Name}";
        }
    }
}