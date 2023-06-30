using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Events;
using Intent.Modules.Java.Domain.Templates;
using Intent.Modules.Java.Persistence.JPA;
using Intent.Modules.Java.Persistence.JPA.Templates;
using Intent.Modules.Metadata.RDBMS.Api.Indexes;
using Intent.Modules.Metadata.RDBMS.Settings;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository
{
    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    public partial class EntityRepositoryTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel, EntityRepositoryDecorator>, IJavaFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Spring.Data.Repositories.EntityRepository";

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public EntityRepositoryTemplate(IOutputTarget outputTarget, Intent.Modelers.Domain.Api.ClassModel model) : base(TemplateId, outputTarget, model)
        {
            this.AddDomainEntityTypeSource();
            AddTypeSource("Data.QueryProjection");
            JavaFile = new JavaFile(this.GetPackage(), this.GetFolderPath())
                .AddImport("org.springframework.data.jpa.repository.JpaRepository")
                .AddInterface($"{Model.Name.ToPascalCase()}Repository")
                .OnBuild(file =>
                {
                    var inter = file.Interfaces.First();
                    inter.AddMetadata("model", Model);
                    inter.WithComments(@"Spring Data JPA repository for the <#= GetEntityType() #> entity.")
                        .AddAnnotation(this.IntentMergeAnnotation())
                        .ExtendsInterface($"JpaRepository<{GetEntityType()}, {GetEntityIdType()}>");
                })
                .AfterBuild(file =>
                {
                    var inter = file.Interfaces.First();
                    foreach (var query in GetQueriesFromIndexes())
                    {
                        inter.AddMethod(query.ReturnType, query.MethodName, method =>
                        {
                            foreach (var param in query.Params)
                            {
                                method.AddParameter(param.Key, param.Value);
                            }
                        });
                    }

                    foreach (var member in GetDecorators().SelectMany(x => x.GetMembers()))
                    {
                        inter.AddCodeBlock(member);
                    }
                });
        }

        public JavaFile JavaFile { get; }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return JavaFile.GetConfig();
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return JavaFile.ToString();
        }

        private string GetEntityType()
        {
            return GetTypeName(Model.InternalElement);
        }

        private string GetEntityIdType()
        {
            var (primaryKeys, fromClass) = Model.GetPrimaryKeys();

            return primaryKeys.Count switch
            {
                0 => OutputTarget.ExecutionContext.Settings.GetDatabaseSettings().KeyType().AsEnum() switch
                {
                    DatabaseSettings.KeyTypeOptionsEnum.Guid => ImportType("java.util.UUID"),
                    DatabaseSettings.KeyTypeOptionsEnum.Long => "Long",
                    DatabaseSettings.KeyTypeOptionsEnum.Int => "Integer",
                    _ => throw new ArgumentOutOfRangeException()
                },
                1 => GetTypeName(primaryKeys[0]),
                _ => this.GetCompositeIdName(fromClass)
            };
        }

        private IEnumerable<(string ReturnType, string MethodName, IEnumerable<KeyValuePair<string, string>> Params)> GetQueriesFromIndexes()
        {
            return Model.GetIndexes()
                .Select(x =>
                {
                    var returnType = $"{ImportType("java.util.List")}<{this.GetDomainModelName()}>";
                    var methodName = $"findBy{string.Join("And", x.KeyColumns.Select(c => c.Name.ToPascalCase()))}";
                    var parameters = x.KeyColumns.Select(c => new KeyValuePair<string, string>(GetTypeName(c.SourceType.TypeReference), c.Name.ToCamelCase()));

                    return (
                        returnType,
                        methodName,
                        parameters
                    );
                });
        }
    }
}