using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.Enum;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.DomainModel
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class DomainModelTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel, DomainModelDecorator>
    {
        [IntentManaged(Mode.Fully)] public const string TemplateId = "Intent.Java.Domain.DomainModel";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public DomainModelTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
            SetDefaultTypeCollectionFormat("java.util.List<{0}>");
            AddTypeSource(TemplateId).WithCollectionFormat("java.util.List<{0}>");
            AddDependency(JavaDependencies.Lombok);
            AddTypeSource(EnumTemplate.TemplateId);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name.ToPascalCase()}",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        public override void BeforeTemplateExecution()
        {
            foreach (var decorator in GetDecorators())
            {
                decorator.BeforeTemplateExecution();
            }
        }

        private IEnumerable<string> GetClassAnnotations()
        {
            foreach (var annotation in GetDecorators().SelectMany(x => x.ClassAnnotations()))
            {
                yield return annotation;
            }

            if (Model.IsAbstract)
            {
                yield return $"@{ImportType("lombok.Getter")}";
                yield return $"@{ImportType("lombok.Setter")}";
            }
            else
            {
                yield return $"@{ImportType("lombok.Data")}";
            }

            if (Model.Attributes.Any() || GetDecorators().SelectMany(x => x.Fields()).Any())
            {
                yield return $"@{ImportType("lombok.AllArgsConstructor")}";
            }

            yield return "@NoArgsConstructor";
            yield return $"{this.IntentManageClassAnnotation()}(privateMethods = {this.IntentModeIgnore()})";
        }

        public string GetBaseClass() => Model.ParentClass != null
            ? $" extends {GetTypeName(Model.Generalizations()[0].TypeReference)}"
            : $" implements {ImportType("java.io.Serializable")}";

        private string GetAbstractDefinition() => Model.IsAbstract
            ? " abstract"
            : string.Empty;

        private string GetGenericTypeParameters()
        {
            if (!Model.GenericTypes.Any())
            {
                return string.Empty;
            }

            return $"<{string.Join(',', Model.GenericTypes)}>";
        }
    }
}