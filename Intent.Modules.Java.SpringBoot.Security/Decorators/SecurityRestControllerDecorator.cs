using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.WebApi.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class SecurityRestControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.SpringBoot.Security.SecurityRestControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public SecurityRestControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override IEnumerable<string> ControllerAnnotations()
        {
            var roles = _template.Model.GetSecured()?.Roles() ?? string.Empty;
            var split = roles.Split(',')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => $"hasRole('{x.Trim()}')")
                .ToArray();

            if (split.Length > 0)
            {
                yield return $"@{_template.ImportType("org.springframework.security.access.prepost.PreAuthorize")}(\"{string.Join(" or ", split)}\")";
            }

            foreach (var annotation in base.ControllerAnnotations())
            {
                yield return annotation;
            }
        }

        public override IEnumerable<string> OperationAnnotations(OperationModel operation)
        {
            var roles = operation.GetSecured()?.Roles() ?? string.Empty;
            var split = roles.Split(',')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => $"hasRole('{x.Trim()}')")
                .ToArray();

            if (split.Length > 0)
            {
                yield return $"@{_template.ImportType("org.springframework.security.access.prepost.PreAuthorize")}(\"{string.Join(" or ", split)}\")";
            }

            foreach (var annotation in base.OperationAnnotations(operation))
            {
                yield return annotation;
            }
        }
    }
}