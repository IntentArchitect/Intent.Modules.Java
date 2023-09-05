using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.SpringBoot.Validation.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.SpringBoot.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Utils;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Validation.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class DtoValidationDecorator : DataTransferModelDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.SpringBoot.Validation.DtoValidationDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly DataTransferModelTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public DtoValidationDecorator(DataTransferModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _template.AddDependency(JavaDependencies.SpringBootValidation);
        }

        private string UseValidationConstraint(string name)
        {
            return _template.ImportType($"{JavaxJakarta()}.validation.constraints.{name}");
        }

        private string JavaxJakarta()
        {
            return _application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => "javax",
                Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => "jakarta",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override IEnumerable<string> GetFieldAnnotations(DTOFieldModel field)
        {
            if (!field.TryGetValidations(out DTOFieldModelStereotypeExtensions.Validations validations))
            {
                var maxLength = GetTextConstraintsMaxLength(field);
                if (maxLength.HasValue)
                {
                    yield return $"@{UseValidationConstraint("Size")}(max = {maxLength})";
                }

                if (!field.TypeReference.IsNullable)
                {
                    yield return $"@{UseValidationConstraint("NotNull")}";
                }

                yield break;
            }

            if (!field.TypeReference.IsCollection &&
                validations.AssertFalse() &&
                IsAnyOf(field, "bool", "boolean", "Boolean"))
            {
                yield return $"@{UseValidationConstraint("AssertFalse")}";
            }

            if (!field.TypeReference.IsCollection &&
                validations.AssertTrue() &&
                IsAnyOf(field, "bool", "boolean", "Boolean"))
            {
                yield return $"@{UseValidationConstraint("AssertTrue")}";
            }

            if (!field.TypeReference.IsCollection &&
                validations.DecimalMax().HasValue &&
                IsAnyOf(field, "BigDecimal", "BigInteger", "CharSequence", "byte", "short", "int", "long", "Byte", "Short", "Integer", "Long", "decimal"))
            {
                var isInclusive = validations.DecimalMaxInclusive()
                    ? ", inclusive = true"
                    : string.Empty;

                yield return $"@{UseValidationConstraint("DecimalMax")}(value = \"{validations.DecimalMax()!.Value}\"{isInclusive})";
            }

            if (!field.TypeReference.IsCollection &&
                validations.DecimalMin().HasValue &&
                IsAnyOf(field, "BigDecimal", "BigInteger", "CharSequence", "byte", "short", "int", "long", "Byte", "Short", "Integer", "Long", "decimal"))
            {
                var isInclusive = validations.DecimalMinInclusive()
                    ? ", inclusive = true"
                    : string.Empty;

                yield return $"@{UseValidationConstraint("DecimalMin")}(value = \"{validations.DecimalMin()!.Value}\"{isInclusive})";
            }

            if (!field.TypeReference.IsCollection &&
                (validations.DigitsFraction().HasValue || validations.DigitsInteger().HasValue) &&
                IsAnyOf(field, "BigDecimal", "BigInteger", "CharSequence", "byte", "short", "int", "long", "Byte", "Short", "Integer", "Long", "decimal"))
            {
                yield return $"@{UseValidationConstraint("Digits")}(fraction = {validations.DigitsFraction():D}, integer = {validations.DigitsInteger():D})";
            }

            if (!field.TypeReference.IsCollection &&
                validations.Future() &&
                IsAnyOf(field, "Date", "Calender", "datetime", "datetimeoffset"))
            {
                yield return $"@{UseValidationConstraint("Future")}";
            }

            if (!field.TypeReference.IsCollection &&
                validations.Max().HasValue &&
                IsAnyOf(field, "BigDecimal", "BigInteger", "CharSequence", "byte", "short", "int", "long", "Byte", "Short", "Integer", "Long", "decimal"))
            {
                yield return $"@{UseValidationConstraint("Max")}(value = {validations.Max()!.Value:D})";
            }

            if (!field.TypeReference.IsCollection &&
                validations.Min().HasValue &&
                IsAnyOf(field, "BigDecimal", "BigInteger", "CharSequence", "byte", "short", "int", "long", "Byte", "Short", "Integer", "Long", "decimal"))
            {
                yield return $"@{UseValidationConstraint("Min")}(value = {validations.Min()!.Value:D})";
            }

            if (validations.NotNull())
            {
                yield return $"@{UseValidationConstraint("NotNull")}";
            }

            if (validations.NotNull())
            {
                yield return $"@{UseValidationConstraint("Null")}";
            }

            if (!field.TypeReference.IsCollection &&
                validations.Past() &&
                IsAnyOf(field, "Date", "Calender", "datetime", "datetimeoffset"))
            {
                yield return $"@{UseValidationConstraint("Past")}";
            }

            if (!field.TypeReference.IsCollection &&
                !string.IsNullOrWhiteSpace(validations.Pattern()) &&
                IsAnyOf(field, "String", "string"))
            {
                var flags = validations.PatternFlags().Any()
                    ? $", flags = {string.Join(", ", validations.PatternFlags().Select(x => $"@{UseValidationConstraint("Pattern.Flag")}.{x}"))}"
                    : string.Empty;

                yield return $"@{UseValidationConstraint("Pattern")}(regexp = \"{validations.Pattern()}\"{flags})";
            }

            if ((validations.SizeMax().HasValue || validations.SizeMin().HasValue) &&
                (IsAnyOf(field, "String") || field.TypeReference.IsCollection))
            {
                var elements = new[]
                    {
                        validations.SizeMin().HasValue ? $"min = {validations.SizeMin()!.Value:D}" : null,
                        validations.SizeMax().HasValue ? $"max = {validations.SizeMax()!.Value:D}" : null
                    }
                    .Where(x => x != null);

                yield return $"@{UseValidationConstraint("Size")}({string.Join(", ", elements)})";
            }
        }

        private static bool IsAnyOf(IHasTypeReference hasTypeReference, params string[] types)
        {
            var typeName = hasTypeReference.TypeReference.Element?.Name;
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return false;
            }

            return types.Any(type => typeName.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks to see if the field is mapped to an attribute, then checks for the presence of a
        /// <c>Text Constraints</c> stereotype on it, then if it has <c>MaxLength</c> property with
        /// a value and if all are true, then returns its value.
        /// </summary>
        private static int? GetTextConstraintsMaxLength(DTOFieldModel dtoField)
        {
            const string stereotypeName = "Text Constraints";
            const string stereotypePropertyName = "MaxLength";

            try
            {
                if (!dtoField.InternalElement.IsMapped ||
                    !dtoField.InternalElement.MappedElement.Element.IsAttributeModel())
                {
                    return null;
                }

                var attribute = dtoField.InternalElement.MappedElement.Element.AsAttributeModel();
                if (!attribute.HasStereotype(stereotypeName))
                {
                    return null;
                }

                var maxLength = attribute.GetStereotypeProperty<int?>(stereotypeName, stereotypePropertyName);

                return maxLength;
            }
            catch (Exception exception)
            {
                Logging.Log.Debug($"Exception occurred when attempting to resolve [{stereotypePropertyName}]" +
                                  $" for DTO-Field [{dtoField.Name}] for DTO [{dtoField.InternalElement.ParentElement?.Name}]:" +
                                  $"{Environment.NewLine}{exception}");
                return null;
            }
        }
    }
}