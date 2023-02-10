using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.Services.Templates.ExceptionType
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class ExceptionTypeTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

{this.IntentIgnoreBodyAnnotation()}
public class {ClassName} extends Exception {{
}}
";
        }
    }
}