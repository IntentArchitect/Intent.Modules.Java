using System.Collections.Generic;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Data.Repositories.Templates
{
    public static class TemplateExtensions
    {
        public static string GetEntityRepositoryName<T>(this IIntentTemplate<T> template) where T : ClassModel
        {
            return template.GetTypeName(EntityRepositoryTemplate.TemplateId, template.Model);
        }

        public static string GetEntityRepositoryName(this IIntentTemplate template, ClassModel model)
        {
            return template.GetTypeName(EntityRepositoryTemplate.TemplateId, model);
        }

    }
}