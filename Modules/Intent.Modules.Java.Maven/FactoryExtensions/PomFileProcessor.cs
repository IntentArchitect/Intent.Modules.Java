using System;
using System.Collections.Generic;
using System.Linq;
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

            var updatedContent = Process(
                content: content,
                projectInheritance: _projectInheritance,
                javaDependencies: _javaDependencies);
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

        /// <remarks>
        /// Marked <see langword="internal"/> so that can be unit tested.
        /// </remarks>
        internal static string Process(string content, JavaDependency projectInheritance, IEnumerable<JavaDependency> javaDependencies)
        {
            var doc = XDocument.Parse(content, LoadOptions.PreserveWhitespace);

            var namespaces = new XmlNamespaceManager(new NameTable());
            var @namespace = doc.Root!.GetDefaultNamespace();
            namespaces.AddNamespace("ns", @namespace.NamespaceName);

            if (projectInheritance != null)
            {
                var modelVersionElement = doc.XPathSelectElement("ns:project/ns:modelVersion", namespaces);
                if (modelVersionElement != null && doc.XPathSelectElement("ns:project/ns:parent", namespaces) == null)
                {
                    var element = XElement.Parse($@"
	<parent>
		<groupId>{projectInheritance.GroupId}</groupId>
		<artifactId>{projectInheritance.ArtifactId}</artifactId>
		<version>{projectInheritance.Version}</version>
		<relativePath/> <!-- lookup parent from repository -->
	</parent>", LoadOptions.PreserveWhitespace).WithoutNamespaces(@namespace);

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

            foreach (var dependency in javaDependencies)
            {
                var dependencyElement = doc.XPathSelectElement($"ns:project/ns:dependencies/ns:dependency[ns:groupId[text() = \"{dependency.GroupId}\"] and ns:artifactId[text() = \"{dependency.ArtifactId}\"]]", namespaces);
                if (dependencyElement == null)
                {
                    dependencyElement = XElement.Parse($@"<dependency>
			<groupId>{dependency.GroupId}</groupId>
			<artifactId>{dependency.ArtifactId}</artifactId>
		</dependency>", LoadOptions.PreserveWhitespace).WithoutNamespaces(@namespace);
                    if (!string.IsNullOrWhiteSpace(dependency.Version))
                    {
                        dependencyElement.Add("\t", XElement.Parse($"<version>{dependency.Version}</version>").WithoutNamespaces(@namespace));
                        dependencyElement.Add(Environment.NewLine + "\t\t");
                    }
                    dependenciesElement.Add("\t", dependencyElement);
                    dependenciesElement.Add(Environment.NewLine + "\t");
                }

                var groupIdElement = dependencyElement.Element(XName.Get("groupId", @namespace.NamespaceName))!;
                var lastElement = groupIdElement;

                var artifactIdElement = dependencyElement.Element(XName.Get("artifactId", @namespace.NamespaceName));
                lastElement = artifactIdElement ?? lastElement;

                // <version>
                {
                    var versionElement = dependencyElement.Element(XName.Get("version", @namespace.NamespaceName));
                    if (versionElement != null &&
                        dependency.Version != null &&
                        ComparableVersion.Parse(versionElement.Value) < ComparableVersion.Parse(dependency.Version))
                    {
                        versionElement.Value = dependency.Version;
                    }

                    lastElement = versionElement ?? lastElement;
                }

                // <exclusions>
                {
                    var exclusionsElement = dependencyElement.Element(XName.Get("exclusions", @namespace.NamespaceName));
                    if (exclusionsElement == null &&
                        dependency.Exclusions?.Any() == true)
                    {
                        exclusionsElement = new XElement(XName.Get("exclusions", @namespace.NamespaceName), Environment.NewLine, "\t\t\t");
                        lastElement.AddAfterSelf(Environment.NewLine + "\t\t\t", exclusionsElement);
                    }

                    foreach (var exclusion in dependency.Exclusions ?? Enumerable.Empty<JavaDependencyExclusion>())
                    {
                        var exclusionElement = exclusionsElement.XPathSelectElement(
                            $"ns:exclusion[ns:groupId[text() = \"{exclusion.GroupId}\"] and ns:artifactId[text() = \"{exclusion.ArtifactId}\"]]", namespaces);
                        if (exclusionElement != null)
                        {
                            continue;
                        }

                        exclusionElement = XElement.Parse($@"<exclusion>
					<groupId>{exclusion.GroupId}</groupId>
					<artifactId>{exclusion.ArtifactId}</artifactId>
				</exclusion>", LoadOptions.PreserveWhitespace).WithoutNamespaces(@namespace);

                        exclusionsElement.Add("\t", exclusionElement, Environment.NewLine, "\t\t\t");
                    }

                    lastElement = exclusionsElement ?? lastElement;
                }

                // <type>
                {
                    var typeElement = dependencyElement.Element(XName.Get("type", @namespace.NamespaceName));
                    if (typeElement == null &&
                        dependency.Type != null)
                    {
                        typeElement = new XElement(XName.Get("type", @namespace.NamespaceName));
                        lastElement.AddAfterSelf(Environment.NewLine + "\t\t\t", typeElement);
                    }

                    if (typeElement != null &&
                        dependency.Type != null &&
                        !string.Equals(typeElement.Value, dependency.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        typeElement.SetValue(dependency.Type);
                    }

                    lastElement = typeElement ?? lastElement;
                }

                // <scope>
                {
                    var scopeElement = dependencyElement.Element(XName.Get("scope", @namespace.NamespaceName));
                    if (scopeElement == null &&
                        dependency.Scope != null)
                    {
                        scopeElement = new XElement(XName.Get("scope", @namespace.NamespaceName));
                        lastElement.AddAfterSelf(Environment.NewLine + "\t\t\t", scopeElement);
                    }

                    if (scopeElement != null &&
                        dependency.Scope != null &&
                        !string.Equals(scopeElement.Value, dependency.Scope.Value.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        scopeElement.SetValue(dependency.Scope.Value.ToString().ToLowerInvariant());
                    }

                    lastElement = scopeElement ?? lastElement;
                }

                // <optional>
                {
                    var optionalElement = dependencyElement.Element(XName.Get("optional", @namespace.NamespaceName));
                    if (dependency.Optional)
                    {
                        if (optionalElement == null)
                        {
                            optionalElement = new XElement(XName.Get("optional", @namespace.NamespaceName));
                            lastElement.AddAfterSelf(Environment.NewLine + "\t\t\t", optionalElement);
                        }

                        if (optionalElement.Value != "true")
                        {
                            optionalElement.SetValue("true");
                        }
                    }
                    else
                    {
                        optionalElement?.Remove();
                    }
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
                    $"{Environment.NewLine}\t\t",
                    dependencyElement);
            }

            if (dependenciesElement.HasElements)
            {
                dependenciesElement.Add($"{Environment.NewLine}\t");
            }

            return doc.ToStringUTF8();
        }

        private static bool IsSemanticallyTheSame(string original, string updated)
        {
            return XDocument.Parse(original).ToString() == XDocument.Parse(updated).ToString();
        }
    }
}