using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.Queries.Api
{
    public static class QueryModelStereotypeExtensions
    {
        public static QuerySettings GetQuerySettings(this QueryModel model)
        {
            var stereotype = model.GetStereotype("5ffc1a1f-cb80-4451-a70e-db3738491e0e");
            return stereotype != null ? new QuerySettings(stereotype) : null;
        }


        public static bool HasQuerySettings(this QueryModel model)
        {
            return model.HasStereotype("5ffc1a1f-cb80-4451-a70e-db3738491e0e");
        }

        public static bool TryGetQuerySettings(this QueryModel model, out QuerySettings stereotype)
        {
            if (!HasQuerySettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new QuerySettings(model.GetStereotype("5ffc1a1f-cb80-4451-a70e-db3738491e0e"));
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

            public string TableAlias()
            {
                return _stereotype.GetProperty<string>("Table Alias");
            }

            public bool Distinct()
            {
                return _stereotype.GetProperty<bool>("Distinct");
            }

        }

    }
}