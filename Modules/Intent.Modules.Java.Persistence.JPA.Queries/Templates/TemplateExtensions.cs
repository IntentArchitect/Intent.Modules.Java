using System.Collections.Generic;
using Intent.Java.Persistence.JPA.Queries.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryProjection;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Templates
{
    public static class TemplateExtensions
    {
        public static string GetQueryProjectionName<T>(this IIntentTemplate<T> template) where T : QueryProjectionModel
        {
            return template.GetTypeName(QueryProjectionTemplate.TemplateId, template.Model);
        }

        public static string GetQueryProjectionName(this IIntentTemplate template, QueryProjectionModel model)
        {
            return template.GetTypeName(QueryProjectionTemplate.TemplateId, model);
        }

    }
}