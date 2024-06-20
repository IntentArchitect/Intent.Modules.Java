using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Api
{
    public static class ParameterModelStereotypeExtensions
    {
        public static PageableSettings GetPageableSettings(this ParameterModel model)
        {
            var stereotype = model.GetStereotype("f9297602-456c-424e-9651-21d235e04e0d");
            return stereotype != null ? new PageableSettings(stereotype) : null;
        }


        public static bool HasPageableSettings(this ParameterModel model)
        {
            return model.HasStereotype("f9297602-456c-424e-9651-21d235e04e0d");
        }

        public static bool TryGetPageableSettings(this ParameterModel model, out PageableSettings stereotype)
        {
            if (!HasPageableSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new PageableSettings(model.GetStereotype("f9297602-456c-424e-9651-21d235e04e0d"));
            return true;
        }

        public class PageableSettings
        {
            private IStereotype _stereotype;

            public PageableSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public int? DefaultPageNumber()
            {
                return _stereotype.GetProperty<int?>("Default Page Number");
            }

            public int? DefaultPageSize()
            {
                return _stereotype.GetProperty<int?>("Default Page Size");
            }

        }

    }
}