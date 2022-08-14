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
    public static class CustomQueryModelStereotypeExtensions
    {
        public static QuerySettings GetQuerySettings(this CustomQueryModel model)
        {
            var stereotype = model.GetStereotype("Query Settings");
            return stereotype != null ? new QuerySettings(stereotype) : null;
        }


        public static bool HasQuerySettings(this CustomQueryModel model)
        {
            return model.HasStereotype("Query Settings");
        }

        public static bool TryGetQuerySettings(this CustomQueryModel model, out QuerySettings stereotype)
        {
            if (!HasQuerySettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new QuerySettings(model.GetStereotype("Query Settings"));
            return true;
        }

        public class QuerySettings
        {
            private IStereotype _stereotype;

            public QuerySettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public string ViewName()
            {
                return _stereotype.GetProperty<string>("View Name");
            }

            public bool ReturnsCollection()
            {
                return _stereotype.GetProperty<bool>("Returns Collection");
            }

            public string Alias()
            {
                return _stereotype.GetProperty<string>("Alias");
            }

            public bool Distinct()
            {
                return _stereotype.GetProperty<bool>("Distinct");
            }

        }

    }
}