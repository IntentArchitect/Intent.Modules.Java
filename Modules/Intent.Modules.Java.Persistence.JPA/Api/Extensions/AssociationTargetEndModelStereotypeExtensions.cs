using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.Api
{
    public static class AssociationTargetEndModelStereotypeExtensions
    {
        public static AssociationJPASettings GetAssociationJPASettings(this AssociationTargetEndModel model)
        {
            var stereotype = model.GetStereotype(AssociationJPASettings.DefinitionId);
            return stereotype != null ? new AssociationJPASettings(stereotype) : null;
        }


        public static bool HasAssociationJPASettings(this AssociationTargetEndModel model)
        {
            return model.HasStereotype(AssociationJPASettings.DefinitionId);
        }

        public static bool TryGetAssociationJPASettings(this AssociationTargetEndModel model, out AssociationJPASettings stereotype)
        {
            if (!HasAssociationJPASettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new AssociationJPASettings(model.GetStereotype(AssociationJPASettings.DefinitionId));
            return true;
        }


        public class AssociationJPASettings
        {
            private IStereotype _stereotype;
            public const string DefinitionId = "49de8cd7-872f-4b37-b9a2-2056304434dd";

            public AssociationJPASettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public FetchTypeOptions FetchType()
            {
                return new FetchTypeOptions(_stereotype.GetProperty<string>("Fetch Type"));
            }

            public class FetchTypeOptions
            {
                public readonly string Value;

                public FetchTypeOptions(string value)
                {
                    Value = value;
                }

                public FetchTypeOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "Eager":
                            return FetchTypeOptionsEnum.Eager;
                        case "Lazy":
                            return FetchTypeOptionsEnum.Lazy;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsEager()
                {
                    return Value == "Eager";
                }
                public bool IsLazy()
                {
                    return Value == "Lazy";
                }
            }

            public enum FetchTypeOptionsEnum
            {
                Eager,
                Lazy
            }
        }

    }
}