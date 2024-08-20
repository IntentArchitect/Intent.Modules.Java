using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Templates;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies;

public static class JavaLombokAttributeExtensions
{
    public static string Getter(this DTOFieldModel dtoField)
    {
        if (dtoField.TypeReference?.HasBoolType() == true || dtoField.TypeReference?.HasJavaBooleanType() == true)
        {
            return $"is{dtoField.Name.ToPascalCase()}";
        }
        else
        {
            return $"get{dtoField.Name.ToPascalCase()}";
        }
    }

    public static string Setter(this AttributeModel domainAttribute)
    {
        if (domainAttribute.TypeReference?.HasBoolType() == true || domainAttribute.TypeReference?.HasJavaBooleanType() == true)
        {
            return $"set{domainAttribute.Name.ToPascalCase().RemovePrefix("Is")}";
        }
        else
        {
            return $"set{domainAttribute.Name.ToPascalCase()}";
        }
    }
}