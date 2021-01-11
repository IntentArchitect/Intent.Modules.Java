using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.RestController
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class RestControllerTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.RestController";

        public RestControllerTemplate(IOutputTarget outputTarget, ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(DataTransferModelTemplate.TemplateId, "List<{0}>");
            AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter", "2.3.1.RELEASE"));
            AddDependency(new JavaDependency("org.springframework.boot", "spring-boot-starter-web", "2.3.1.RELEASE"));
        }

        public string RootName => Model.Name.RemoveSuffix("Service", "Controller", "Resource");

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{RootName}Controller",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        private string GetControllerAnnotations()
        {
            var annotations = new List<string>();
            //if (Model.HasSecured())
            //{
            //    attributes.Add("[Authorize]");
            //}
            annotations.Add($@"@RequestMapping(""/{(string.IsNullOrWhiteSpace(Model.GetHttpServiceSettings().Route())
                ? $"api/{RootName.ToKebabCase()}"
                : Model.GetHttpServiceSettings().Route().RemovePrefix("/"))}"")");

            return string.Join(@"
    ", annotations);
        }

        private string GetServiceInterfaceName()
        {
            return GetTypeName(ServiceInterfaceTemplate.TemplateId, Model);
        }

        private string GetOperationAnnotations(OperationModel operation)
        {
            var annotations = new List<string>();
            //if (Model.HasSecured())
            //{
            //    attributes.Add("[Authorize]");
            //}
            annotations.Add(GetPath(operation) != null
                ? $@"@{GetHttpVerb(operation).ToString().ToLower().ToPascalCase()}Mapping(""{GetPath(operation)}"")"
                : $@"@{GetHttpVerb(operation).ToString().ToLower().ToPascalCase()}Mapping");

            return string.Join(@"
    ", annotations);
        }

        private string GetReturnType(OperationModel operation)
        {
            if (operation.ReturnType == null)
            {
                return "void";
            }
            return $"ResponseEntity<{GetTypeName(operation.TypeReference)}>";
        }

        private string GetPath(OperationModel operation)
        {
            var path = operation.GetHttpSettings().Route();
            return !string.IsNullOrWhiteSpace(path) ? $"/{path.RemovePrefix("/")}" : null;
        }

        private string GetParameters(OperationModel operation)
        {
            return string.Join(", ", operation.Parameters.Select(param => GetParameter(operation, param))
                    .Concat(new[] { "@RequestHeader Map<String, String> headers" }))
                ;
        }

        private string GetParameter(OperationModel operation,  ParameterModel parameter)
        {
            return $"{GetParameterBindingAttribute(operation, parameter)}{GetTypeName(parameter)} {parameter.Name}";
        }

        private string GetParameterBindingAttribute(OperationModel operation, ParameterModel parameter)
        {
            if (parameter.GetParameterSettings().Source().IsDefault())
            {
                if (GetTypeInfo(parameter.TypeReference).IsPrimitive && !parameter.TypeReference.IsCollection)
                {
                    if (GetPath(operation) != null && GetPath(operation).Split('/', StringSplitOptions.RemoveEmptyEntries).Any(x => 
                        x.Contains('{') 
                        && x.Contains('}') 
                        && x.Split( new [] {'{', '}'}, StringSplitOptions.RemoveEmptyEntries).Any(i => i == parameter.Name)))
                    {
                        return $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : "")}) ";
                    }
                    return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : "")}) ";
                }

                if (GetHttpVerb(operation) == HttpVerb.POST || GetHttpVerb(operation) == HttpVerb.PUT)
                {
                    return "@RequestBody ";
                }
                return "";
            }

            if (parameter.GetParameterSettings().Source().IsFromBody())
            {
                return "";
            }
            if (parameter.GetParameterSettings().Source().IsFromHeader())
            {
                return "@RequestHeader ";
            }
            if (parameter.GetParameterSettings().Source().IsFromQuery())
            {
                return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : "")}) ";
            }
            if (parameter.GetParameterSettings().Source().IsFromRoute())
            {
                return $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : "")}) ";
            }

            return "";
        }

        private HttpVerb GetHttpVerb(OperationModel operation)
        {
            var verb = operation.GetHttpSettings().Verb();
            return Enum.TryParse(verb.Value, out HttpVerb verbEnum) ? verbEnum : HttpVerb.POST;
        }

        public enum HttpVerb
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}