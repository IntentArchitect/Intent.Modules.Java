using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Api;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.Enum;
using Intent.Modules.Java.Services.Templates.ExceptionType;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;
using JavaFileConfig = Intent.Modules.Common.Java.Templates.JavaFileConfig;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceImplementation
{
    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    public partial class ServiceImplementationTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel, ServiceImplementationDecorator>, IJavaFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Services.ServiceImplementation";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ServiceImplementationTemplate(IOutputTarget outputTarget, Intent.Modelers.Services.Api.ServiceModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.Lombok);
            AddTypeSource(DataTransferModelTemplate.TemplateId).WithCollectionFormat("java.util.List<{0}>");
            AddTypeSource(ExceptionTypeTemplate.TemplateId);
            AddTypeSource("Domain.Enum");
            AddTypeSource(EnumTemplate.TemplateId);
            AddTypeSource("Domain.Entity");

            JavaFile = new JavaFile(this.GetPackage(), this.GetFolderPath())
                .AddClass($"{Model.Name.RemoveSuffix("Controller", "Service")}ServiceImpl", c => c
                    .AddMetadata("model", Model))
                .OnBuild(file =>
                {
                    file.AddImport("lombok.AllArgsConstructor")
                        .AddImport("org.springframework.stereotype.Service")
                        .AddImport("org.springframework.transaction.annotation.Transactional");

                    var @class = file.Classes.First();
                    @class.AddAnnotation("Service")
                        .AddAnnotation("AllArgsConstructor")
                        .AddAnnotation(this.IntentMergeAnnotation());
                    @class.ImplementsInterface(GetTypeName(ServiceInterface.ServiceInterfaceTemplate.TemplateId, Model));
                    var dependencies = GetConstructorDependencies();
                    foreach (var dependency in dependencies)
                    {
                        @class.AddField(ImportType(dependency.Type), dependency.Name.ToCamelCase(), field => field.Private());
                    }

                    foreach (var operation in Model.Operations)
                    {
                        @class.AddMethod(GetTypeName(operation), operation.Name, method =>
                        {
                            method.Override();
                            method.AddAnnotation("Transactional", ann => ann.AddArgument($"readOnly = {IsReadOnly(operation)}"))
                                .AddAnnotation(this.IntentIgnoreBodyAnnotation());
                            foreach (var parameter in operation.Parameters)
                            {
                                method.AddParameter(GetTypeName(parameter), parameter.Name.ToCamelCase());
                            }

                            method.AddStatements(GetImplementation(operation));
                        });
                    }
                });
        }

        public JavaFile JavaFile { get; set; }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return JavaFile.GetConfig();
        }

        public override string TransformText()
        {
            return JavaFile.ToString();
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