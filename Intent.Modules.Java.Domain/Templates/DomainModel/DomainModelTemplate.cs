// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.Domain.Templates.DomainModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.Java.Templates;
    using Intent.Templates;
    using Intent.Metadata.Models;
    using Intent.Modelers.Domain.Api;
    using Intent.Modules.Java.Weaving.Annotations.Templates.IntentIgnoreBody;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class DomainModelTemplate : JavaTemplateBase<Intent.Modelers.Domain.Api.ClassModel, Intent.Modules.Java.Domain.Templates.DomainModel.DomainModelDecorator>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("package ");
            
            #line 12 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Package));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nimport lombok.NoArgsConstructor;\r\n\r\n");
            
            #line 16 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  foreach (var annotation in GetClassAnnotations()) { 
            
            #line default
            #line hidden
            
            #line 17 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(annotation));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 18 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            this.Write("public");
            
            #line 19 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetAbstractDefinition()));
            
            #line default
            #line hidden
            this.Write(" class ");
            
            #line 19 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            
            #line 19 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericTypeParameters()));
            
            #line default
            #line hidden
            
            #line 19 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetBaseType()));
            
            #line default
            #line hidden
            this.Write(" {\r\n");
            
            #line 20 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  if (!Model.IsAbstract) { 
            
            #line default
            #line hidden
            this.Write("    private static final long serialVersionUID = 1L;\r\n");
            
            #line 22 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 23 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  foreach(var field in GetDecorators().SelectMany(x => x.Fields())) { 
            
            #line default
            #line hidden
            this.Write("\r\n    ");
            
            #line 25 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 26 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 27 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  foreach(var attribute in Model.Attributes) 
    {

            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 31 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
      foreach (var annotation in GetAnnotations(attribute))
        {

            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 34 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(annotation));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 35 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
      }

            
            #line default
            #line hidden
            this.Write("    private ");
            
            #line 37 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(attribute)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 37 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 38 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 39 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  foreach(var associationEnd in Model.AssociatedClasses.Where(x => x.IsNavigable)) { 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 41 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
      foreach (var annotation in GetAnnotations(associationEnd))
        {

            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 44 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(annotation));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 45 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
      }

            
            #line default
            #line hidden
            this.Write("    private ");
            
            #line 47 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(associationEnd)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 47 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(associationEnd.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 48 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 49 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  foreach(var operation in Model.Operations) { 
            
            #line default
            #line hidden
            this.Write("\r\n    @");
            
            #line 51 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(IntentIgnoreBodyTemplate.TemplateId)));
            
            #line default
            #line hidden
            this.Write("\r\n    public ");
            
            #line 52 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(operation)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 52 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(operation.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 52 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.GetMethodParameters(operation.Parameters)));
            
            #line default
            #line hidden
            this.Write(") {\r\n        throw new UnsupportedOperationException(\"Write your implementation h" +
                    "ere...\");\r\n    }\r\n");
            
            #line 55 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 56 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  foreach(var method in GetDecorators().SelectMany(x => x.Methods())) { 
            
            #line default
            #line hidden
            this.Write("\r\n    ");
            
            #line 58 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 59 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Domain\Templates\DomainModel\DomainModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            this.Write("}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
