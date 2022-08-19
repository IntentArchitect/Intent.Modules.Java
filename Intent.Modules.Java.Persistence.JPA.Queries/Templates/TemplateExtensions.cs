using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryResult;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Templates
{
    public static class TemplateExtensions
    {
        public static string GetQueryResultName<T>(this IntentTemplateBase<T> template) where T : Intent.Java.Persistence.JPA.Queries.Api.QueryResultModel
        {
            return template.GetTypeName(QueryResultTemplate.TemplateId, template.Model);
        }

        public static string GetQueryResultName(this IntentTemplateBase template, Intent.Java.Persistence.JPA.Queries.Api.QueryResultModel model)
        {
            return template.GetTypeName(QueryResultTemplate.TemplateId, model);
        }

    }
}