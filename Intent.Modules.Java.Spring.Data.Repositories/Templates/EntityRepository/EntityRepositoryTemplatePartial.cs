using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.TypeResolution;
using Intent.Modules.Java.Domain.Events;
using Intent.Modules.Java.Domain.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Metadata.RDBMS.Api.Indexes;
using Intent.Modules.Metadata.RDBMS.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EntityRepositoryTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel, EntityRepositoryDecorator>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Spring.Data.Repositories.EntityRepository";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public EntityRepositoryTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
            AddTypeSource(DomainModelTemplate.TemplateId);
            ExecutionContext.EventDispatcher.Subscribe<DomainEntityTypeSourceAvailableEvent>(Handle);
        }

        private void Handle(DomainEntityTypeSourceAvailableEvent @event)
        {
            AddTypeSource(@event.TypeSource);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}Repository",
                package: this.GetPackage(),
                relativeLocation: this.GetPackageFolderPath()
            );
        }

        private string GetEntityType()
        {
            return GetTypeName(Model.InternalElement);
        }

        private string GetEntityIdType()
        {
            return OutputTarget.ExecutionContext.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
            {
                DatabaseSettings.KeyTypeOptionsEnum.Guid => ImportType("java.util.UUID"),
                DatabaseSettings.KeyTypeOptionsEnum.Long => "Long",
                DatabaseSettings.KeyTypeOptionsEnum.Int => "Integer",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private IEnumerable<string> GetQueriesFromIndexes()
        {
            return Model.GetIndexes()
                .Select(x =>
                {
                    var returnType = $"{ImportType("java.util.List")}<{this.GetDomainModelName()}>";
                    var methodName = $"findBy{string.Join("And", x.KeyColumns.Select(c => c.Name.ToPascalCase()))}";
                    var parameters = $"{string.Join(", ", x.KeyColumns.Select(c => $"{GetTypeName(c.SourceType.TypeReference)} {c.Name.ToCamelCase()}"))}";

                    return $"{returnType} {methodName}({parameters});";
                });
        }
    }
}