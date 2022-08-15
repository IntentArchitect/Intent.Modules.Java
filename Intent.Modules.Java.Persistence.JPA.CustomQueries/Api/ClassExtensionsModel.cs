using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementExtensionModel", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.CustomQueries.Api
{
    [IntentManaged(Mode.Fully, Signature = Mode.Fully)]
    public class ClassExtensionsModel : ClassModel
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public ClassExtensionsModel(IElement element) : base(element)
        {
        }

        public IList<CustomQueryModel> CustomQueries => _element.ChildElements
            .GetElementsOfType(CustomQueryModel.SpecializationTypeId)
            .Select(x => new CustomQueryModel(x))
            .ToList();

    }
}