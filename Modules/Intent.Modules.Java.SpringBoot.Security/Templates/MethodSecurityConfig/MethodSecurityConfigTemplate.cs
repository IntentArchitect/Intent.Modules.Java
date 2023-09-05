using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.MethodSecurityConfig
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class MethodSecurityConfigTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.method.configuration.{GetMethodSecurityConfigurationAnnotationName()};

@Configuration
@{GetMethodSecurityConfigurationAnnotationName()}(securedEnabled = true, prePostEnabled = true)
public class {ClassName}{GetClassInheritance()} {{
}}
";
        }
    }
}