using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Services.Api
{
    public static class ExceptionTypeModelStereotypeExtensions
    {
        public static ExceptionSettings GetExceptionSettings(this ExceptionTypeModel model)
        {
            var stereotype = model.GetStereotype("Exception Settings");
            return stereotype != null ? new ExceptionSettings(stereotype) : null;
        }


        public static bool HasExceptionSettings(this ExceptionTypeModel model)
        {
            return model.HasStereotype("Exception Settings");
        }

        public static bool TryGetExceptionSettings(this ExceptionTypeModel model, out ExceptionSettings stereotype)
        {
            if (!HasExceptionSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new ExceptionSettings(model.GetStereotype("Exception Settings"));
            return true;
        }

        public class ExceptionSettings
        {
            private IStereotype _stereotype;

            public ExceptionSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool IsExternal()
            {
                return _stereotype.GetProperty<bool>("Is External");
            }

            public string Package()
            {
                return _stereotype.GetProperty<string>("Package");
            }

        }

    }
}