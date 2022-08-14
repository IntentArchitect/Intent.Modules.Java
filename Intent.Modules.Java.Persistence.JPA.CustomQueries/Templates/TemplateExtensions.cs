using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates.QueryView;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates
{
    public static class TemplateExtensions
    {
        public static string GetQueryViewName<T>(this IntentTemplateBase<T> template) where T : Intent.Java.Persistence.JPA.CustomQueries.Api.CustomQueryModel
        {
            return template.GetTypeName(QueryViewTemplate.TemplateId, template.Model);
        }

        public static string GetQueryViewName(this IntentTemplateBase template, Intent.Java.Persistence.JPA.CustomQueries.Api.CustomQueryModel model)
        {
            return template.GetTypeName(QueryViewTemplate.TemplateId, model);
        }

    }
}