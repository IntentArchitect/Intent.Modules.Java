using System.Collections.Generic;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA.Templates.CompositeId;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Templates
{
    public static class TemplateExtensions
    {
        public static string GetCompositeIdName<T>(this IIntentTemplate<T> template) where T : ClassModel
        {
            return template.GetTypeName(CompositeIdTemplate.TemplateId, template.Model);
        }

        public static string GetCompositeIdName(this IIntentTemplate template, ClassModel model)
        {
            return template.GetTypeName(CompositeIdTemplate.TemplateId, model);
        }

    }
}