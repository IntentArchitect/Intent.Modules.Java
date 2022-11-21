using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileTemplatePartial", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.Enum
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    partial class EnumTemplate : JavaTemplateBase<Intent.Modules.Common.Types.Api.EnumModel>
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Java.Domain.Enum";

        [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
        public EnumTemplate(IOutputTarget outputTarget, Intent.Modules.Common.Types.Api.EnumModel model) : base(TemplateId, outputTarget, model)
        {
        }

        [IntentManaged(Mode.Merge, Body = Mode.Ignore, Signature = Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig()
        {
            return new JavaFileConfig(
                className: $"{Model.Name}",
                package: this.GetPackage(),
                relativeLocation: this.GetFolderPath()
            );
        }

        private static string GetEnumLiterals(IEnumerable<EnumLiteralModel> literals)
        {
            return string.Join(@",
    ", literals.Select(GetEnumLiteral));
        }

        private static string GetEnumLiteral(EnumLiteralModel literal)
        {
            // Java Enums doesn't have the literal values like C#, etc.
            // If you want to store the literal values, you need a constructor
            // and a field that can store int's.
            // Example: https://www.baeldung.com/a-guide-to-java-enums#fields-methods-and-constructors-in-enums
            return $"{literal.Name.ToJavaIdentifier(CapitalizationBehaviour.AsIs)}";
        }
    }
}