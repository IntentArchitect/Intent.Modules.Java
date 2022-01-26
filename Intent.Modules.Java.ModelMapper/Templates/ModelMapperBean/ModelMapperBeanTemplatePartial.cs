using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.ModelMapper.Templates.EntityToDtoMapping;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.ModelMapper.Templates.ModelMapperBean
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ModelMapperBeanTemplate : JavaTemplateBase<IList<Intent.Modelers.Services.Api.DTOModel>>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.ModelMapper.ModelMapperBean";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public ModelMapperBeanTemplate(IOutputTarget outputTarget, IList<Intent.Modelers.Services.Api.DTOModel> model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency(groupId: "org.modelmapper", artifactId: "modelmapper", version: "2.3.8"));
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"ModelMapperBean",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        private IEnumerable<string> GetMappings()
        {
            foreach (var mappedDto in Model)
            {
                yield return GetTypeName(EntityToDtoMappingTemplate.TemplateId, mappedDto);
            }
        }
    }
}