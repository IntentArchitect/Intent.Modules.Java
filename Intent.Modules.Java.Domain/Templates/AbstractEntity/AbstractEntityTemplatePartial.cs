using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.AbstractEntity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class AbstractEntityTemplate : JavaTemplateBase<object, AbstractEntityDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Domain.AbstractEntity";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public AbstractEntityTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"AbstractEntity",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private string GetClassAnnotations()
        {
            return string.Join(@"
", GetDecorators().SelectMany(x => x.ClassAnnotations()));
        }

        private string GetFields()
        {
            var fields = GetDecoratorsOutput(x => x.Fields());
            return string.IsNullOrWhiteSpace(fields) ? "" : $@"
    {fields}";
        }

        private string GetMethods()
        {
            var methods = GetDecoratorsOutput(x => x.Methods());
            return string.IsNullOrWhiteSpace(methods) ? "" : $@"
    {methods}";
        }

    }
}