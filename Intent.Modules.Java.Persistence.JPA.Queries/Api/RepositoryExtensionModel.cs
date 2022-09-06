using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Repositories.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.Queries.Api
{
    [IntentManaged(Mode.Fully, Signature = Mode.Fully)]
    public class RepositoryExtensionModel : RepositoryModel
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public RepositoryExtensionModel(IElement element) : base(element)
        {
        }

        public IList<QueryModel> Queries => _element.ChildElements
            .GetElementsOfType(QueryModel.SpecializationTypeId)
            .Select(x => new QueryModel(x))
            .ToList();

    }
}