using System;
using System.Collections.Generic;
using System.Text;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.Services.Templates
{
    public static class JavaTemplateTypeExtensions
    {
        public static string GetTypeAsObject<T>(this JavaTemplateBase<T> template, ITypeReference typeReference)
        {
            // TODO: This should be managed in the Intent.Common.Java as a type resolution option
            var type = template.GetTypeName(typeReference);
            switch (type)
            {
                case "int":
                    return "Integer";
                case "short":
                    return "Short";
                case "long":
                    return "Long";
                case "char":
                    return "Character";
                case "float":
                    return "Float";
                case "double":
                    return "Double";
                case "boolean":
                    return "Boolean";
                case "byte":
                    return "Byte";
                case "List<int>":
                    return "List<Integer>";
                case "List<short>":
                    return "List<Short>";
                case "List<long>":
                    return "List<Long>";
                case "List<char>":
                    return "List<Character>";
                case "List<float>":
                    return "List<Float>";
                case "List<double>":
                    return "List<Double>";
                case "List<boolean>":
                    return "List<Boolean>";
                case "List<byte>":
                    return "List<Byte>";
                default:
                    return type;
            }
        }
    }
}
