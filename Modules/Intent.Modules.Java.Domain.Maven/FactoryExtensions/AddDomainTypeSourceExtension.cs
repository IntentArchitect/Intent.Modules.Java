using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Intent.Engine;
using Intent.Java.Domain.Maven.Api;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Java.TypeResolvers;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeResolution;
using Intent.Modules.Java.Domain.Events;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Maven.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class AddDomainTypeSourceExtension : FactoryExtensionBase
    {
        public override string Id => "Intent.Java.Domain.Maven.AddDomainTypeSourceExtension";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            application.EventDispatcher.Publish(new DomainEntityTypeSourceAvailableEvent(new TypeSource(application)));
        }

        private class TypeSource : ITypeSource
        {
            private readonly IApplication _application;

            public TypeSource(IApplication application)
            {
                _application = application;
            }

            public IResolvedTypeInfo GetType(ITypeReference typeReference)
            {
                if (typeReference.Element?.Package.SpecializationTypeId != DomainPackageModel.SpecializationTypeId)
                {
                    return null;
                }

                // If the element has a Java stereotype with a set Package property, we use that
                if (typeReference.Element.HasStereotype("Java") &&
                    !string.IsNullOrWhiteSpace(typeReference.Element.GetStereotypeProperty<string>("Java", "Package")))
                {
                    return null;
                }

                var mavenDependency = new DomainPackageModel(typeReference.Element.Package).GetMavenDependency();
                if (mavenDependency == null)
                {
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(mavenDependency.GroupId()) &&
                    !string.IsNullOrWhiteSpace(mavenDependency.ArtifactId()))
                {
                    _application.EventDispatcher.Publish(new JavaDependency(
                        groupId: mavenDependency.GroupId(),
                        artifactId: mavenDependency.ArtifactId(),
                        version: mavenDependency.Version()));
                }

                return JavaResolvedTypeInfo.Create(
                    name: typeReference.Element.Name.ToPascalCase(),
                    package: mavenDependency.Package(),
                    isPrimitive: false,
                    isNullable: typeReference.IsNullable,
                    isCollection: typeReference.IsCollection,
                    typeReference: typeReference);
            }

            public IEnumerable<ITemplateDependency> GetTemplateDependencies() => Enumerable.Empty<ITemplateDependency>();
            public ICollectionFormatter CollectionFormatter => null;
            public INullableFormatter NullableFormatter => null;
        }
    }
}