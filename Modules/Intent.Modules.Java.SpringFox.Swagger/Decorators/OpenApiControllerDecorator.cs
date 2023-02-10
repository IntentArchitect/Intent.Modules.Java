using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.SpringFox.Swagger.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringFox.Swagger.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class OpenApiControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.SpringFox.Swagger.OpenApiControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;

        [IntentManaged(Mode.Merge, Body = Mode.Fully)]
        public OpenApiControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override IEnumerable<string> ControllerAnnotations()
        {
            var options = new Dictionary<string, string>()
            {
                {"value", $"\"{_template.Model.Name}\""}
            };

            if (!string.IsNullOrWhiteSpace(_template.Model.InternalElement.Comment))
            {
                options.Add("description", $"\"{_template.Model.InternalElement.Comment}\"");
            }

            yield return $"@{_template.ImportType("io.swagger.annotations.Api")}({string.Join(", ", options.Select(x => $"{x.Key} = {x.Value}"))})";

            var securityRequirements = _template.Model.GetOpenAPISettings()?.SecurityRequirement();
            if (!string.IsNullOrWhiteSpace(securityRequirements))
            {
                foreach (var securityRequirement in securityRequirements.Split(',').Where(requirement => !string.IsNullOrWhiteSpace(requirement)))
                {
                    yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.security.SecurityRequirement")}(name = \"{securityRequirement.Trim()}\")";
                }
            }
        }

        public override IEnumerable<string> OperationAnnotations(OperationModel operation)
        {
            var options = new Dictionary<string, string>()
            {
                {"value", $"\"{operation.Name}\""}
            };
            if (!string.IsNullOrWhiteSpace(operation.InternalElement.Comment))
            {
                options.Add("notes", $"\"{operation.InternalElement.Comment}\"");
            }
            yield return $"@{_template.ImportType("io.swagger.annotations.ApiOperation")}({string.Join(", ", options.Select(x => $"{x.Key} = {x.Value}"))})";
        }
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;
    }
}