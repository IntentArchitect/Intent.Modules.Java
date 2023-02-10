using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringFox.Swagger.Templates.SpringFoxConfig;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringFox.Swagger.Templates
{
    public static class TemplateExtensions
    {
        public static string GetSpringFoxConfigName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(SpringFoxConfigTemplate.TemplateId);
        }

    }
}