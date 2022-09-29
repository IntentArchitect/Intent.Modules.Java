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
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ExceptionType;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.Modules.Java.SpringBoot.Api;
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
        [IntentManaged(Mode.Fully)] public const string TemplateId = "Intent.Java.SpringBoot.RestController";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public RestControllerTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(DataTransferModelTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
            AddDependency(JavaDependencies.Lombok);
            AddTypeSource(ExceptionTypeTemplate.TemplateId);
        }

        public string RootName => Model.Name.RemoveSuffix("Service", "Controller", "Resource");

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{RootName}Controller",
                package: this.GetPackage(),
                relativeLocation: this.GetPackageFolderPath()
            );
        }

        private string GetControllerAnnotations()
        {
            var annotations = new List<string>();

            if (Model.Operations
                .Any(operation => new OperationExtensionModel(operation.InternalElement).CheckedExceptions
                    .Select(GetCheckedExceptionHandling)
                    .Any(e => e.Log)))
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

            if (operation.GetHttpSettings().ReturnTypeMediatype().IsApplicationJson()
                && (GetTypeInfo(operation.ReturnType).IsPrimitive || operation.ReturnType.HasStringType() || operation.ReturnType.Element.Name == "guid"))
            {
                var wrappedReturnType = $"{this.GetJsonResponseName()}<{GetTypeName(operation.TypeReference).AsReferenceType()}>";
                return $"ResponseEntity<{wrappedReturnType}>";
            }

            var returnType = operation.TypeReference.Element.Name != "object"
                ? GetTypeName(operation.TypeReference).AsReferenceType()
                : operation.TypeReference.IsCollection
                    ? $"{ImportType("java.util.List")}<?>"
                    : "?";

            return $"ResponseEntity<{returnType}>";
        }

        private string GetResultValue(OperationModel operation)
        {
            if (operation.GetHttpSettings().ReturnTypeMediatype().IsApplicationJson()
                && (GetTypeInfo(operation.ReturnType).IsPrimitive || operation.ReturnType.HasStringType() || operation.ReturnType.Element.Name == "guid"))
            {
                var wrappedReturnType = $"{this.GetJsonResponseName()}<{GetTypeName(operation.TypeReference).AsReferenceType()}>";
                return $"new {wrappedReturnType}(result)";
            }

            return "result";
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
                     GetTypeInfo(parameter.TypeReference).Name == "String" ||
                     GetTypeInfo(parameter.TypeReference).Name == "UUID") &&
                    !parameter.TypeReference.IsCollection)
                {
                    if (GetPath(operation) != null && GetPath(operation).Split('/', StringSplitOptions.RemoveEmptyEntries).Any(x =>
                            x.Contains('{')
                            && x.Contains('}')
                            && x.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Any(i => i == parameter.Name)))
                    {
                        return
                            $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
                    }

                    return
                        $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
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
                return
                    $"@RequestParam(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
            }

            if (parameter.GetParameterSettings().Source().IsFromRoute())
            {
                return
                    $"@PathVariable(value = \"{parameter.Name}\"{(parameter.Type.IsNullable ? $", required = {parameter.TypeReference.IsNullable.ToString().ToLower()}" : string.Empty)})";
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

        private (string TypeName, string HttpResponse, bool Log) GetCheckedExceptionHandling(CheckedExceptionModel checkedException)
        {
            var typeName = GetTypeName(checkedException);

            switch (checkedException.TypeReference.Element.SpecializationTypeId)
            {
                case TypeDefinitionModel.SpecializationTypeId:
                    {
                        var stereotype = checkedException.TypeReference.Element.AsTypeDefinitionModel()?.GetCheckedExceptionHandling();
                        if (stereotype != null)
                        {
                            return (typeName, stereotype.HttpResponseStatus().Value, stereotype.Log());
                        }

                        break;
                    }
                case ExceptionTypeModel.SpecializationTypeId:
                    {
                        var stereotype = checkedException.TypeReference.Element.AsExceptionTypeModel()?.GetCheckedExceptionHandling();
                        if (stereotype != null)
                        {
                            return (typeName, stereotype.HttpResponseStatus().Value, stereotype.Log());
                        }

                        break;
                    }
            }

            return (typeName, "INTERNAL_SERVER_ERROR (500)", true);
        }

        private IEnumerable<CheckedException> GetCheckedExceptions(OperationModel operation)
        {
            var checkedExceptions = new OperationExtensionModel(operation.InternalElement).CheckedExceptions
                .Select(GetCheckedExceptionHandling)
                .ToArray();

            return checkedExceptions
                .GroupBy(
                    checkedExceptionHandling => new
                    {
                        checkedExceptionHandling.HttpResponse,
                        checkedExceptionHandling.Log
                    },
                    (key, groupItems) =>
                    {
                        return new CheckedException(
                            types: groupItems.Select(z => z.TypeName).ToArray(),
                            httpStatus: key.HttpResponse.Split(' ')[0],
                            log: key.Log);
                    })
                .OrderBy(x => x.IsBaseType);
        }

        private class CheckedException
        {
            public CheckedException(IReadOnlyCollection<string> types, string httpStatus, bool log)
            {
                (Types, IsBaseType) = types.Any(x => x == "Exception")
                    ? ("Exception", true)
                    : (string.Join(" | ", types), false);
                HttpStatus = httpStatus;
                Log = log;
            }

            public string Types { get; }
            public string HttpStatus { get; }
            public bool Log { get; }
            public bool IsBaseType { get; }
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