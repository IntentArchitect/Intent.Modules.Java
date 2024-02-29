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
            var stereotype = model.GetStereotype("824d3c4c-8e15-4413-afb7-6bcd372fe557");
            return stereotype != null ? new CORSSettings(stereotype) : null;
        }


        public static bool HasCORSSettings(this OperationModel model)
        {
            return model.HasStereotype("824d3c4c-8e15-4413-afb7-6bcd372fe557");
        }

        public static bool TryGetCORSSettings(this OperationModel model, out CORSSettings stereotype)
        {
            if (!HasCORSSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new CORSSettings(model.GetStereotype("824d3c4c-8e15-4413-afb7-6bcd372fe557"));
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