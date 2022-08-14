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
    public static class ColumnModelStereotypeExtensions
    {
        public static ColumnSettings GetColumnSettings(this ColumnModel model)
        {
            var stereotype = model.GetStereotype("Column Settings");
            return stereotype != null ? new ColumnSettings(stereotype) : null;
        }


        public static bool HasColumnSettings(this ColumnModel model)
        {
            return model.HasStereotype("Column Settings");
        }

        public static bool TryGetColumnSettings(this ColumnModel model, out ColumnSettings stereotype)
        {
            if (!HasColumnSettings(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new ColumnSettings(model.GetStereotype("Column Settings"));
            return true;
        }

        public class ColumnSettings
        {
            private IStereotype _stereotype;

            public ColumnSettings(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool IncludeInResult()
            {
                return _stereotype.GetProperty<bool>("Include in result");
            }

            public WhereClauseCriteriaTypeOptions WhereClauseCriteriaType()
            {
                return new WhereClauseCriteriaTypeOptions(_stereotype.GetProperty<string>("Where clause criteria type"));
            }

            public string WhereClauseCustomCriteria()
            {
                return _stereotype.GetProperty<string>("Where clause custom criteria");
            }

            public IElement WhereClauseParameterCriteria()
            {
                return _stereotype.GetProperty<IElement>("Where clause parameter criteria");
            }

            public class WhereClauseCriteriaTypeOptions
            {
                public readonly string Value;

                public WhereClauseCriteriaTypeOptions(string value)
                {
                    Value = value;
                }

                public WhereClauseCriteriaTypeOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "Parameter":
                            return WhereClauseCriteriaTypeOptionsEnum.Parameter;
                        case "Custom":
                            return WhereClauseCriteriaTypeOptionsEnum.Custom;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsParameter()
                {
                    return Value == "Parameter";
                }
                public bool IsCustom()
                {
                    return Value == "Custom";
                }
            }

            public enum WhereClauseCriteriaTypeOptionsEnum
            {
                Parameter,
                Custom
            }
        }

    }
}