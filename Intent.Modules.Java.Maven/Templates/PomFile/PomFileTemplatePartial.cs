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
using Intent.Modules.Java.Maven.Utils;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Java.Maven.Templates.PomFile
{
    [IntentManaged(Mode.Merge)]
    partial class PomFileTemplate : IntentTemplateBase<object>
    {
        private readonly ICollection<JavaDependency> _javaDependencies = new List<JavaDependency>();
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
            if (!TryGetExistingFileContent(out var content))
            {
                content = TransformText();
            }

            var doc = XDocument.Load(content, LoadOptions.PreserveWhitespace);

            var namespaces = new XmlNamespaceManager(new NameTable());
            var @namespace = doc.Root!.GetDefaultNamespace();
            namespaces.AddNamespace("ns", @namespace.NamespaceName);
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
                        e.Name = @namespace + e.Name.LocalName; // remove namespaces
                    }
                    modelVersionElement.AddAfterSelf(element);
                    modelVersionElement.AddAfterSelf(@"
    ");
                }
            }

            var projectElement = doc.XPathSelectElement("ns:project", namespaces);
            if (projectElement == null)
            {
                projectElement = new XElement("project", @namespace);
                doc.Add(projectElement);
            }

            var dependenciesElement = doc.XPathSelectElement("ns:project/ns:dependencies", namespaces);
            if (dependenciesElement == null)
            {
                dependenciesElement = new XElement("dependencies", @namespace);
                projectElement.Add(dependenciesElement);
            }

            foreach (var dependency in _javaDependencies)
            {
                var dependencyElement = doc.XPathSelectElement($"ns:project/ns:dependencies/ns:dependency[ns:groupId[text() = \"{dependency.GroupId}\"] and ns:artifactId[text() = \"{dependency.ArtifactId}\"]]", namespaces);
                if (dependencyElement == null)
                {
                    dependencyElement = XElement.Parse($@"<dependency>
            <groupId>{dependency.GroupId}</groupId>
            <artifactId>{dependency.ArtifactId}</artifactId>
        </dependency>", LoadOptions.PreserveWhitespace);
                    if (!string.IsNullOrWhiteSpace(dependency.Version))
                    {
                        dependencyElement.Add("    ", XElement.Parse($@"<version>{dependency.Version}</version>"));
                        dependencyElement.Add(Environment.NewLine + "        ");
                    }
                    foreach (var e in dependencyElement.DescendantsAndSelf())
                    {
                        e.Name = @namespace + e.Name.LocalName; // remove namespaces
                    }
                    dependenciesElement.Add("    ", dependencyElement);
                    dependenciesElement.Add(Environment.NewLine + "    ");
                }

                var version = dependencyElement.Element(XName.Get("version", @namespace.NamespaceName));
                if (version != null &&
                    dependency.Version != null &&
                    ComparableVersion.Parse(version.Value) < ComparableVersion.Parse(dependency.Version))
                {
                    version.Value = dependency.Version;
                }
            }

            // Sort dependencies
            var sortedDependencyElements = dependenciesElement
                .Elements()
                .OrderBy(x => x.Element(XName.Get("groupId", @namespace.NamespaceName))?.Value)
                .ThenBy(x => x.Element(XName.Get("artifactId", @namespace.NamespaceName))?.Value)
                .ToArray();
            dependenciesElement.RemoveAll();

            foreach (var dependencyElement in sortedDependencyElements)
            {
                dependenciesElement.Add(
                    $"{Environment.NewLine}        ",
                    dependencyElement);
            }

            if (dependenciesElement.HasElements)
            {
                dependenciesElement.Add($"{Environment.NewLine}    ");
            }

            return doc.ToStringUTF8();
        }
    }
}