using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.AbstractEntity;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.DomainModel
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class DomainModelTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel, DomainModelDecorator>, IDeclareImports
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Domain.DomainModel";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public DomainModelTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
            SetDefaultTypeCollectionFormat("List<{0}>");
            AddTypeSource(TemplateId, "List<{0}>");
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        public string GetBaseClass()
        {
            return GetTypeName(AbstractEntityTemplate.TemplateId);
        }

        public IEnumerable<string> DeclareImports()
        {
            if (Model.AssociatedClasses.Any(x => x.IsCollection && x.IsNavigable))
            {
                yield return "java.util.List";
            }
        }
    }
}