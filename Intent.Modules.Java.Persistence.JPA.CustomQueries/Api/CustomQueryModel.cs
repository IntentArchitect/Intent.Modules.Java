using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.CustomQueries.Api
{
    [IntentManaged(Mode.Fully, Signature = Mode.Fully)]
    public class CustomQueryModel : IMetadataModel, IHasStereotypes, IHasName, IHasTypeReference
    {
        public const string SpecializationType = "Custom Query";
        public const string SpecializationTypeId = "4276d179-00df-4105-bad6-a467a06a799b";
        protected readonly IElement _element;

        [IntentManaged(Mode.Fully)]
        public CustomQueryModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        public string Id => _element.Id;

        public string Name => _element.Name;

        public string Comment => _element.Comment;

        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        public ITypeReference TypeReference => _element.TypeReference;

        public bool IsMapped => _element.IsMapped;

        public IElementMapping Mapping => _element.MappedElement;

        public IElement InternalElement => _element;

        public IList<ParameterModel> Parameters => _element.ChildElements
            .GetElementsOfType(ParameterModel.SpecializationTypeId)
            .Select(x => new ParameterModel(x))
            .ToList();

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(CustomQueryModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomQueryModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class CustomQueryModelExtensions
    {

        public static bool IsCustomQueryModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == CustomQueryModel.SpecializationTypeId;
        }

        public static CustomQueryModel AsCustomQueryModel(this ICanBeReferencedType type)
        {
            return type.IsCustomQueryModel() ? new CustomQueryModel((IElement)type) : null;
        }

        public static bool HasMapParametersMapping(this CustomQueryModel type)
        {
            return type.Mapping?.MappingSettingsId == "2290d513-2fd9-4425-8f88-14e17c008162";
        }

        public static IElementMapping GetMapParametersMapping(this CustomQueryModel type)
        {
            return type.HasMapParametersMapping() ? type.Mapping : null;
        }
    }
}