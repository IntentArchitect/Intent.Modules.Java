using System.Collections.Generic;
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
        public static string GetEntityToDtoMappingName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Services.Api.DTOModel
        {
            return template.GetTypeName(EntityToDtoMappingTemplate.TemplateId, template.Model);
        }

        public static string GetEntityToDtoMappingName(this IntentTemplateBase template, Intent.Modelers.Services.Api.DTOModel model)
        {
            return template.GetTypeName(EntityToDtoMappingTemplate.TemplateId, model);
        }

        public static string GetModelMapperBeanName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(ModelMapperBeanTemplate.TemplateId);
        }

    }
}