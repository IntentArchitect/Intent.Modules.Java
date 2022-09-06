using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Intent.Engine;
using Intent.Eventing;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Maven.Templates;
using Intent.Modules.Java.Maven.Templates.PomFile;
using Intent.Modules.Java.Maven.Utils;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Java.Maven.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class PomFileProcessor : FactoryExtensionBase
    {
        private readonly ISoftwareFactoryEventDispatcher _sfEventDispatcher;
        private readonly IChanges _changeManager;
        private const string OverwriteFileCommand = "Intent.SoftwareFactory.OverwriteFileCommand"; // https://github.com/IntentSoftware/Intent.Modules.NET/blob/release/3.3.x/Modules/Intent.Modules.VisualStudio.Projects/Events/SoftwareFactoryEvents.cs#L24-L24
        public override string Id => "Intent.Java.Maven.PomFileProcessor";
        private readonly ICollection<JavaDependency> _javaDependencies = new List<JavaDependency>();
        private JavaDependency _projectInheritance;

        public PomFileProcessor(ISoftwareFactoryEventDispatcher sfEventDispatcher, IChanges changeManager)
        {
            _sfEventDispatcher = sfEventDispatcher;
            _changeManager = changeManager;
        }

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        protected override void OnStart(IApplication application)
        {
            application.EventDispatcher.Subscribe<JavaDependency>(javaDependency => _javaDependencies.Add(javaDependency));
            application.EventDispatcher.Subscribe<MavenProjectInheritanceRequest>(mavenProjectInheritanceRequest => _projectInheritance = mavenProjectInheritanceRequest);
        }

        protected override void OnAfterTemplateExecution(IApplication application)
        {
            var pomFileTemplate = application.FindTemplateInstance<PomFileTemplate>(PomFileTemplate.TemplateId);
            var pomFilePath = pomFileTemplate.FileMetadata.GetFilePath();

            var change = _changeManager.FindChange(pomFilePath);
            var content = change?.Content;
            if (content == null &&
                !pomFileTemplate.TryGetExistingFileContent(out content))
            {
                throw new Exception("Could not get content for POM file.");
            }

            var updatedContent = Process(content);
            if (IsSemanticallyTheSame(content, updatedContent))
            {
                return;
            }

            if (change == null)
            {
                _sfEventDispatcher.Publish(new SoftwareFactoryEvent(
                    eventIdentifier: OverwriteFileCommand,
                    additionalInfo: new Dictionary<string, string>
                    {
                        ["FullFileName"] = pomFilePath,
                        ["Context"] = pomFileTemplate.ToString(),
                        ["Content"] = string.Empty
                    }));

                change = _changeManager.FindChange(pomFilePath);
            }

            change.ChangeContent(updatedContent);
        }

        private string Process(string content)
        {
            var doc = XDocument.Parse(content, LoadOptions.PreserveWhitespace);

            var namespaces = new XmlNamespaceManager(new NameTable());
            var @namespace = doc.Root!.GetDefaultNamespace();
            namespaces.AddNamespace("ns", @namespace.NamespaceName);

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

        private static bool IsSemanticallyTheSame(string original, string updated)
        {
            return XDocument.Parse(original).ToString() == XDocument.Parse(updated).ToString();
        }
    }
}