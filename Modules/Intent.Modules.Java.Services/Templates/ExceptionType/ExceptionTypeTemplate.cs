using System;
using System.Collections.Generic;
using Intent.Modules.Java.Weaving.Annotations.Templates;
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

{this.GetIntentIgnoreBodyName()}
public class {ClassName} extends Exception {{
}}
";
        }
    }
}