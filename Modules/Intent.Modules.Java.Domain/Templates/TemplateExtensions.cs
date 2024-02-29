using System.Collections.Generic;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Domain.Templates.Enum;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates
{
    public static class TemplateExtensions
    {
        public static string GetDomainModelName<T>(this IIntentTemplate<T> template) where T : ClassModel
        {
            return template.GetTypeName(DomainModelTemplate.TemplateId, template.Model);
        }

        public static string GetDomainModelName(this IIntentTemplate template, ClassModel model)
        {
            return template.GetTypeName(DomainModelTemplate.TemplateId, model);
        }

        public static string GetEnumName<T>(this IIntentTemplate<T> template) where T : EnumModel
        {
            return template.GetTypeName(EnumTemplate.TemplateId, template.Model);
        }

        public static string GetEnumName(this IIntentTemplate template, EnumModel model)
        {
            return template.GetTypeName(EnumTemplate.TemplateId, model);
        }

    }
}