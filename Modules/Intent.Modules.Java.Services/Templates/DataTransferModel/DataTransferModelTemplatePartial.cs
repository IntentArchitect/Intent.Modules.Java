using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates.Enum;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.DataTransferModel
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class DataTransferModelTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.DTOModel, DataTransferModelDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.DataTransferModel";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public DataTransferModelTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.DTOModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(new JavaDependency("org.projectlombok", "lombok", optional: true));
            if (model.Fields.Any(x => x.TypeReference.IsCollection))
            {
                SetDefaultTypeCollectionFormat("java.util.List<{0}>");
            }
            AddTypeSource(TemplateId).WithCollectionFormat("java.util.List<{0}>");
            AddTypeSource("Domain.Enum");
            AddTypeSource(EnumTemplate.TemplateId);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name.ToPascalCase()}",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private string GetAbstractDefinition() => Model.IsAbstract
            ? " abstract"
            : string.Empty;

        public string GetBaseType() => Model.ParentDtoTypeReference != null
            ? $" extends {GetTypeName(Model.ParentDtoTypeReference)}"
            : string.Empty;

        private string GetGenericTypeParameters()
        {
            if (!Model.GenericTypes.Any())
            {
                return string.Empty;
            }

            return $"<{string.Join(',', Model.GenericTypes)}>";
        }
    }
}