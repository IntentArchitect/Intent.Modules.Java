using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Events;
using Intent.Modules.Java.Domain.Templates;
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

        [IntentManaged(Mode.Fully)]
        private readonly DataTransferModelTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public DataTransferModelMappingDecorator(DataTransferModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            _template.AddDomainEntityTypeSource();
        }

        public override string Methods()
        {
            if (!_template.Model.IsMapped || _template.Model.Mapping.Element.SpecializationTypeId != ClassModel.SpecializationTypeId)
            {
                return null;
            }

            var typeName = GetEntityTypeName();
            var typeNameCamelCased = typeName.ToCamelCase();
            var typeNamePascalCased = typeName.ToPascalCase();

            var mapFromParamName = typeNameCamelCased.Pluralize();
            var mapLambdaParamName = typeNameCamelCased;

            if (mapLambdaParamName == mapFromParamName)
            {
                mapLambdaParamName = $"nested{typeNamePascalCased}";
            }

            return $@"
    public static {_template.ClassName} mapFrom{typeNamePascalCased}({typeName} {typeNameCamelCased}, {_template.ImportType("org.modelmapper.ModelMapper")} mapper) {{
        return mapper.map({typeNameCamelCased}, {_template.ClassName}.class);
    }}

    public static {_template.ImportType("java.util.List")}<{_template.ClassName}> mapFrom{typeNamePascalCased.Pluralize()}({_template.ImportType("java.util.Collection")}<{typeName}> {mapFromParamName}, {_template.ImportType("org.modelmapper.ModelMapper")} mapper) {{
        return {mapFromParamName}
            .stream()
            .map({mapLambdaParamName} -> {_template.ClassName}.mapFrom{typeName}({mapLambdaParamName}, mapper))
            .collect({_template.ImportType("java.util.stream.Collectors")}.toList());
    }}";
        }

        private string GetEntityTypeName()
        {
            return _template.GetDomainModelName(((IElement)_template.Model.Mapping.Element).AsClassModel());
        }
    }
}