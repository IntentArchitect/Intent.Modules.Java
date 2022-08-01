using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.RestController
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class RestControllerTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel, RestControllerDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.RestController";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public RestControllerTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(DataTransferModelTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
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
            annotations.AddRange(GetDecorators().SelectMany(x => x.ControllerAnnotations() ?? new List<string>()));
            return string.Join(@"
", annotations.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private string GetServiceInterfaceName()
        {
            return GetTypeName(ServiceInterfaceTemplate.TemplateId, Model);
        }

        private string GetOperationAnnotations(OperationModel operation)
        {
            var annotations = new List<string>();
            var mappingAnnotationParameters = new List<string>();

            if (operation.ReturnType == null)
            {
                annotations.Add($"@ResponseStatus({GetHttpResponseCode()})");
            }

            if (GetPath(operation) != null)
            {
                mappingAnnotationParameters.Add($"path = \"{GetPath(operation)}\"");
            }

            if (operation.Parameters.Any() &&
                operation.Parameters.All(parameter =>
                    parameter.GetParameterSettings().Source().AsEnum() == ParameterModelStereotypeExtensions
                        .ParameterSettings.SourceOptionsEnum.FromForm))
            {
                mappingAnnotationParameters.Add($"consumes = {ImportType("org.springframework.http.MediaType")}.APPLICATION_FORM_URLENCODED_VALUE");
            }

            annotations.Add(mappingAnnotationParameters.Any()
                ? $@"@{GetHttpVerb(operation).ToString().ToLower().ToPascalCase()}Mapping({string.Join(", ", mappingAnnotationParameters)})"
                : $@"@{GetHttpVerb(operation).ToString().ToLower().ToPascalCase()}Mapping");
            annotations.AddRange(GetDecorators().SelectMany(x => x.OperationAnnotations(operation) ?? new List<string>()));

            return string.Join(@"
    ", annotations);
        }

        private static string GetHttpResponseCode()
        {
            return "HttpStatus.OK";
        }

        private string GetReturnType(OperationModel operation)
        {
            if (operation.ReturnType == null)
            {
                return "void";
            }

            return $"ResponseEntity<{GetTypeName(operation.TypeReference).AsReferenceType()}>";
        }

        private string GetPath(OperationModel operation)
        {
            var path = operation.GetHttpSettings().Route();
            return !string.IsNullOrWhiteSpace(path) ? $"/{path.RemovePrefix("/")}" : null;
        }

        private string GetParameters(OperationModel operation)
        {
            return string.Join(", ", operation.Parameters.Select(param => GetParameter(operation, param)));
        }

        private string GetParameter(OperationModel operation, ParameterModel parameter)
        {
            return $"{GetParameterBindingAttribute(operation, parameter)}{GetTypeName(parameter)} {parameter.Name}";
        }

        private string GetParameterBindingAttribute(OperationModel operation, ParameterModel parameter)
        {
            if (parameter.GetParameterSettings().Source().IsDefault())
            {
                if ((GetTypeInfo(parameter.TypeReference).IsPrimitive || GetTypeInfo(parameter.TypeReference).Name == "String") && !parameter.TypeReference.IsCollection)
                {
                    if (GetPath(operation) != null && GetPath(operation).Split('/', StringSplitOptions.RemoveEmptyEntries).Any(x =>
                        x.Contains('{')
                        && x.Contains('}')
                        && x.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Any(i => i == parameter.Name)))
                    {
                        return $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)}) ";
                    }
                    return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)}) ";
                }

                if (GetHttpVerb(operation) == HttpVerb.PATCH ||
                    GetHttpVerb(operation) == HttpVerb.POST ||
                    GetHttpVerb(operation) == HttpVerb.PUT)
                {
                    return "@RequestBody ";
                }

                return string.Empty;
            }

            if (parameter.GetParameterSettings().Source().IsFromBody() ||
                parameter.GetParameterSettings().Source().IsFromForm())
            {
                return string.Empty;
            }

            if (parameter.GetParameterSettings().Source().IsFromHeader())
            {
                return "@RequestHeader ";
            }

            if (parameter.GetParameterSettings().Source().IsFromQuery())
            {
                return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)}) ";
            }

            if (parameter.GetParameterSettings().Source().IsFromRoute())
            {
                return $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)}) ";
            }

            return string.Empty;
        }

        private static HttpVerb GetHttpVerb(OperationModel operation)
        {
            var verb = operation.GetHttpSettings().Verb();
            return Enum.TryParse(verb.Value, out HttpVerb verbEnum) ? verbEnum : HttpVerb.POST;
        }

        public enum HttpVerb
        {
            DELETE,
            GET,
            PATCH,
            POST,
            PUT
        }
    }
}