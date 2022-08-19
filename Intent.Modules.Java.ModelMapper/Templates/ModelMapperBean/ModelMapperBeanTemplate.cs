// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.ModelMapper.Templates.ModelMapperBean
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
    
    #line 1 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.ModelMapper\Templates\ModelMapperBean\ModelMapperBeanTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class ModelMapperBeanTemplate : JavaTemplateBase<IList<Intent.Modelers.Services.Api.DTOModel>>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("package ");
            
            #line 10 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.ModelMapper\Templates\ModelMapperBean\ModelMapperBeanTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Package));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nimport org.modelmapper.ModelMapper;\r\nimport org.springframework.context.anno" +
                    "tation.Bean;\r\nimport org.springframework.context.annotation.Configuration;\r\n\r\n@C" +
                    "onfiguration\r\npublic class ");
            
            #line 17 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.ModelMapper\Templates\ModelMapperBean\ModelMapperBeanTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" {\r\n    @Bean\r\n    public ModelMapper modelMapper() {\r\n        var modelMapper = " +
                    "new ModelMapper();\r\n\r\n        InitializeMappings(modelMapper);\r\n\r\n        return" +
                    " modelMapper;\r\n    }\r\n\r\n    private void InitializeMappings(ModelMapper modelMap" +
                    "per) {\r\n");
            
            #line 28 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.ModelMapper\Templates\ModelMapperBean\ModelMapperBeanTemplate.tt"
  foreach(var mapping in GetMappings()) { 
            
            #line default
            #line hidden
            this.Write("        modelMapper.addMappings(new ");
            
            #line 29 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.ModelMapper\Templates\ModelMapperBean\ModelMapperBeanTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(mapping));
            
            #line default
            #line hidden
            this.Write("());\r\n");
            
            #line 30 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.ModelMapper\Templates\ModelMapperBean\ModelMapperBeanTemplate.tt"
  } 
            
            #line default
            #line hidden
            this.Write("    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
