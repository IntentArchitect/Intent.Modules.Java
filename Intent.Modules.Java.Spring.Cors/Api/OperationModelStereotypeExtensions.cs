using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.Spring.Cors.Api
{
    public static class OperationModelStereotypeExtensions
    {
        public static CORSSettings GetCORSSettings(this OperationModel model)
        {
            var stereotype = model.GetStereotype("CORS Settings");
            return stereotype != null ? new CORSSettings(stereotype) : null;
        }


        public static bool HasCORSSettings(this OperationModel model)
        {
            return model.HasStereotype("CORS Settings");
        }

        public static bool TryGetCORSSettings(this OperationModel model, out CORSSettings stereotype)
        {
            if (!HasCORSSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new CORSSettings(model.GetStereotype("CORS Settings"));
            return true;
        }


        public class CORSSettings
        {
            private IStereotype _stereotype;

            public CORSSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool AllowCrossOriginRequests()
            {
                return _stereotype.GetProperty<bool>("Allow Cross Origin Requests");
            }
        }

    }
}