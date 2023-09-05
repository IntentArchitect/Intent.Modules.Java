﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.Spring.Cors.Templates.CorsConfig
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
    
    #line 1 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Spring.Cors\Templates\CorsConfig\CorsConfigTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class CorsConfigTemplate : JavaTemplateBase<object>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("package ");
            
            #line 10 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Spring.Cors\Templates\CorsConfig\CorsConfigTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Package));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nimport org.springframework.beans.factory.annotation.Value;\r\nimport org.springframework.context.annotation.Configuration;\r\nimport org.springframework.context.annotation.Bean;\r\nimport org.springframework.web.servlet.config.annotation.CorsRegistry;\r\nimport org.springframework.web.servlet.config.annotation.WebMvcConfigurer;\r\n\r\n@Configuration\r\npublic class ");
            
            #line 19 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.Spring.Cors\Templates\CorsConfig\CorsConfigTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" {\r\n    @Value(\"${cors.origin}\")\r\n    private String origin;\r\n\r\n    @Bean\r\n    public WebMvcConfigurer corsConfigurer() {\r\n        return new WebMvcConfigurer() {\r\n            @Override\r\n            public void addCorsMappings(CorsRegistry registry) {\r\n                registry.addMapping(\"/api/**\")\r\n                        .allowedOrigins(origin)\r\n                        .allowedMethods(\"*\")\r\n                        .maxAge(3600)\r\n                        .allowedHeaders(\"*\")\r\n                        .exposedHeaders(\"*\");\r\n            }\r\n        };\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
}
