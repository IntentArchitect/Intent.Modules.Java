using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Validation.Templates.RestResponseEntityExceptionHandler
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class RestResponseEntityExceptionHandlerTemplate : JavaTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.Validation.RestResponseEntityExceptionHandler";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public RestResponseEntityExceptionHandlerTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.SpringBootValidation);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"RestResponseEntityExceptionHandler",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private string GetHttpStatusType()
        {
            return ExecutionContext.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => ImportType("org.springframework.http.HttpStatus"),
                Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => ImportType("org.springframework.http.HttpStatusCode"),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}