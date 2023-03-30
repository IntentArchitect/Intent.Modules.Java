using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
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

            base.BeforeTemplateExecution();
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

        private string GetDefaultValueSpecification(AttributeModel attributeModel)
        {
            if (string.IsNullOrWhiteSpace(attributeModel.Value))
            {
                return string.Empty;
            }

            if (attributeModel.TypeReference.HasStringType() || attributeModel.TypeReference.HasJavaStringType())
            {
                var stringValue = attributeModel.Value;
                if (stringValue.StartsWith("\"") || stringValue.StartsWith("\'"))
                {
                    stringValue = stringValue.Substring(1, stringValue.Length - 2);
                }
                return $@" = ""{stringValue}""";
            }

            if (attributeModel.TypeReference.Element.IsEnumModel())
            {
                var enumModel = attributeModel.TypeReference.Element.AsEnumModel();
                var foundLiteral = enumModel.Literals.FirstOrDefault(p =>
                    string.Equals(p.Value, attributeModel.Value, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(p.Name, attributeModel.Value, StringComparison.OrdinalIgnoreCase));
                if (foundLiteral != null)
                {
                    return $@" = {GetTypeName(enumModel.InternalElement)}.{foundLiteral.Name.ToSnakeCase().ToUpperInvariant()}";
                }
            }

            return $" = {attributeModel.Value}";
        }

        private string GetAbstractDefinition() => Model.IsAbstract
            ? " abstract"
            : string.Empty;

        public string GetBaseType() => Model.ParentClassTypeReference != null
            ? $" extends {GetTypeName(Model.ParentClassTypeReference)}"
            : $" implements {ImportType("java.io.Serializable")}";

        private string GetGenericTypeParameters()
        {
            if (!Model.GenericTypes.Any())
            {
                return string.Empty;
            }

            return $"<{string.Join(',', Model.GenericTypes)}>";
        }

        private IEnumerable<string> GetAnnotations(AttributeModel attribute)
        {
            if (!attribute.TypeReference.IsNullable &&
                !attribute.HasStereotype("Primary Key") &&
                !attribute.HasStereotype("Foreign Key"))
            {
                yield return $"@{ImportType("javax.validation.constraints.NotNull")}";
            }

            if (attribute.HasStereotype("Text Constraints") && attribute.GetStereotypeProperty<int?>("Text Constraints", "MaxLength") != null)
            {
                yield return $"@{ImportType("javax.validation.constraints.Size")}(max = {attribute.GetStereotypeProperty<int?>("Text Constraints", "MaxLength"):D})";
            }

            foreach (var annotation in GetDecorators().Select(x => x.FieldAnnotations(attribute)))
            {
                yield return annotation.Trim();
            }
        }

        private IEnumerable<string> GetAnnotations(AssociationEndModel thatEnd)
        {
            var otherEnd = thatEnd.OtherEnd();
            var isManyToOne = otherEnd.IsCollection && !thatEnd.IsCollection;
            var hasAggregationForeignKey = Model.Attributes.Any(p => p.Name.Equals((thatEnd.Name + "Id").Replace("_", ""), StringComparison.OrdinalIgnoreCase));

            if (!thatEnd.TypeReference.IsNullable &&
                !(isManyToOne && hasAggregationForeignKey))
            {
                yield return $"@{ImportType("javax.validation.constraints.NotNull")}";
            }

            foreach (var annotation in GetDecorators().Select(x => x.FieldAnnotations(thatEnd)))
            {
                yield return annotation.Trim();
            }
        }
    }
}