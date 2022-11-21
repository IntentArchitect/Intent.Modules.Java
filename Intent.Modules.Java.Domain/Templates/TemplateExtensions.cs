using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.AbstractEntity;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Domain.Templates.Enum;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates
{
    public static class TemplateExtensions
    {
        public static string GetAbstractEntityName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(AbstractEntityTemplate.TemplateId);
        }

        public static string GetDomainModelName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Domain.Api.ClassModel
        {
            return template.GetTypeName(DomainModelTemplate.TemplateId, template.Model);
        }

        public static string GetDomainModelName(this IntentTemplateBase template, Intent.Modelers.Domain.Api.ClassModel model)
        {
            return template.GetTypeName(DomainModelTemplate.TemplateId, model);
        }

        public static string GetEnumName<T>(this IntentTemplateBase<T> template) where T : Intent.Modules.Common.Types.Api.EnumModel
        {
            return template.GetTypeName(EnumTemplate.TemplateId, template.Model);
        }

        public static string GetEnumName(this IntentTemplateBase template, Intent.Modules.Common.Types.Api.EnumModel model)
        {
            return template.GetTypeName(EnumTemplate.TemplateId, model);
        }

    }
}