using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.ModelMapper.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class DataTransferModelMappingDecorator : DataTransferModelDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.ModelMapper.DataTransferModelMappingDecorator";

        private readonly DataTransferModelTemplate _template;

        public DataTransferModelMappingDecorator(DataTransferModelTemplate template)
        {
            _template = template;
        }

        public override string Methods()
        {
            if (!_template.Model.IsMapped || _template.Model.Mapping.Element.SpecializationTypeId != ClassModel.SpecializationTypeId)
            {
                return null;
            }

            return $@"
    public static {_template.ClassName} mapFrom{GetEntityTypeName()}({GetEntityTypeName()} {GetEntityTypeName().ToCamelCase()}, {_template.ImportType("org.modelmapper.ModelMapper")} mapper) {{
        return mapper.map({GetEntityTypeName().ToCamelCase()}, {_template.ClassName}.class);
    }}

    public static {_template.ImportType("java.util.List")}<{_template.ClassName}> mapFrom{GetEntityTypeName().ToPluralName()}({_template.ImportType("java.util.Collection")}<{GetEntityTypeName()}> {GetEntityTypeName().ToCamelCase().ToPluralName()}, {_template.ImportType("org.modelmapper.ModelMapper")} mapper) {{
        return {GetEntityTypeName().ToCamelCase().ToPluralName()}
            .stream()
            .map({GetEntityTypeName().ToCamelCase()} -> {_template.ClassName}.mapFrom{GetEntityTypeName()}({GetEntityTypeName().ToCamelCase()}, mapper))
            .collect({_template.ImportType("java.util.stream.Collectors")}.toList());
    }}";
        }

        private string GetEntityTypeName()
        {
            return _template.GetTypeName(DomainModelTemplate.TemplateId, _template.Model.Mapping.Element);
        }
    }
}