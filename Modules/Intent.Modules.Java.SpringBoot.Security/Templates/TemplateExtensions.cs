using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Security.Templates.AuthTokenFilter;
using Intent.Modules.Java.SpringBoot.Security.Templates.JwtUtils;
using Intent.Modules.Java.SpringBoot.Security.Templates.MethodSecurityConfig;
using Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfigV2;
using Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfigV3;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAuthTokenFilterName(this IIntentTemplate template)
        {
            return template.GetTypeName(AuthTokenFilterTemplate.TemplateId);
        }

        public static string GetJwtUtilsName(this IIntentTemplate template)
        {
            return template.GetTypeName(JwtUtilsTemplate.TemplateId);
        }

        public static string GetMethodSecurityConfigName(this IIntentTemplate template)
        {
            return template.GetTypeName(MethodSecurityConfigTemplate.TemplateId);
        }

        public static string GetWebSecurityConfigV2Name(this IIntentTemplate template)
        {
            return template.GetTypeName(WebSecurityConfigV2Template.TemplateId);
        }

        public static string GetWebSecurityConfigV3Name(this IIntentTemplate template)
        {
            return template.GetTypeName(WebSecurityConfigV3Template.TemplateId);
        }

    }
}