using System;
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

namespace Intent.Modules.Java.Services.Templates.ServiceImplementation;

[IntentManaged(Mode.Merge, Signature = Mode.Fully)]
public partial class
    ServiceImplementationTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel, ServiceImplementationDecorator>, IJavaFileBuilderTemplate
{
    [IntentManaged(Mode.Fully)]
    public const string TemplateId = "Intent.Java.Services.ServiceImplementation";

    [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
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
                    .AddImport("org.springframework.stereotype.Service");

                var @class = file.Classes.First();
                @class.AddAnnotation("Service")
                    .AddAnnotation("AllArgsConstructor")
                    .AddAnnotation(this.IntentMergeAnnotation());
                @class.ImplementsInterface(GetTypeName(ServiceInterface.ServiceInterfaceTemplate.TemplateId, Model));
                var dependencies = GetConstructorDependencies();
                foreach (var dependency in dependencies)
                {
                    @class.AddField(ImportType(dependency.Type), dependency.Name.ToCamelCase(),
                        field => field.Private());
                }

                foreach (var operation in Model.Operations)
                {
                    @class.AddMethod(GetTypeName(operation), operation.Name, method =>
                    {
                        method.Override();
                        var checkedExceptions = new OperationExtensionModel(operation.InternalElement).CheckedExceptions
                            .Select(GetTypeName)
                            .ToArray();
                        foreach (var exception in checkedExceptions)
                        {
                            method.Throws(exception);
                        }

                        if (operation.GetTransactionOptions() == null ||
                            operation.GetTransactionOptions().IsEnabled())
                        {
                            method.AddAnnotation(ImportType("org.springframework.transaction.annotation.Transactional"), ann =>
                            {
                                ann.AddArgument($"readOnly = {(IsReadOnly(operation) ? "true" : "false")}");

                                var transactionOptions = operation.GetTransactionOptions();
                                if (transactionOptions == null)
                                {
                                    return;
                                }

                                if (!transactionOptions.Propagation().IsRequired())
                                {
                                    var propagationEnumExpression = GetPropagationEnumExpression(transactionOptions,
                                        ImportType("org.springframework.transaction.annotation.Propagation"));
                                    ann.AddArgument($"propagation = {propagationEnumExpression}");
                                }

                                if (!transactionOptions.IsolationLevel().IsDefault())
                                {
                                    var isolationEnumExpression = GetIsolationEnumExpression(transactionOptions,
                                        ImportType("org.springframework.transaction.annotation.Isolation"));
                                    ann.AddArgument($"isolation = {isolationEnumExpression}");
                                }

                                if (transactionOptions.Timeout() != -1)
                                {
                                    ann.AddArgument($"timeout = {transactionOptions.Timeout()!.Value}");
                                }
                            });
                        }

                        method.AddAnnotation(this.IntentIgnoreBodyAnnotation());

                        foreach (var parameter in operation.Parameters)
                        {
                            method.AddParameter(GetTypeName(parameter), parameter.Name.ToCamelCase());
                        }

                        method.AddStatements(GetImplementation(operation));
                    });
                }
            });
    }

    private static string GetIsolationEnumExpression(OperationModelStereotypeExtensions.TransactionOptions transactionOptions, string isolationTypeName)
    {
        return transactionOptions.IsolationLevel().AsEnum() switch
        {
            OperationModelStereotypeExtensions.TransactionOptions.IsolationLevelOptionsEnum.Default => $"{isolationTypeName}.DEFAULT",
            OperationModelStereotypeExtensions.TransactionOptions.IsolationLevelOptionsEnum.ReadCommitted => $"{isolationTypeName}.READ_COMMITTED",
            OperationModelStereotypeExtensions.TransactionOptions.IsolationLevelOptionsEnum.ReadUncommitted => $"{isolationTypeName}.READ_UNCOMMITTED",
            OperationModelStereotypeExtensions.TransactionOptions.IsolationLevelOptionsEnum.RepeatableRead => $"{isolationTypeName}.REPEATABLE_READ",
            OperationModelStereotypeExtensions.TransactionOptions.IsolationLevelOptionsEnum.Serializable => $"{isolationTypeName}.SERIALIZABLE",
            _ => throw new ArgumentOutOfRangeException(nameof(transactionOptions.IsolationLevel), transactionOptions.IsolationLevel().Value)
        };
    }

    private static string GetPropagationEnumExpression(OperationModelStereotypeExtensions.TransactionOptions transactionOptions, string propagationTypeName)
    {
        return transactionOptions.Propagation().AsEnum() switch
        {
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.Mandatory => $"{propagationTypeName}.MANDATORY",
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.Nested => $"{propagationTypeName}.NESTED",
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.Never => $"{propagationTypeName}.NEVER",
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.NotSupported => $"{propagationTypeName}.NOT_SUPPORTED",
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.Required => $"{propagationTypeName}.REQUIRED",
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.RequiresNew => $"{propagationTypeName}.REQUIRES_NEW",
            OperationModelStereotypeExtensions.TransactionOptions.PropagationOptionsEnum.Supports => $"{propagationTypeName}.SUPPORTS",
            _ => throw new ArgumentOutOfRangeException(nameof(transactionOptions.Propagation), transactionOptions.Propagation().Value)
        };
    }

    [IntentManaged(Mode.Fully)]
    public JavaFile JavaFile { get; }

    [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
    public override ITemplateFileConfig GetTemplateFileConfig()
    {
        return JavaFile.GetConfig();
    }

    [IntentManaged(Mode.Fully)]
    public override string TransformText()
    {
        return JavaFile.ToString();
    }

    public bool IsReadOnly(OperationModel operation)
    {
        return operation.GetTransactionOptions()?.IsReadOnly() == true ||
               operation.GetStereotype("Http Settings")?.GetProperty<string>("Verb") == "GET" ||
               operation.Name.ToLower().StartsWith("find") ||
               operation.Name.ToLower().StartsWith("get") ||
               operation.Name.ToLower().StartsWith("lookup");
    }

    private string GetImplementation(OperationModel operation)
    {
        var decorator = GetDecoratorsOutput(x => x.GetImplementation(operation));
        return !string.IsNullOrWhiteSpace(decorator)
            ? decorator
            : @"throw new UnsupportedOperationException(""Your implementation here..."");";
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