// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.Weaving.Annotations.Templates.IntentCanAdd
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.Java.Templates;
    using Intent.Templates;
    using Intent.Metadata.Models;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Weaving.Annotations\Templates\IntentCanAdd\IntentCanAddTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class IntentCanAddTemplate : JavaTemplateBase<object>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("package ");
            
            #line 10 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Weaving.Annotations\Templates\IntentCanAdd\IntentCanAddTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Package));
            
            #line default
            #line hidden
            this.Write(@";

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
 
/**
 * Instructs Intent that it may add child members to this element.
 */
@Target({ ElementType.TYPE, ElementType.METHOD, ElementType.FIELD, ElementType.CONSTRUCTOR })
@Retention(RetentionPolicy.SOURCE)
public @interface ");
            
            #line 22 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Weaving.Annotations\Templates\IntentCanAdd\IntentCanAddTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(@"{
    /**
    * Override for the identifier for this element. 
    * Use this if you want Intent Architect to match this element to an output element, irrespective of its name or signature.
    */
    String id() default """";
    /**
    * Sets the instruction for how to manage annotations on this element.
    */
    int annotations() default 0;
}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
