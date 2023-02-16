using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiPackageExtensionModel", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.Queries.Api
{
    [IntentManaged(Mode.Merge)]
    public class DomainPackageExtensionModel : DomainPackageModel
    {
        [IntentManaged(Mode.Ignore)]
        public DomainPackageExtensionModel(IPackage package) : base(package)
        {
        }

        [IntentManaged(Mode.Fully)]
        public IList<QueryProjectionModel> QueryProjections => UnderlyingPackage.ChildElements
            .GetElementsOfType(QueryProjectionModel.SpecializationTypeId)
            .Select(x => new QueryProjectionModel(x))
            .ToList();

    }
}