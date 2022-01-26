using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EntityRepositoryTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Spring.Data.Repositories.EntityRepository";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public EntityRepositoryTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}Repository",
                package: $"{OutputTarget.GetPackage()}"
            );
        }

        private string GetEntityType()
        {
            return GetTypeName(DomainModelTemplate.TemplateId, Model);
        }

        private string GetEntityIdType()
        {
            return "Integer";
        }

        private IEnumerable<string> GetQueriesFromIndexes()
        {
            if (Model.Attributes.Any(x => x.HasIndex()))
            {
                var indexes = Model.Attributes
                    .Where(x => x.HasIndex())
                    .GroupBy(x => x.GetIndex().UniqueKey() ?? $"IX_{Model.Name}_{x.Name.ToCamelCase()}");

                return indexes.Select(x => $"{ImportType("java.util.List")}<{this.GetDomainModelName()}> findBy{string.Join("And", x.Select(c => c.Name.ToPascalCase()))}({string.Join(", ", x.Select(c => $"{GetTypeName(c)} {c.Name.ToCamelCase()}"))});");
            }

            return new string[0];
        }
    }
}