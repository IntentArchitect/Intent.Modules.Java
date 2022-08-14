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
    public static class JoinedTableModelStereotypeExtensions
    {
        public static JoinSettings GetJoinSettings(this JoinedTableModel model)
        {
            var stereotype = model.GetStereotype("Join Settings");
            return stereotype != null ? new JoinSettings(stereotype) : null;
        }


        public static bool HasJoinSettings(this JoinedTableModel model)
        {
            return model.HasStereotype("Join Settings");
        }

        public static bool TryGetJoinSettings(this JoinedTableModel model, out JoinSettings stereotype)
        {
            if (!HasJoinSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new JoinSettings(model.GetStereotype("Join Settings"));
            return true;
        }

        public class JoinSettings
        {
            private IStereotype _stereotype;

            public JoinSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string Alias()
            {
                return _stereotype.GetProperty<string>("Alias");
            }

        }

    }
}