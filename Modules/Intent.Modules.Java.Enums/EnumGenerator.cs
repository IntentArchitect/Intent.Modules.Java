using System;
using System.Linq;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;

namespace Intent.Modules.Java.Enums
{
    // NOTE: This file is included in both the "Intent.Modules.Java.Domain" and
    // "Intent.Modules.Java.Services" projects.
    public class EnumGenerator
    {
        private readonly EnumModel _model;
        private readonly string _className;
        private readonly string _package;
        private readonly LiteralType _literalType;

        private EnumGenerator(EnumModel model, string className, string package)
        {
            _model = model;
            _className = className;
            _package = package;
            _literalType = DetermineLiteralType(model);
        }

        public static string Generate(EnumModel model, string className, string package) =>
            new EnumGenerator(model, className, package).Generate();

        private string Generate()
        {
            return $@"
package {_package};

import com.fasterxml.jackson.annotation.JsonValue;

public enum {_className} {{{GetLiterals()}{GetMembers()}
}}";
        }

        private string GetMembers()
        {
            if (_literalType == LiteralType.None)
            {
                return string.Empty;
            }

            var type = _literalType switch
            {
                LiteralType.Int => "int",
                LiteralType.Long => "long",
                LiteralType.String => "String",
                _ => throw new ArgumentOutOfRangeException()
            };

            return $@"

    @JsonValue
    public final {type} value;

    private {_className}({type} value) {{
        this.value = value;
    }}";
        }

        private string GetLiterals()
        {
            if (!_model.Literals.Any())
            {
                return string.Empty;
            }

            var currentNumber = -1L;

            return string.Join(@",", _model.Literals
                .Select(literal => $"{Environment.NewLine}    {literal.Name.ToSnakeCase().ToUpperInvariant()}{GetLiteralValue(literal)}")) + ";";

            string GetLiteralValue(EnumLiteralModel literal)
            {
                switch (_literalType)
                {
                    case LiteralType.None:
                        return string.Empty;
                    case LiteralType.Int:
                        currentNumber = string.IsNullOrWhiteSpace(literal.Value?.Trim())
                            ? currentNumber += 1
                            : int.Parse(literal.Value.Trim());
                        return $"({currentNumber:D})";
                    case LiteralType.Long:
                        currentNumber = string.IsNullOrWhiteSpace(literal.Value?.Trim())
                            ? currentNumber += 1
                            : long.Parse(literal.Value.Trim());
                        return $"({currentNumber:D}L)";
                    case LiteralType.String:
                        return $"(\"{(literal.Value ?? literal.Name).Trim()}\")";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static LiteralType DetermineLiteralType(EnumModel model)
        {
            if (model.Literals.All(x => string.IsNullOrWhiteSpace(x.Value?.Trim())))
            {
                return LiteralType.None;
            }

            if (model.Literals.Any(x => int.TryParse(x.Value?.Trim(), out _)) &&
                model.Literals.All(x => string.IsNullOrWhiteSpace(x.Value?.Trim()) || int.TryParse(x.Value.Trim(), out _)))
            {
                return LiteralType.Int;
            }

            if (model.Literals.Any(x => long.TryParse(x.Value?.Trim(), out _)) &&
                model.Literals.All(x => string.IsNullOrWhiteSpace(x.Value?.Trim()) || long.TryParse(x.Value.Trim(), out _)))
            {
                return LiteralType.Long;
            }

            return LiteralType.String;
        }

        private enum LiteralType
        {
            None,
            String,
            Int,
            Long
        }
    }
}
