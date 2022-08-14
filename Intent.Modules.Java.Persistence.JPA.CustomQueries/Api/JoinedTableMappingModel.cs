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
    public class JoinedTableMappingModel : IMetadataModel, IHasStereotypes, IHasName
    {
        public const string SpecializationType = "Joined Table Mapping";
        public const string SpecializationTypeId = "e96d7695-e8ed-470f-8592-f23f3e5713ce";
        protected readonly IElement _element;

        [IntentManaged(Mode.Fully)]
        public JoinedTableMappingModel(IElement element, string requiredType = SpecializationType)
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

        public IList<JoinedTableModel> JoinedTables => _element.ChildElements
            .GetElementsOfType(JoinedTableModel.SpecializationTypeId)
            .Select(x => new JoinedTableModel(x))
            .ToList();

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(JoinedTableMappingModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JoinedTableMappingModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class JoinedTableMappingModelExtensions
    {

        public static bool IsJoinedTableMappingModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == JoinedTableMappingModel.SpecializationTypeId;
        }

        public static JoinedTableMappingModel AsJoinedTableMappingModel(this ICanBeReferencedType type)
        {
            return type.IsJoinedTableMappingModel() ? new JoinedTableMappingModel((IElement)type) : null;
        }

        public static bool HasMapFromDomainMapping(this JoinedTableMappingModel type)
        {
            return type.Mapping?.MappingSettingsId == "b1d32e1d-024a-4099-b870-8365ff0efaeb";
        }

        public static IElementMapping GetMapFromDomainMapping(this JoinedTableMappingModel type)
        {
            return type.HasMapFromDomainMapping() ? type.Mapping : null;
        }
    }
}