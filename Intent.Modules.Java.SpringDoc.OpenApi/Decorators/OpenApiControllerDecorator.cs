using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Intent.Engine;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;
using static Intent.Modules.Java.SpringBoot.Templates.RestController.RestControllerTemplate;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringDoc.OpenApi.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class OpenApiControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Modules.Java.SpringDoc.OpenApi.OpenApiControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public OpenApiControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddDependency(new JavaDependency("org.springdoc", "springdoc-openapi-ui", "1.6.12"));
            _template.AddDependency(new JavaDependency("io.swagger.core.v3", "swagger-annotations", "2.2.4"));
        }

        public override IEnumerable<string> ControllerAnnotations()
        {
            var options = new Dictionary<string, string>
            {
                {"name", $"\"{_template.Model.Name}\""}
            };

            if (!string.IsNullOrWhiteSpace(_template.Model.InternalElement.Comment))
            {
                options.Add("description", $"\"{Escape(_template.Model.InternalElement.Comment)}\"");
            }

            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.tags.Tag")}({string.Join(", ", options.Select(x => $"{x.Key} = {x.Value}"))})";
        }

        public override IEnumerable<string> OperationAnnotations(OperationModel operation)
        {
            var operationOptions = new Dictionary<string, string>
            {
                {"summary", $"\"{operation.Name}\""}
            };

            if (!string.IsNullOrWhiteSpace(operation.InternalElement.Comment))
            {
                operationOptions.Add("description", $"\"{Escape(operation.InternalElement.Comment)}\"");
            }

            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.Operation")}({string.Join(", ", operationOptions.Select(x => $"{x.Key} = {x.Value}"))})";

            var apiResponses = new List<(string Code, string Description)>();
            var verb = GetHttpVerb(operation);
            switch (verb)
            {
                case HttpVerb.Get:
                    apiResponses.Add(("200", $"Returns the specified {_template.GetTypeName(operation.ReturnType)}."));
                    break;
                case HttpVerb.Post:
                    apiResponses.Add(("201", "Successfully created."));
                    break;
                case HttpVerb.Patch:
                case HttpVerb.Put:
                    apiResponses.Add((operation.ReturnType != null ? "200" : "204", "Successfully updated."));
                    break;
                case HttpVerb.Delete:
                    apiResponses.Add(("200", "Successfully deleted."));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (operation.Parameters.Any())
            {
                apiResponses.Add(("400", "One or more validation errors have occurred."));
            }

            if (IsOperationSecured(operation))
            {
                apiResponses.Add(("401", "Unauthorized request"));
                apiResponses.Add(("403", "Forbidden request."));
            }

            if (verb == HttpVerb.Get && operation.ReturnType?.IsCollection == false)
            {
                apiResponses.Add(("404", $"Can't find an {_template.GetTypeName(operation.ReturnType)} with the parameters provided."));
            }

            var values = apiResponses
                .Select(x => @$"
        @{_template.ImportType("io.swagger.v3.oas.annotations.responses.ApiResponse")}(responseCode = ""{x.Code}"", description = ""{Escape(x.Description)}"")");

            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.responses.ApiResponses")}(value = {{{string.Join(',', values)} }})";

        }

        private static HttpVerb GetHttpVerb(OperationModel operation)
        {
            var verb = operation.GetHttpSettings().Verb();

            return Enum.TryParse(verb.Value, ignoreCase: true, out HttpVerb verbEnum) ? verbEnum : HttpVerb.Post;
        }

        private bool IsControllerSecured()
        {
            return _template.Model.HasSecured();
        }

        private bool IsOperationSecured(OperationModel operation)
        {
            return (IsControllerSecured() || operation.HasSecured()) && !operation.HasUnsecured();
        }

        private static string Escape(string @string)
        {
            return @string
                .Replace("\\", "\\\\")
                .Replace("\t", "\\t")
                .Replace("\b", "\\b")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\f", "\\f")
                .Replace("\'", "\\'")
                .Replace("\"", "\\\"")
                ;
        }
    }
}