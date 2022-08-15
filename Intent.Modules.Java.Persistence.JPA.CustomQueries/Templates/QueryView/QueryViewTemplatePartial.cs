using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Java.Persistence.JPA.CustomQueries.Api;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.CustomQueries.Templates.QueryView
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class QueryViewTemplate : JavaTemplateBase<Intent.Java.Persistence.JPA.CustomQueries.Api.CustomQueryModel>
    {
        private readonly bool _canRun;

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Persistence.JPA.CustomQueries.QueryView";

        [IntentManaged(Mode.Merge)]
        public QueryViewTemplate(IOutputTarget outputTarget, CustomQueryModel model, bool canRun) : base(TemplateId, outputTarget, model)
        {
            _canRun = canRun;
        }

        public override bool CanRunTemplate()
        {
            return _canRun;
        }

        public override string RunTemplate()
        {
            return base.RunTemplate();
        }

        public IEnumerable<ColumnModel> GetFields()
        {
            IEnumerable<ColumnModel> GetFields(IElement element)
            {
                var columnModel = element.AsColumnModel();
                if (columnModel?.GetColumnSettings()?.IncludeInResult() == true)
                {
                    yield return columnModel;
                }

                foreach (var childColumnModel in element.ChildElements.SelectMany(GetFields))
                {
                    yield return childColumnModel;
                }
            }

            return GetFields(Model.InternalElement);
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.GetQuerySettings().ViewName()}",
                package: $"{OutputTarget.GetPackage()}.{Model.InternalElement.ParentElement.Name.ToJavaPackage()}",
                relativeLocation: this.GetFolderPath(Model.InternalElement.ParentElement.Name)
            );
        }
    }
}