using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.SpringBoot.Events;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Templates.ApplicationProperties
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class ApplicationPropertiesTemplate : IntentTemplateBase<object>
    {
        private readonly List<ApplicationPropertyRequest> _requests = new();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.ApplicationProperties";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ApplicationPropertiesTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<ApplicationPropertyRequest>(Handle);
        }

        private void Handle(ApplicationPropertyRequest request)
        {
            _requests.Add(request);
        }

        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new TemplateFileConfig(
                fileName: $"application",
                fileExtension: "properties"
            );
        }

        public override string TransformText()
        {
            if (!TryGetExistingFileContent(out var content))
            {
                content = string.Empty;
            }

            var sb = new StringBuilder(content);

            var existing = content
                .Replace("\r\n", "\n")
                .Split('\n')
                .Select(x => x.Split('=').FirstOrDefault())
                .ToHashSet();

            foreach (var request in _requests)
            {
                if (existing.Contains(request.Name))
                {
                    continue;
                }

                sb.AppendLine($"{request.Name}={request.Value}");
                existing.Add(request.Name);
            }

            return sb.ToString();
        }
    }
}