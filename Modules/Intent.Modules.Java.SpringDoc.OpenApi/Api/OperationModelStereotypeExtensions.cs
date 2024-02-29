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
    public static class OperationModelStereotypeExtensions
    {
        public static OpenAPIOperation GetOpenAPIOperation(this OperationModel model)
        {
            var stereotype = model.GetStereotype("f2b1207e-01fa-4961-b997-0bb53f7e6e20");
            return stereotype != null ? new OpenAPIOperation(stereotype) : null;
        }


        public static bool HasOpenAPIOperation(this OperationModel model)
        {
            return model.HasStereotype("f2b1207e-01fa-4961-b997-0bb53f7e6e20");
        }

        public static bool TryGetOpenAPIOperation(this OperationModel model, out OpenAPIOperation stereotype)
        {
            if (!HasOpenAPIOperation(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new OpenAPIOperation(model.GetStereotype("f2b1207e-01fa-4961-b997-0bb53f7e6e20"));
            return true;
        }

        public static OpenAPITag GetOpenAPITag(this OperationModel model)
        {
            var stereotype = model.GetStereotype("7ee3b765-c3c4-4db3-aa8b-f77693b3bb4f");
            return stereotype != null ? new OpenAPITag(stereotype) : null;
        }

        public static IReadOnlyCollection<OpenAPITag> GetOpenAPITags(this OperationModel model)
        {
            var stereotypes = model
                .GetStereotypes("7ee3b765-c3c4-4db3-aa8b-f77693b3bb4f")
                .Select(stereotype => new OpenAPITag(stereotype))
                .ToArray();

            return stereotypes;
        }


        public static bool HasOpenAPITag(this OperationModel model)
        {
            return model.HasStereotype("7ee3b765-c3c4-4db3-aa8b-f77693b3bb4f");
        }

        public static bool TryGetOpenAPITag(this OperationModel model, out OpenAPITag stereotype)
        {
            if (!HasOpenAPITag(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new OpenAPITag(model.GetStereotype("7ee3b765-c3c4-4db3-aa8b-f77693b3bb4f"));
            return true;
        }

        public class OpenAPIOperation
        {
            private IStereotype _stereotype;

            public OpenAPIOperation(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string Summary()
            {
                return _stereotype.GetProperty<string>("Summary");
            }

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