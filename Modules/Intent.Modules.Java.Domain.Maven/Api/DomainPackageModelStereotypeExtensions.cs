using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.Domain.Maven.Api
{
    public static class DomainPackageModelStereotypeExtensions
    {
        public static MavenDependency GetMavenDependency(this DomainPackageModel model)
        {
            var stereotype = model.GetStereotype("090c5c0c-f1b9-457f-ba2e-841312d9e98f");
            return stereotype != null ? new MavenDependency(stereotype) : null;
        }


        public static bool HasMavenDependency(this DomainPackageModel model)
        {
            return model.HasStereotype("090c5c0c-f1b9-457f-ba2e-841312d9e98f");
        }

        public static bool TryGetMavenDependency(this DomainPackageModel model, out MavenDependency stereotype)
        {
            if (!HasMavenDependency(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new MavenDependency(model.GetStereotype("090c5c0c-f1b9-457f-ba2e-841312d9e98f"));
            return true;
        }

        public class MavenDependency
        {
            private IStereotype _stereotype;

            public MavenDependency(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string Package()
            {
                return _stereotype.GetProperty<string>("Package");
            }

            public string GroupId()
            {
                return _stereotype.GetProperty<string>("GroupId");
            }

            public string ArtifactId()
            {
                return _stereotype.GetProperty<string>("ArtifactId");
            }

            public string Version()
            {
                return _stereotype.GetProperty<string>("Version");
            }

        }

    }
}