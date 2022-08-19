using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.CustomQueries.Api
{
    [IntentManaged(Mode.Merge)]
    public class DomainPackageExtensionModel : ServicesPackageModel
    {
        [IntentManaged(Mode.Ignore)]
        public DomainPackageExtensionModel(IPackage package) : base(package)
        {
        }

        public IList<QueryResultModel> QueryResults => UnderlyingPackage.ChildElements
            .GetElementsOfType(QueryResultModel.SpecializationTypeId)
            .Select(x => new QueryResultModel(x))
            .ToList();

    }
}