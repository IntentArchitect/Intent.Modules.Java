using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Templates.CompositeId
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class CompositeIdTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Persistence.JPA.CompositeId";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public CompositeIdTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
            AddDependency(JavaDependencies.Lombok);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}Id",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

    }
}