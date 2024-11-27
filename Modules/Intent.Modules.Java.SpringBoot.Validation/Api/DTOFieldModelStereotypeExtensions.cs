using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.SpringBoot.Validation.Api
{
    public static class DTOFieldModelStereotypeExtensions
    {
        public static Validations GetValidations(this DTOFieldModel model)
        {
            var stereotype = model.GetStereotype(Validations.DefinitionId);
            return stereotype != null ? new Validations(stereotype) : null;
        }


        public static bool HasValidations(this DTOFieldModel model)
        {
            return model.HasStereotype(Validations.DefinitionId);
        }

        public static bool TryGetValidations(this DTOFieldModel model, out Validations stereotype)
        {
            if (!HasValidations(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new Validations(model.GetStereotype(Validations.DefinitionId));
            return true;
        }

        public class Validations
        {
            private IStereotype _stereotype;
            public const string DefinitionId = "2ec1d83e-7980-435c-b8ac-8ba4f3c5d5ea";

            public Validations(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool AssertFalse()
            {
                return _stereotype.GetProperty<bool>("AssertFalse");
            }

            public bool AssertTrue()
            {
                return _stereotype.GetProperty<bool>("AssertTrue");
            }

            public int? DecimalMax()
            {
                return _stereotype.GetProperty<int?>("DecimalMax");
            }

            public bool DecimalMaxInclusive()
            {
                return _stereotype.GetProperty<bool>("DecimalMax Inclusive");
            }

            public int? DecimalMin()
            {
                return _stereotype.GetProperty<int?>("DecimalMin");
            }

            public bool DecimalMinInclusive()
            {
                return _stereotype.GetProperty<bool>("DecimalMin Inclusive");
            }

            public int? DigitsInteger()
            {
                return _stereotype.GetProperty<int?>("Digits Integer");
            }

            public int? DigitsFraction()
            {
                return _stereotype.GetProperty<int?>("Digits Fraction");
            }

            public bool Future()
            {
                return _stereotype.GetProperty<bool>("Future");
            }

            public int? Max()
            {
                return _stereotype.GetProperty<int?>("Max");
            }

            public int? Min()
            {
                return _stereotype.GetProperty<int?>("Min");
            }

            public bool NotNull()
            {
                return _stereotype.GetProperty<bool>("NotNull");
            }

            public bool Null()
            {
                return _stereotype.GetProperty<bool>("Null");
            }

            public bool Past()
            {
                return _stereotype.GetProperty<bool>("Past");
            }

            public string Pattern()
            {
                return _stereotype.GetProperty<string>("Pattern");
            }

            public PatternFlagsOptions[] PatternFlags()
            {
                return _stereotype.GetProperty<string[]>("Pattern Flags")?.Select(x => new PatternFlagsOptions(x)).ToArray() ?? new PatternFlagsOptions[0];
            }

            public int? SizeMin()
            {
                return _stereotype.GetProperty<int?>("Size Min");
            }

            public int? SizeMax()
            {
                return _stereotype.GetProperty<int?>("Size Max");
            }

            public class PatternFlagsOptions
            {
                public readonly string Value;

                public PatternFlagsOptions(string value)
                {
                    Value = value;
                }

                public PatternFlagsOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "CANON_EQ":
                            return PatternFlagsOptionsEnum.CANON_EQ;
                        case "CASE_INSENSITIVE":
                            return PatternFlagsOptionsEnum.CASE_INSENSITIVE;
                        case "COMMENTS":
                            return PatternFlagsOptionsEnum.COMMENTS;
                        case "DOTALL":
                            return PatternFlagsOptionsEnum.DOTALL;
                        case "MULTILINE":
                            return PatternFlagsOptionsEnum.MULTILINE;
                        case "UNICODE_CASE":
                            return PatternFlagsOptionsEnum.UNICODE_CASE;
                        case "UNIX_LINES":
                            return PatternFlagsOptionsEnum.UNIX_LINES;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsCANON_EQ()
                {
                    return Value == "CANON_EQ";
                }
                public bool IsCASE_INSENSITIVE()
                {
                    return Value == "CASE_INSENSITIVE";
                }
                public bool IsCOMMENTS()
                {
                    return Value == "COMMENTS";
                }
                public bool IsDOTALL()
                {
                    return Value == "DOTALL";
                }
                public bool IsMULTILINE()
                {
                    return Value == "MULTILINE";
                }
                public bool IsUNICODE_CASE()
                {
                    return Value == "UNICODE_CASE";
                }
                public bool IsUNIX_LINES()
                {
                    return Value == "UNIX_LINES";
                }
            }

            public enum PatternFlagsOptionsEnum
            {
                CANON_EQ,
                CASE_INSENSITIVE,
                COMMENTS,
                DOTALL,
                MULTILINE,
                UNICODE_CASE,
                UNIX_LINES
            }

        }

    }
}