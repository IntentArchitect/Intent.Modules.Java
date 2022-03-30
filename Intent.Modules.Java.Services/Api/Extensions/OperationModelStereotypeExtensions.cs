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

        public static IReadOnlyCollection<TransactionOptions> GetTransactionOptionss(this OperationModel model)
        {
            var stereotypes = model
                .GetStereotypes("Transaction Options")
                .Select(stereotype => new TransactionOptions(stereotype))
                .ToArray();

            return stereotypes;
        }

        public static bool HasTransactionOptions(this OperationModel model)
        {
            return model.HasStereotype("Transaction Options");
        }


        public class TransactionOptions
        {
            private IStereotype _stereotype;

            public TransactionOptions(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public bool IsReadOnly()
            {
                return _stereotype.GetProperty<bool>("Is Read Only");
            }

        }

    }
}