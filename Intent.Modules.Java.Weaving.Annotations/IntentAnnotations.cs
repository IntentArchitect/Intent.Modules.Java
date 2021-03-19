using System;
using System.Collections.Generic;
using System.Text;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanAdd;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanRemove;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanUpdate;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentIgnore;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentIgnoreBody;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentManage;
using Intent.Modules.Java.Weaving.Annotations.Templates.IntentMerge;
using Intent.Modules.Java.Weaving.Annotations.Templates.ModeEnum;

namespace Intent.Modules.Java
{
    public static class IntentAnnotations
    {
        public static string IntentIgnoreAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentIgnoreTemplate.TemplateId);
        }

        public static string IntentIgnoreBodyAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentIgnoreBodyTemplate.TemplateId);
        }

        public static string IntentMergeAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentMergeTemplate.TemplateId);
        }

        public static string IntentManageAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentManageTemplate.TemplateId);
        }

        public static string IntentCanAddAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentCanAddTemplate.TemplateId);
        }

        public static string IntentCanUpdateAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentCanUpdateTemplate.TemplateId);
        }

        public static string IntentCanRemoveAnnotation<T>(this JavaTemplateBase<T> template)
        {
            return "@" + template.GetTypeName(IntentCanRemoveTemplate.TemplateId);
        }

        public static string IntentModeIgnore<T>(this JavaTemplateBase<T> template)
        {
            return $"{template.GetTypeName(ModeEnumTemplate.TemplateId)}.Ignore";
        }
    }
}
