using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modules.Java.Services.Api
{
    [IntentManaged(Mode.Fully, Signature = Mode.Fully)]
    public class ExceptionTypeModel : IMetadataModel, IHasStereotypes, IHasName, IElementWrapper, IHasFolder
    {
        public const string SpecializationType = "Exception Type";
        public const string SpecializationTypeId = "625e3f28-5e29-4cd3-92e6-9a111dbbb32c";
        protected readonly IElement _element;

        [IntentManaged(Mode.Fully)]
        public ExceptionTypeModel(IElement element, string requiredType = SpecializationType)
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

        public IElement InternalElement => _element;

        public override string ToString()
        {
            return _element.ToString();
        }

        public bool Equals(ExceptionTypeModel other)
        {
            return Equals(_element, other?._element);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExceptionTypeModel)obj);
        }

        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
    }

    [IntentManaged(Mode.Fully)]
    public static class ExceptionTypeModelExtensions
    {

        public static bool IsExceptionTypeModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == ExceptionTypeModel.SpecializationTypeId;
        }

        public static ExceptionTypeModel AsExceptionTypeModel(this ICanBeReferencedType type)
        {
            return type.IsExceptionTypeModel() ? new ExceptionTypeModel((IElement)type) : null;
        }
    }
}