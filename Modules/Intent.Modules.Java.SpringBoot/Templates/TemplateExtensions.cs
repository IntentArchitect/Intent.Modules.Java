using System.Collections.Generic;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Templates.Application;
using Intent.Modules.Java.SpringBoot.Templates.JsonResponse;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates
{
    public static class TemplateExtensions
    {
        public static string GetApplicationName(this IIntentTemplate template)
        {
            return template.GetTypeName(ApplicationTemplate.TemplateId);
        }

        public static string GetJsonResponseName(this IIntentTemplate template)
        {
            return template.GetTypeName(JsonResponseTemplate.TemplateId);
        }

        public static string GetRestControllerName<T>(this IIntentTemplate<T> template) where T : ServiceModel
        {
            return template.GetTypeName(RestControllerTemplate.TemplateId, template.Model);
        }

        public static string GetRestControllerName(this IIntentTemplate template, ServiceModel model)
        {
            return template.GetTypeName(RestControllerTemplate.TemplateId, model);
        }

    }
}