using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Validation.Templates.RestResponseEntityExceptionHandler;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Validation.Templates
{
    public static class TemplateExtensions
    {
        public static string GetRestResponseEntityExceptionHandlerName(this IIntentTemplate template)
        {
            return template.GetTypeName(RestResponseEntityExceptionHandlerTemplate.TemplateId);
        }

    }
}