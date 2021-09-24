using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Templates.Application;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates
{
    public static class TemplateExtensions
    {
        public static string GetApplicationName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(ApplicationTemplate.TemplateId);
        }

        public static string GetRestControllerName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Services.Api.ServiceModel
        {
            return template.GetTypeName(RestControllerTemplate.TemplateId, template.Model);
        }

        public static string GetRestControllerName(this IntentTemplateBase template, Intent.Modelers.Services.Api.ServiceModel model)
        {
            return template.GetTypeName(RestControllerTemplate.TemplateId, model);
        }

    }
}