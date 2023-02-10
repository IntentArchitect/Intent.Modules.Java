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

            return string.Join(@",", _model.Literals
                .Select(literal => $"{Environment.NewLine}    {literal.Name.ToSnakeCase().ToUpperInvariant()}{GetLiteralValue(literal)}")) + ";";
        }

        private string GetLiteralValue(EnumLiteralModel model)
        {
            return _literalType switch
            {
                LiteralType.None => string.Empty,
                LiteralType.Int => $"({model.Value.Trim()})",
                LiteralType.Long => $"({model.Value.Trim()}L)",
                LiteralType.String => $"(\"{model.Value.Trim()}\")",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static LiteralType DetermineLiteralType(EnumModel model)
        {
            if (model.Literals.All(x => string.IsNullOrWhiteSpace(x.Value)))
            {
                return LiteralType.None;
            }

            if (model.Literals.All(x => int.TryParse(x.Value.Trim(), out _)))
            {
                return LiteralType.Int;
            }

            if (model.Literals.All(x => long.TryParse(x.Value.Trim(), out _)))
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
