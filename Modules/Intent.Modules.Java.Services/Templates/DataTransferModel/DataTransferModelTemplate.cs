// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.Services.Templates.DataTransferModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.Java.Templates;
    using Intent.Templates;
    using Intent.Metadata.Models;
    using Intent.Modelers.Services.Api;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class DataTransferModelTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.DTOModel, Intent.Modules.Java.Services.Templates.DataTransferModel.DataTransferModelDecorator>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("package ");
            
            #line 11 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Package));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nimport lombok.Data;\r\nimport lombok.NoArgsConstructor;\r\n\r\n");
            
            #line 16 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
  if (Model.Fields.Any()) { 
            
            #line default
            #line hidden
            this.Write("@");
            
            #line 17 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ImportType("lombok.AllArgsConstructor")));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 18 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            this.Write("@NoArgsConstructor\r\n@Data\r\n");
            
            #line 21 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
      foreach(var annotation in GetDecorators().SelectMany(x => x.GetClassAnnotations())) { 
            
            #line default
            #line hidden
            
            #line 22 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(annotation));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 23 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
      } 
            
            #line default
            #line hidden
            this.Write("public");
            
            #line 24 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetAbstractDefinition()));
            
            #line default
            #line hidden
            this.Write(" class ");
            
            #line 24 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            
            #line 24 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGenericTypeParameters()));
            
            #line default
            #line hidden
            
            #line 24 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetBaseType()));
            
            #line default
            #line hidden
            this.Write(" {\r\n");
            
            #line 25 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
  foreach(var field in Model.Fields) { 
            
            #line default
            #line hidden
            
            #line 26 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
      foreach(var annotation in GetDecorators().SelectMany(x => x.GetFieldAnnotations(field))) { 
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 27 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(annotation));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 28 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
      } 
            
            #line default
            #line hidden
            this.Write("    private ");
            
            #line 29 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(field.TypeReference)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 29 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 30 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 30 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Services\Templates\DataTransferModel\DataTransferModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetDecoratorsOutput(x => x.Methods())));
            
            #line default
            #line hidden
            this.Write("\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}