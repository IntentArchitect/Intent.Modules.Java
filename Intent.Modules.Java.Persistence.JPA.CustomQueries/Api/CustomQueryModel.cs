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
    public class CustomQueryModel : IMetadataModel, IHasStereotypes, IHasName
    {
        public const string SpecializationType = "Custom Query";
        public const string SpecializationTypeId = "174c0034-1b66-4042-834c-cfb1e05df263";
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

        public bool IsMapped => _element.IsMapped;

        public IElementMapping Mapping => _element.MappedElement;

        public IElement InternalElement => _element;

        public IList<ColumnModel> Columns => _element.ChildElements
            .GetElementsOfType(ColumnModel.SpecializationTypeId)
            .Select(x => new ColumnModel(x))
            .ToList();

        public IList<QueryParameterModel> Parameters => _element.ChildElements
            .GetElementsOfType(QueryParameterModel.SpecializationTypeId)
            .Select(x => new QueryParameterModel(x))
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

        public static bool HasMapFromDomainMapping(this CustomQueryModel type)
        {
            return type.Mapping?.MappingSettingsId == "27656313-0a7d-4fb1-ba8a-013cf8ee5263";
        }

        public static IElementMapping GetMapFromDomainMapping(this CustomQueryModel type)
        {
            return type.HasMapFromDomainMapping() ? type.Mapping : null;
        }
    }
}