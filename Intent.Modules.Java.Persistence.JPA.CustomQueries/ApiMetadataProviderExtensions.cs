using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiMetadataProviderExtensions", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.CustomQueries.Api
{
    public static class ApiMetadataProviderExtensions
    {
        public static IList<CustomQueryModel> GetCustomQueryModels(this IDesigner designer)
        {
            return designer.GetElementsOfType(CustomQueryModel.SpecializationTypeId)
                .Select(x => new CustomQueryModel(x))
                .ToList();
        }

        public static IList<QueryResultModel> GetQueryResultModels(this IDesigner designer)
        {
            return designer.GetElementsOfType(QueryResultModel.SpecializationTypeId)
                .Select(x => new QueryResultModel(x))
                .ToList();
        }

    }
}