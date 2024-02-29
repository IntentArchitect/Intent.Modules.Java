using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Registrations;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.TemplateRegistration.SingleFileListModel", Version = "1.0")]

namespace Intent.Modules.Java.ModelMapper.Templates.ModelMapperBean
{
    [IntentManaged(Mode.Merge, Body = Mode.Merge, Signature = Mode.Fully)]
    public class ModelMapperBeanTemplateRegistration : SingleFileListModelTemplateRegistration<DTOModel>
    {
        public override string TemplateId => ModelMapperBeanTemplate.TemplateId;

        [IntentManaged(Mode.Fully)]
        public override ITemplate CreateTemplateInstance(IOutputTarget outputTarget, IList<DTOModel> model)
        {
            return new ModelMapperBeanTemplate(outputTarget, model);
        }
        private readonly IMetadataManager _metadataManager;

        public ModelMapperBeanTemplateRegistration(IMetadataManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override IList<DTOModel> GetModels(IApplication application)
        {
            return _metadataManager.Services(application).GetDTOModels().Where(x => x.IsMapped).ToList();
        }
    }
}