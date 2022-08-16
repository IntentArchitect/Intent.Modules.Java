using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.CustomQueries.Api
{
    public static class ParameterModelStereotypeExtensions
    {
        public static ParameterSettings GetParameterSettings(this ParameterModel model)
        {
            var stereotype = model.GetStereotype("Parameter Settings");
            return stereotype != null ? new ParameterSettings(stereotype) : null;
        }


        public static bool HasParameterSettings(this ParameterModel model)
        {
            return model.HasStereotype("Parameter Settings");
        }

        public static bool TryGetParameterSettings(this ParameterModel model, out ParameterSettings stereotype)
        {
            if (!HasParameterSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new ParameterSettings(model.GetStereotype("Parameter Settings"));
            return true;
        }

        public class ParameterSettings
        {
            private IStereotype _stereotype;

            public ParameterSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool ExcludeFromParameterList()
            {
                return _stereotype.GetProperty<bool>("Exclude From Parameter List");
            }

        }

    }
}