using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.SpringDoc.OpenApi.Api
{
    public static class ServiceModelStereotypeExtensions
    {
        public static OpenAPITag GetOpenAPITag(this ServiceModel model)
        {
            var stereotype = model.GetStereotype("OpenAPI Tag");
            return stereotype != null ? new OpenAPITag(stereotype) : null;
        }

        public static IReadOnlyCollection<OpenAPITag> GetOpenAPITags(this ServiceModel model)
        {
            var stereotypes = model
                .GetStereotypes("OpenAPI Tag")
                .Select(stereotype => new OpenAPITag(stereotype))
                .ToArray();

            return stereotypes;
        }


        public static bool HasOpenAPITag(this ServiceModel model)
        {
            return model.HasStereotype("OpenAPI Tag");
        }

        public static bool TryGetOpenAPITag(this ServiceModel model, out OpenAPITag stereotype)
        {
            if (!HasOpenAPITag(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new OpenAPITag(model.GetStereotype("OpenAPI Tag"));
            return true;
        }

        public class OpenAPITag
        {
            private IStereotype _stereotype;

            public OpenAPITag(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string StereotypeName => _stereotype.Name;

            public string Name()
            {
                return _stereotype.GetProperty<string>("Name");
            }

            public string Description()
            {
                return _stereotype.GetProperty<string>("Description");
            }

        }

    }
}