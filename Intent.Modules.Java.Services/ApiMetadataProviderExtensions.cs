using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiMetadataProviderExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Services.Api
{
    public static class ApiMetadataProviderExtensions
    {
        public static IList<CheckedExceptionModel> GetCheckedExceptionModels(this IDesigner designer)
        {
            return designer.GetElementsOfType(CheckedExceptionModel.SpecializationTypeId)
                .Select(x => new CheckedExceptionModel(x))
                .ToList();
        }

    }
}