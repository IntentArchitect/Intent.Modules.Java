using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Api;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ExceptionType;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceInterface
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ServiceInterfaceTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel, ServiceInterfaceDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.ServiceInterface";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ServiceInterfaceTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(DataTransferModelTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
            AddTypeSource(ExceptionTypeTemplate.TemplateId);
        }

        private string GetCheckedExceptions(OperationModel operation)
        {
            var checkedExceptions = new OperationExtensionModel(operation.InternalElement).CheckedExceptions
                .Select(GetTypeName)
                .ToArray();

            return checkedExceptions.Length == 0
                ? string.Empty
                : @$"
            throws {string.Join(", ", checkedExceptions)}";
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name.RemoveSuffix("Controller", "Service")}Service",
                package: this.GetPackage(),
                relativeLocation: this.GetPackageFolderPath()
            );
        }

    }
}