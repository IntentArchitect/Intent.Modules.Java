using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
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
        public string Description => ExecutionContext.GetApplicationConfig().Description;

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
            if (!TryGetExistingFileContent(out var content) ||
                string.IsNullOrWhiteSpace(content))
            {
                content = base.RunTemplate();
            }

            var document = XDocument.Parse(content, LoadOptions.PreserveWhitespace);
            var rootElement = document.Root;
            var ns = rootElement!.Name.Namespace;

            var groupIdElement = rootElement.Element(ns + "groupId");
            if (groupIdElement != null && string.IsNullOrWhiteSpace(groupIdElement.Value))
            {
                groupIdElement.SetValue(GroupId);
            }

            var artifactIdElement = rootElement.Element(ns + "artifactId");
            if (artifactIdElement != null && string.IsNullOrWhiteSpace(artifactIdElement.Value))
            {
                artifactIdElement.SetValue(ArtifactId);
            }

            var versionElement = rootElement.Element(ns + "version");
            if (versionElement != null && string.IsNullOrWhiteSpace(versionElement.Value))
            {
                versionElement.SetValue(Version);
            }

            var nameElement = rootElement.Element(ns + "name");
            if (nameElement != null && string.IsNullOrWhiteSpace(nameElement.Value))
            {
                nameElement.SetValue(Name);
            }

            var descriptionElement = rootElement.Element(ns + "description");
            if (descriptionElement != null && string.IsNullOrWhiteSpace(descriptionElement.Value))
            {
                descriptionElement.SetValue(Description);
            }

            return document.ToStringUTF8();
        }
    }
}