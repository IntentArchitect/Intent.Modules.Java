using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
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
        private ICollection<JavaDependency> _javaDependencies = new List<JavaDependency>();
        private JavaDependency _projectInheritance;

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Maven.PomFile";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public PomFileTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<JavaDependency>(Handle);
            ExecutionContext.EventDispatcher.Subscribe<MavenProjectInheritanceRequest>(Handle);
        }

        private void Handle(JavaDependency dependency)
        {
            _javaDependencies.Add(dependency);
        }

        private void Handle(MavenProjectInheritanceRequest dependency)
        {
            _projectInheritance = dependency;
        }

        public string GroupId => OutputTarget.Application.SolutionName.ToDotCase();
        public string ArtifactId => OutputTarget.Application.Name.ToDotCase();
        public string Version => "1.0.0";
        public string Name => OutputTarget.Application.Name;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"pom",
                fileExtension: "xml"
            );
        }

        public override string RunTemplate()
        {
            var doc = File.Exists(GetMetadata().GetFilePath()) ? XDocument.Load(GetMetadata().GetFilePath(), LoadOptions.PreserveWhitespace) : XDocument.Parse(TransformText(), LoadOptions.PreserveWhitespace);
            var namespaces = new XmlNamespaceManager(new NameTable());
            var _namespace = doc.Root.GetDefaultNamespace();
            namespaces.AddNamespace("ns", _namespace.NamespaceName);
            // manipulate and save

            if (_projectInheritance != null)
            {
                var modelVersionElement = doc.XPathSelectElement("ns:project/ns:modelVersion", namespaces);
                if (modelVersionElement != null && doc.XPathSelectElement("ns:project/ns:parent", namespaces) == null)
                {
                    var element = XElement.Parse($@"
    <parent>
		<groupId>{_projectInheritance.GroupId}</groupId>
		<artifactId>{_projectInheritance.ArtifactId}</artifactId>
		<version>{_projectInheritance.Version}</version>
	</parent>", LoadOptions.PreserveWhitespace);
                    foreach (XElement e in element.DescendantsAndSelf())
                    {
                        e.Name = _namespace + e.Name.LocalName; // remove namespaces
                    }
                    modelVersionElement.AddAfterSelf(element);
                    modelVersionElement.AddAfterSelf(@"
	");
                }
            }

            foreach (var dependency in _javaDependencies)
            {

                var existing = doc.XPathSelectElement($"ns:project/ns:dependencies/ns:dependency[ns:groupId[text() = \"{dependency.GroupId}\"] and ns:artifactId[text() = \"{dependency.ArtifactId}\"]]", namespaces);
                if (existing == null)
                {
                    var dependencies = doc.XPathSelectElement($"ns:project/ns:dependencies", namespaces);
                    var newDependency = XElement.Parse($@"<dependency>
            <groupId>{dependency.GroupId}</groupId>
            <artifactId>{dependency.ArtifactId}</artifactId>
        </dependency>", LoadOptions.PreserveWhitespace);
                    if (!string.IsNullOrWhiteSpace(dependency.Version))
                    {
                        newDependency.Add("    ", XElement.Parse($@"<version>{dependency.Version}</version>"));
                        newDependency.Add(Environment.NewLine + "        ");
                    }
                    foreach (XElement e in newDependency.DescendantsAndSelf())
                    {
                        e.Name = _namespace + e.Name.LocalName; // remove namespaces
                    }
                    dependencies?.Add("    ", newDependency);
                    dependencies?.Add(Environment.NewLine + "    ");
                }
            }

            return doc.ToStringUTF8();
        }
    }
}