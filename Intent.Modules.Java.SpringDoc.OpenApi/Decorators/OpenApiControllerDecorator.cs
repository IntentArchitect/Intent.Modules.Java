using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.Modules.Java.SpringDoc.OpenApi.Api;
using Intent.RoslynWeaver.Attributes;

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
            var options = new Dictionary<string, string>()
            {
                {"name", $"\"{_template.Model.Name}\""}
            };

            if (!string.IsNullOrWhiteSpace(_template.Model.InternalElement.Comment))
            {
                options.Add("description", $"\"{_template.Model.InternalElement.Comment}\"");
            }

            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.tags.Tag")}({string.Join(", ", options.Select(x => $"{x.Key} = {x.Value}"))})";

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
                {"summary", $"\"{operation.Name}\""}
            };
            if (!string.IsNullOrWhiteSpace(operation.InternalElement.Comment))
            {
                options.Add("description", $"\"{operation.InternalElement.Comment}\"");
            }
            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.Operation")}({string.Join(", ", options.Select(x => $"{x.Key} = {x.Value}"))})";
        }
    }
}