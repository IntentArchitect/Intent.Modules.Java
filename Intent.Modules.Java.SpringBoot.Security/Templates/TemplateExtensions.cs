using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Security.Templates.AuthTokenFilter;
using Intent.Modules.Java.SpringBoot.Security.Templates.JwtUtils;
using Intent.Modules.Java.SpringBoot.Security.Templates.MethodSecurityConfig;
using Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfig;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAuthTokenFilterName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(AuthTokenFilterTemplate.TemplateId);
        }

        public static string GetJwtUtilsName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(JwtUtilsTemplate.TemplateId);
        }

        public static string GetMethodSecurityConfigName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(MethodSecurityConfigTemplate.TemplateId);
        }

        public static string GetWebSecurityConfigName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(WebSecurityConfigTemplate.TemplateId);
        }

    }
}