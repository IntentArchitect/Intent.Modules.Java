using System.Collections.Generic;
using Intent.Engine;
using Intent.Java.Spring.Cors.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Cors.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class CorsRestControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Spring.Cors.CorsRestControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public CorsRestControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override IEnumerable<string> ControllerAnnotations()
        {
            if (_template.Model.GetCORSSettings()?.AllowCrossOriginRequests() == true)
            {
                yield return $"@{_template.ImportType("org.springframework.web.bind.annotation.CrossOrigin")}";
            }
        }

        public override IEnumerable<string> OperationAnnotations(OperationModel operation)
        {
            if (operation.GetCORSSettings()?.AllowCrossOriginRequests() == true)
            {
                yield return $"@{_template.ImportType("org.springframework.web.bind.annotation.CrossOrigin")}";
            }
        }
    }
}