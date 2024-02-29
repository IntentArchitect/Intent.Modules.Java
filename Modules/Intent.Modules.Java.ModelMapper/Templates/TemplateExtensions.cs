using System.Collections.Generic;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.ModelMapper.Templates.EntityToDtoMapping;
using Intent.Modules.Java.ModelMapper.Templates.ModelMapperBean;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.ModelMapper.Templates
{
    public static class TemplateExtensions
    {
        public static string GetEntityToDtoMappingName<T>(this IIntentTemplate<T> template) where T : DTOModel
        {
            return template.GetTypeName(EntityToDtoMappingTemplate.TemplateId, template.Model);
        }

        public static string GetEntityToDtoMappingName(this IIntentTemplate template, DTOModel model)
        {
            return template.GetTypeName(EntityToDtoMappingTemplate.TemplateId, model);
        }

        public static string GetModelMapperBeanName(this IIntentTemplate template)
        {
            return template.GetTypeName(ModelMapperBeanTemplate.TemplateId);
        }

    }
}