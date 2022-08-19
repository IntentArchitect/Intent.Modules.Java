using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Queries.Templates.QueryProjection
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class QueryProjectionTemplate{
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

public interface {ClassName} {{{string.Join(Environment.NewLine, GetMembers())}
}}
";
        }

        private IEnumerable<string> GetMembers()
        {
            return Model.Columns
                .Select(column => @$"
    {GetTypeName(column)} get{column.Name.ToPascalCase()}();");
        }
    }
}