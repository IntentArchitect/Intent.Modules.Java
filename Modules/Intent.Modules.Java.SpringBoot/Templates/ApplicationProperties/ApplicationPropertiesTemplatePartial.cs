using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Events;
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
        private readonly Dictionary<string, ApplicationProperty> _properties = new();

        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.SpringBoot.ApplicationProperties";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public ApplicationPropertiesTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            ExecutionContext.EventDispatcher.Subscribe<ApplicationPropertyRequest>(x => Handle(x.Name, x.Value, null));
            ExecutionContext.EventDispatcher.Subscribe<ApplicationPropertyRequiredEvent>(x => Handle(x.Name, x.Value, x.Profile));
        }

        private void Handle(string name, string value, string profile)
        {
            if (_properties.TryGetValue(name, out var property))
            {
                throw new Exception(@$"Application property ""{name}"" has already been registered, the stack trace at the time of the previous registration was:
{property.StackTrace}");
            }

            _properties.Add(name, new ApplicationProperty(value, profile, Environment.StackTrace));
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
                .Where(x => x.Contains('='))
                .Select(x => x.Split('=').First().Trim())
                .ToHashSet();

            foreach (var (name, (value, _, _)) in _properties)
            {
                if (existing.Contains(name))
                {
                    continue;
                }

                sb.AppendLine($"{name}={value}");
                existing.Add(name);
            }

            return sb.ToString();
        }

        private record ApplicationProperty(string Value, string Profile, string StackTrace)
        {
            public string Value { get; } = Value;
            public string Profile { get; } = Profile;
            public string StackTrace { get; } = StackTrace;
        }
    }
}