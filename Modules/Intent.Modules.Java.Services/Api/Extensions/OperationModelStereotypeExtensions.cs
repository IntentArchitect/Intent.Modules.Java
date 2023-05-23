using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Services.Api
{
    public static class OperationModelStereotypeExtensions
    {
        public static TransactionOptions GetTransactionOptions(this OperationModel model)
        {
            var stereotype = model.GetStereotype("Transaction Options");
            return stereotype != null ? new TransactionOptions(stereotype) : null;
        }

        public static bool HasTransactionOptions(this OperationModel model)
        {
            return model.HasStereotype("Transaction Options");
        }

        public static bool TryGetTransactionOptions(this OperationModel model, out TransactionOptions stereotype)
        {
            if (!HasTransactionOptions(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new TransactionOptions(model.GetStereotype("Transaction Options"));
            return true;
        }


        public class TransactionOptions
        {
            private IStereotype _stereotype;

            public TransactionOptions(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool IsEnabled()
            {
                return _stereotype.GetProperty<bool>("Is Enabled");
            }

            public bool IsReadOnly()
            {
                return _stereotype.GetProperty<bool>("Is Read Only");
            }

            public int? Timeout()
            {
                return _stereotype.GetProperty<int?>("Timeout");
            }

            public IsolationLevelOptions IsolationLevel()
            {
                return new IsolationLevelOptions(_stereotype.GetProperty<string>("Isolation Level"));
            }

            public PropagationOptions Propagation()
            {
                return new PropagationOptions(_stereotype.GetProperty<string>("Propagation"));
            }

            public class IsolationLevelOptions
            {
                public readonly string Value;

                public IsolationLevelOptions(string value)
                {
                    Value = value;
                }

                public IsolationLevelOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "Default":
                            return IsolationLevelOptionsEnum.Default;
                        case "Read Committed":
                            return IsolationLevelOptionsEnum.ReadCommitted;
                        case "Read Uncommitted":
                            return IsolationLevelOptionsEnum.ReadUncommitted;
                        case "Repeatable Read":
                            return IsolationLevelOptionsEnum.RepeatableRead;
                        case "Serializable":
                            return IsolationLevelOptionsEnum.Serializable;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsDefault()
                {
                    return Value == "Default";
                }
                public bool IsReadCommitted()
                {
                    return Value == "Read Committed";
                }
                public bool IsReadUncommitted()
                {
                    return Value == "Read Uncommitted";
                }
                public bool IsRepeatableRead()
                {
                    return Value == "Repeatable Read";
                }
                public bool IsSerializable()
                {
                    return Value == "Serializable";
                }
            }

            public enum IsolationLevelOptionsEnum
            {
                Default,
                ReadCommitted,
                ReadUncommitted,
                RepeatableRead,
                Serializable
            }
            public class PropagationOptions
            {
                public readonly string Value;

                public PropagationOptions(string value)
                {
                    Value = value;
                }

                public PropagationOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "Required":
                            return PropagationOptionsEnum.Required;
                        case "Supports":
                            return PropagationOptionsEnum.Supports;
                        case "Mandatory":
                            return PropagationOptionsEnum.Mandatory;
                        case "Requires New":
                            return PropagationOptionsEnum.RequiresNew;
                        case "Not Supported":
                            return PropagationOptionsEnum.NotSupported;
                        case "Never":
                            return PropagationOptionsEnum.Never;
                        case "Nested":
                            return PropagationOptionsEnum.Nested;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsRequired()
                {
                    return Value == "Required";
                }
                public bool IsSupports()
                {
                    return Value == "Supports";
                }
                public bool IsMandatory()
                {
                    return Value == "Mandatory";
                }
                public bool IsRequiresNew()
                {
                    return Value == "Requires New";
                }
                public bool IsNotSupported()
                {
                    return Value == "Not Supported";
                }
                public bool IsNever()
                {
                    return Value == "Never";
                }
                public bool IsNested()
                {
                    return Value == "Nested";
                }
            }

            public enum PropagationOptionsEnum
            {
                Required,
                Supports,
                Mandatory,
                RequiresNew,
                NotSupported,
                Never,
                Nested
            }

        }

    }
}