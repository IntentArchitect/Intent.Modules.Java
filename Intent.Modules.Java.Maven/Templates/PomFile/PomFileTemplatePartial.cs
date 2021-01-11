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

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Maven.PomFile";

        public PomFileTemplate(IOutputTarget outputTarget, object model) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<JavaDependency>(Handle);
        }

        private void Handle(JavaDependency dependency)
        {
            _javaDependencies.Add(dependency);
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
            // manipulate and save

            foreach (var dependency in _javaDependencies)
            {
                var namespaces = new XmlNamespaceManager(new NameTable());
                var _namespace = doc.Root.GetDefaultNamespace();
                namespaces.AddNamespace("ns", _namespace.NamespaceName);
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
                        e.Name = _namespace + e.Name.LocalName;
                    }
                    dependencies?.Add("    ", newDependency);
                    dependencies?.Add(Environment.NewLine + "    ");
                }
            }

            return doc.ToStringUTF8();
        }
    }
}