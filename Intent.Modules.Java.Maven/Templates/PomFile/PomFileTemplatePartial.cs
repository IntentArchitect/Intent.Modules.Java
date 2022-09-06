using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Java.Maven.Templates.PomFile
{
    [IntentManaged(Mode.Merge)]
    partial class PomFileTemplate : IntentTemplateBase<object>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Maven.PomFile";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public PomFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
        }

        public string GroupId => ExecutionContext.GetSolutionConfig().SolutionName.ToDotCase();
        public string ArtifactId => ExecutionContext.GetApplicationConfig().Name.ToDotCase();
        public string Version => "1.0.0";
        public string Name => ExecutionContext.GetApplicationConfig().Name;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: "pom",
                fileExtension: "xml"
            );
        }

        public override string RunTemplate()
        {
            return TryGetExistingFileContent(out var content)
                ? content
                : base.RunTemplate();
        }
    }
}