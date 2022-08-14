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
    public class JoinedTableModel : IMetadataModel, IHasStereotypes, IHasName
    {
        public const string SpecializationType = "Joined Table";
        public const string SpecializationTypeId = "41566d5b-85bf-4608-ba72-3d43b7a2cb07";
        protected readonly IElement _element;

        [IntentManaged(Mode.Fully)]
        public JoinedTableModel(IElement element, string requiredType = SpecializationType)
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

        public IElement InternalElement => _element;

        public JoinedTableMappingModel CreateMapping => _element.ChildElements
            .GetElementsOfType(JoinedTableMappingModel.SpecializationTypeId)
            .Select(x => new JoinedTableMappingModel(x))
            .SingleOrDefault();

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(JoinedTableModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JoinedTableModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class JoinedTableModelExtensions
    {

        public static bool IsJoinedTableModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == JoinedTableModel.SpecializationTypeId;
        }

        public static JoinedTableModel AsJoinedTableModel(this ICanBeReferencedType type)
        {
            return type.IsJoinedTableModel() ? new JoinedTableModel((IElement)type) : null;
        }
    }
}