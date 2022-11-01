using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Java.TypeResolvers;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.AbstractEntity;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.DomainModel
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class DomainModelTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel, DomainModelDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Domain.DomainModel";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public DomainModelTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
            SetDefaultTypeCollectionFormat("java.util.List<{0}>");
            AddTypeSource(TemplateId).WithCollectionFormat("java.util.List<{0}>");
            AddDependency(JavaDependencies.Lombok);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private string GetClassAnnotations()
        {
            var annotations = new List<string>();

            annotations.AddRange(GetDecoratorsOutput(x => x.ClassAnnotations()).Trim()
                .Replace("\r\n", "\n")
                .Split("\n")
                .Select(x => x.Trim()));

            annotations.Add("@Data");
            annotations.Add("@AllArgsConstructor");
            annotations.Add("@NoArgsConstructor");
            annotations.Add($"{this.IntentManageClassAnnotation()}(privateMethods = {this.IntentModeIgnore()})");

            return string.Join(Environment.NewLine, annotations);
        }

        public string GetBaseClass()
        {
            return GetTypeName(AbstractEntityTemplate.TemplateId);
        }
    }
}