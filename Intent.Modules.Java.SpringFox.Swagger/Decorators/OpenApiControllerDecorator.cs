using System.Collections.Generic;
using System.Linq;
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

        private readonly RestControllerTemplate _template;

        public OpenApiControllerDecorator(RestControllerTemplate template)
        {
            _template = template;
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
    }
}