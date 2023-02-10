using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Java.Persistence.JPA.Queries.Api
{
    [IntentManaged(Mode.Fully, Signature = Mode.Fully)]
    public class QueryProjectionModel : IMetadataModel, IHasStereotypes, IHasName, IHasFolder
    {
        public const string SpecializationType = "Query Projection";
        public const string SpecializationTypeId = "b8819c07-fa52-4e38-a92c-439e21220c55";
        protected readonly IElement _element;

        [IntentManaged(Mode.Fully)]
        public QueryProjectionModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
            Folder = _element.ParentElement?.SpecializationTypeId == FolderModel.SpecializationTypeId ? new FolderModel(_element.ParentElement) : null;
        }

        public string Id => _element.Id;

        public string Name => _element.Name;

        public string Comment => _element.Comment;

        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        public FolderModel Folder { get; }

        public bool IsMapped => _element.IsMapped;

        public IElementMapping Mapping => _element.MappedElement;

        public IElement InternalElement => _element;

        public IList<ColumnModel> Columns => _element.ChildElements
            .GetElementsOfType(ColumnModel.SpecializationTypeId)
            .Select(x => new ColumnModel(x))
            .ToList();

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(QueryProjectionModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((QueryProjectionModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class QueryProjectionModelExtensions
    {

        public static bool IsQueryProjectionModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == QueryProjectionModel.SpecializationTypeId;
        }

        public static QueryProjectionModel AsQueryProjectionModel(this ICanBeReferencedType type)
        {
            return type.IsQueryProjectionModel() ? new QueryProjectionModel((IElement)type) : null;
        }

        public static bool HasProjectFromClassMapping(this QueryProjectionModel type)
        {
            return type.Mapping?.MappingSettingsId == "23a4e11b-f188-4fd3-b1f7-ed3fa17e4447";
        }

        public static IElementMapping GetProjectFromClassMapping(this QueryProjectionModel type)
        {
            return type.HasProjectFromClassMapping() ? type.Mapping : null;
        }
    }
}