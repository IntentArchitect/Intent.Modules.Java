using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using OperationModel = Intent.Modelers.Domain.Api.OperationModel;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.ModelMapper.Templates.EntityToDtoMapping
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EntityToDtoMappingTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.DTOModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.ModelMapper.EntityToDtoMapping";

        private readonly ClassModel _entity;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public EntityToDtoMappingTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.DTOModel model) : base(TemplateId, outputTarget, model)
        {
            _entity = new ClassModel((IElement)Model.Mapping.Element);
            AddTypeSource(DataTransferModelTemplate.TemplateId);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{_entity.Name}To{Model.Name}Mapping",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        private DomainModelTemplate _entityTemplate;
        public DomainModelTemplate EntityTemplate => _entityTemplate ??= GetTemplate<DomainModelTemplate>(DomainModelTemplate.TemplateId, Model.Mapping.ElementId);


        private string GetDtoType()
        {
            return GetTypeName(DataTransferModelTemplate.TemplateId, Model);
        }

        private IEnumerable<string> GetMappings()
        {
            var memberMappings = new List<string>();
            foreach (var field in Model.Fields.Where(x => x.Mapping != null))
            {
                var shouldCast = GetTypeInfo(field.TypeReference).IsPrimitive &&
                                 field.Mapping.Element?.TypeReference != null &&
                                 GetTypeInfo(field.TypeReference).Name != EntityTemplate.GetTypeInfo(field.Mapping.Element.TypeReference).Name;
                if ($"get{field.Name.ToPascalCase()}()" != GetPath(field.Mapping.Path) || shouldCast)
                {
                    memberMappings.Add($@"map().set{field.Name.ToPascalCase()}({(shouldCast ? $"({GetTypeName(field)})" : "")}source.{GetPath(field.Mapping.Path)});");
                }
            }

            return memberMappings;
        }


        private string GetPath(IEnumerable<IElementMappingPathTarget> path)
        {
            return string.Join(".", path
                .Where(x => x.Specialization != GeneralizationModel.SpecializationType)
                .Select(x => x.Specialization == OperationModel.SpecializationType ? $"{x.Name.ToPascalCase()}()" : $"get{x.Name.ToPascalCase()}()"));
        }

    }
}