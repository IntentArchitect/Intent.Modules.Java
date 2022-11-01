using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies;

public static class JavaLombokAttributeExtensions
{
    public static string Getter(this DTOFieldModel dtoField)
    {
        if (dtoField.TypeReference.Element.Name == "bool")
        {
            return $"is{dtoField.Name.ToPascalCase().RemovePrefix("Is")}";
        }
        else
        {
            return $"get{dtoField.Name.ToPascalCase()}";
        }
    }

    public static string Setter(this AttributeModel domainAttribute)
    {
        if (domainAttribute.TypeReference.Element.Name == "bool")
        {
            return $"set{domainAttribute.Name.ToPascalCase().RemovePrefix("Is")}";
        }
        else
        {
            return $"set{domainAttribute.Name.ToPascalCase()}";
        }
    }
}