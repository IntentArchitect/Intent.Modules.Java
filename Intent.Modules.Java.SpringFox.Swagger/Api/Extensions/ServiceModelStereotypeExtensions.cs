using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.SpringFox.Swagger.Api
{
    public static class ServiceModelStereotypeExtensions
    {
        public static OpenAPISettings GetOpenAPISettings(this ServiceModel model)
        {
            var stereotype = model.GetStereotype("OpenAPI Settings");
            return stereotype != null ? new OpenAPISettings(stereotype) : null;
        }


        public static bool HasOpenAPISettings(this ServiceModel model)
        {
            return model.HasStereotype("OpenAPI Settings");
        }


        public class OpenAPISettings
        {
            private IStereotype _stereotype;

            public OpenAPISettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string SecurityRequirement()
            {
                return _stereotype.GetProperty<string>("Security Requirement");
            }

        }

    }
}