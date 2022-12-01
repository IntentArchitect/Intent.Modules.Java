using System;
using System.Collections.Generic;
using Intent.Modules.Common.Java.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.Services.Templates.Enum
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class EnumTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

public enum {ClassName} {{{string.Join(Environment.NewLine, GetMembers())}
}}
";
        }

        private IEnumerable<string> GetMembers()
        {
            foreach (var literal in Model.Literals)
            {
                // Java Enums don't have the literal values like C#, etc.
                // If you want to store the literal values, you need a constructor
                // and a field that can store int's.
                // Example: https://www.baeldung.com/a-guide-to-java-enums#fields-methods-and-constructors-in-enums
                yield return $@"
    {literal.Name.ToJavaIdentifier(CapitalizationBehaviour.AsIs)}";
            }
        }
    }
}