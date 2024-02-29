using System.Collections.Generic;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.Modules.Java.Services.Api;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.Enum;
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
        public static string GetDataTransferModelName<T>(this IIntentTemplate<T> template) where T : DTOModel
        {
            return template.GetTypeName(DataTransferModelTemplate.TemplateId, template.Model);
        }

        public static string GetDataTransferModelName(this IIntentTemplate template, DTOModel model)
        {
            return template.GetTypeName(DataTransferModelTemplate.TemplateId, model);
        }

        public static string GetEnumName<T>(this IIntentTemplate<T> template) where T : EnumModel
        {
            return template.GetTypeName(EnumTemplate.TemplateId, template.Model);
        }

        public static string GetEnumName(this IIntentTemplate template, EnumModel model)
        {
            return template.GetTypeName(EnumTemplate.TemplateId, model);
        }

        public static string GetExceptionTypeName<T>(this IIntentTemplate<T> template) where T : ExceptionTypeModel
        {
            return template.GetTypeName(ExceptionTypeTemplate.TemplateId, template.Model);
        }

        public static string GetExceptionTypeName(this IIntentTemplate template, ExceptionTypeModel model)
        {
            return template.GetTypeName(ExceptionTypeTemplate.TemplateId, model);
        }

        public static string GetServiceImplementationName<T>(this IIntentTemplate<T> template) where T : ServiceModel
        {
            return template.GetTypeName(ServiceImplementationTemplate.TemplateId, template.Model);
        }

        public static string GetServiceImplementationName(this IIntentTemplate template, ServiceModel model)
        {
            return template.GetTypeName(ServiceImplementationTemplate.TemplateId, model);
        }

        public static string GetServiceInterfaceName<T>(this IIntentTemplate<T> template) where T : ServiceModel
        {
            return template.GetTypeName(ServiceInterfaceTemplate.TemplateId, template.Model);
        }

        public static string GetServiceInterfaceName(this IIntentTemplate template, ServiceModel model)
        {
            return template.GetTypeName(ServiceInterfaceTemplate.TemplateId, model);
        }

    }
}