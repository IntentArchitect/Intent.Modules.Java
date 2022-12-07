using System;
using System.Collections.Generic;
using Intent.Modules.Java.Enums;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.Services.Templates.Enum
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class EnumTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText() => EnumGenerator.Generate(Model, ClassName, Package);
    }
}