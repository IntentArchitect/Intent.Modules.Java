using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Templates.CompositeId
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class CompositeIdTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

import java.io.Serializable;
import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
@EqualsAndHashCode
public class {ClassName} implements Serializable {{{string.Join(Environment.NewLine, GetMembers())}
}}
";
        }

        private IEnumerable<string> GetMembers()
        {
            var primaryKeyAttributes = Model.Attributes.Where(x => x.HasPrimaryKey()).ToArray();

            foreach (var attribute in primaryKeyAttributes)
            {
                yield return $@"
    private {GetTypeName(attribute)} {attribute.Name};";
            }
        }
    }
}