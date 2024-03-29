using System.Collections.Generic;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanAdd;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanRemove;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanUpdate;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentIgnore;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentIgnoreBody;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentManage;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentManageClass;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentMerge;
using Intent.Modules.Java.Weaving.Annotations.Templates.ModeEnum;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Weaving.Annotations.Templates
{
    public static class TemplateExtensions
    {
        public static string GetIntentCanAddName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentCanAddTemplate.TemplateId);
        }

        public static string GetIntentCanRemoveName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentCanRemoveTemplate.TemplateId);
        }

        public static string GetIntentCanUpdateName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentCanUpdateTemplate.TemplateId);
        }

        public static string GetIntentIgnoreName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentIgnoreTemplate.TemplateId);
        }

        public static string GetIntentIgnoreBodyName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentIgnoreBodyTemplate.TemplateId);
        }

        public static string GetIntentManageName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentManageTemplate.TemplateId);
        }

        public static string GetIntentManageClassName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentManageClassTemplate.TemplateId);
        }

        public static string GetIntentMergeName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(IntentMergeTemplate.TemplateId);
        }

        public static string GetModeEnumName<T>(this IntentTemplateBase<T> template)
        {
            return template.GetTypeName(ModeEnumTemplate.TemplateId);
        }

    }
}