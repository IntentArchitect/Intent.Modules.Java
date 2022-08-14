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
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Java.Services;
using Intent.Modules.Java.Services.Api;
using Intent.Modules.Java.Services.Templates;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using OperationExtensionModel = Intent.Modules.Java.Services.Api.OperationExtensionModel;

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
            AddDependency(JavaDependencies.Lombok);
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

            if (Model.Operations
                .Any(operation => new OperationExtensionModel(operation.InternalElement).CheckedExceptions
                    .Any(checkException => checkException.TypeReference.Element.AsTypeDefinitionModel()?.GetCheckedExceptionHandling()?.Log() == true)))
            {
                annotations.Add($"@{ImportType("lombok.extern.slf4j.Slf4j")}");
            }

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

            var returnType = operation.TypeReference.Element.Name != "object"
                ? GetTypeName(operation.TypeReference).AsReferenceType()
                : operation.TypeReference.IsCollection
                    ? $"{ImportType("java.util.List")}<?>"
                    : "?";

            return $"ResponseEntity<{returnType}>";
        }

        private static string GetPath(OperationModel operation)
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
            var annotations = GetDecorators()
                .SelectMany(decorator => decorator.ParameterAnnotations(parameter))
                .Append(GetParameterBindingDecorator(operation, parameter))
                .Select(annotation => $"{annotation} ");

            return $"{string.Concat(annotations)}{GetTypeName(parameter)} {parameter.Name}";
        }

        private string GetParameterBindingDecorator(OperationModel operation, ParameterModel parameter)
        {
            if (parameter.GetParameterSettings().Source().IsDefault())
            {
                if ((GetTypeInfo(parameter.TypeReference).IsPrimitive ||
                     GetTypeInfo(parameter.TypeReference).Name == "String") &&
                    !parameter.TypeReference.IsCollection)
                {
                    if (GetPath(operation) != null && GetPath(operation).Split('/', StringSplitOptions.RemoveEmptyEntries).Any(x =>
                        x.Contains('{')
                        && x.Contains('}')
                        && x.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Any(i => i == parameter.Name)))
                    {
                        return $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
                    }
                    return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
                }

                if (GetHttpVerb(operation) == HttpVerb.PATCH ||
                    GetHttpVerb(operation) == HttpVerb.POST ||
                    GetHttpVerb(operation) == HttpVerb.PUT)
                {
                    return "@RequestBody";
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
                var headerName = parameter.GetParameterSettings().HeaderName();
                return $"@RequestHeader{(!string.IsNullOrWhiteSpace(headerName) ? $"(\"{headerName}\")" : string.Empty)}";
            }

            if (parameter.GetParameterSettings().Source().IsFromQuery())
            {
                return $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
            }

            if (parameter.GetParameterSettings().Source().IsFromRoute())
            {
                return $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
            }

            return string.Empty;
        }

        private static HttpVerb GetHttpVerb(OperationModel operation)
        {
            var verb = operation.GetHttpSettings().Verb();
            return Enum.TryParse(verb.Value, out HttpVerb verbEnum) ? verbEnum : HttpVerb.POST;
        }

        private static bool HasCheckedExceptions(OperationModel operation)
        {
            return new OperationExtensionModel(operation.InternalElement).CheckedExceptions.Any();
        }

        private IEnumerable<CheckedException> GetCheckedExceptions(OperationModel operation)
        {
            return new OperationExtensionModel(operation.InternalElement).CheckedExceptions
                .Select(checkedException => new
                {
                    TypeName = GetTypeName(checkedException),
                    Handling = checkedException.TypeReference.Element.AsTypeDefinitionModel().GetCheckedExceptionHandling()
                })
                .Where(checkedException => checkedException.Handling != null)
                .GroupBy(
                    checkedException => new
                    {
                        HttpResponse = checkedException.Handling.HttpResponseStatus().Value,
                        Log = checkedException.Handling.Log()
                    },
                    (key, groupItems) => new CheckedException(
                        Types: string.Join(" | ", groupItems.Select(z => z.TypeName)),
                        HttpStatus: key.HttpResponse.Split(' ')[0],
                        Log: key.Log));
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