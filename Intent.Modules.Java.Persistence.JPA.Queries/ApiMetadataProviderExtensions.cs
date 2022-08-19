using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiMetadataProviderExtensions", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.Queries.Api
{
    public static class ApiMetadataProviderExtensions
    {
        public static IList<QueryModel> GetQueryModels(this IDesigner designer)
        {
            return designer.GetElementsOfType(QueryModel.SpecializationTypeId)
                .Select(x => new QueryModel(x))
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