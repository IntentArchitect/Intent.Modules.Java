using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ExceptionType;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Services.Templates.ServiceInterface;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates
{
    public static class TemplateExtensions
    {
        public static string GetDataTransferModelName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Services.Api.DTOModel
        {
            return template.GetTypeName(DataTransferModelTemplate.TemplateId, template.Model);
        }

        public static string GetDataTransferModelName(this IntentTemplateBase template, Intent.Modelers.Services.Api.DTOModel model)
        {
            return template.GetTypeName(DataTransferModelTemplate.TemplateId, model);
        }

        public static string GetExceptionTypeName<T>(this IntentTemplateBase<T> template) where T : Intent.Modules.Java.Services.Api.ExceptionTypeModel
        {
            return template.GetTypeName(ExceptionTypeTemplate.TemplateId, template.Model);
        }

        public static string GetExceptionTypeName(this IntentTemplateBase template, Intent.Modules.Java.Services.Api.ExceptionTypeModel model)
        {
            return template.GetTypeName(ExceptionTypeTemplate.TemplateId, model);
        }

        public static string GetServiceImplementationName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Services.Api.ServiceModel
        {
            return template.GetTypeName(ServiceImplementationTemplate.TemplateId, template.Model);
        }

        public static string GetServiceImplementationName(this IntentTemplateBase template, Intent.Modelers.Services.Api.ServiceModel model)
        {
            return template.GetTypeName(ServiceImplementationTemplate.TemplateId, model);
        }

        public static string GetServiceInterfaceName<T>(this IntentTemplateBase<T> template) where T : Intent.Modelers.Services.Api.ServiceModel
        {
            return template.GetTypeName(ServiceInterfaceTemplate.TemplateId, template.Model);
        }

        public static string GetServiceInterfaceName(this IntentTemplateBase template, Intent.Modelers.Services.Api.ServiceModel model)
        {
            return template.GetTypeName(ServiceInterfaceTemplate.TemplateId, model);
        }

    }
}