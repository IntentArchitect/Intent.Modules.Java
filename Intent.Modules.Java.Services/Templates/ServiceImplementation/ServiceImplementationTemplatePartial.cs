using System.Collections;
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
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceImplementation
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ServiceImplementationTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel, ServiceImplementationDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.ServiceImplementation";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ServiceImplementationTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.Lombok);
            AddTypeSource(DataTransferModelTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name.RemoveSuffix("Controller", "Service")}ServiceImpl",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        public string IsReadOnly(OperationModel operation)
        {
            return operation.GetTransactionOptions()?.IsReadOnly().ToString().ToLower() ??
                (operation.GetStereotype("Http Settings")?.GetProperty<string>("Verb") == "GET" ? "true" : "false");
        }

        private string GetImplementation(OperationModel operation)
        {
            var decorator = GetDecoratorsOutput(x => x.GetImplementation(operation));
            return !string.IsNullOrWhiteSpace(decorator) ? decorator : @"throw new UnsupportedOperationException(""Your implementation here..."");";
        }

        private IEnumerable<ClassDependency> GetConstructorDependencies()
        {
            return GetDecorators().SelectMany(x => x.GetClassDependencies());
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
    }
}