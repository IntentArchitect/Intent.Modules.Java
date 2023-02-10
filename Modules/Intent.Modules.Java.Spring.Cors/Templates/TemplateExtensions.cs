using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Spring.Cors.Templates.CorsConfig;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Cors.Templates
{
    public static class TemplateExtensions
    {
        public static string GetCorsConfigName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(CorsConfigTemplate.TemplateId);
        }

    }
}