// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.SpringBoot.Templates.RestController
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Modules.Common.Java.Templates;
    using Intent.Templates;
    using Intent.Metadata.Models;
    using Intent.Modelers.Services.Api;
    using Intent.Metadata.WebApi.Api;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class RestControllerTemplate : JavaTemplateBase<Intent.Modelers.Services.Api.ServiceModel, Intent.Modules.Java.SpringBoot.Templates.RestController.RestControllerDecorator>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("package ");
            
            #line 12 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Package));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nimport lombok.AllArgsConstructor;\r\nimport org.springframework.http.HttpStatu" +
                    "s;\r\nimport org.springframework.http.ResponseEntity;\r\nimport org.springframework." +
                    "web.bind.annotation.*;\r\n\r\n@RestController\r\n");
            
            #line 20 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetControllerAnnotations()));
            
            #line default
            #line hidden
            this.Write("\r\n@AllArgsConstructor\r\npublic class ");
            
            #line 22 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" {\r\n    private final ");
            
            #line 23 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetServiceInterfaceName()));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 23 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetServiceInterfaceName().ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 24 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
  foreach(var operation in Model.Operations.Where(p => p.HasHttpSettings())) { 
            
            #line default
            #line hidden
            this.Write("\r\n    ");
            
            #line 26 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetOperationAnnotations(operation)));
            
            #line default
            #line hidden
            this.Write("\r\n    public ");
            
            #line 27 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetReturnType(operation)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 27 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(operation.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 27 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetParameters(operation)));
            
            #line default
            #line hidden
            this.Write(") {\r\n");
            
            #line 28 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"

        var hasCheckedExceptions = HasCheckedExceptions(operation);
        if (hasCheckedExceptions)
        {
            PushIndent("    ");

            
            #line default
            #line hidden
            this.Write("    try {\r\n");
            
            #line 35 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
      }

        if (operation.ReturnType != null) { 
            
            #line default
            #line hidden
            this.Write("        final ");
            
            #line 38 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetTypeName(operation)));
            
            #line default
            #line hidden
            this.Write(" result = ");
            
            #line 38 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetServiceInterfaceName().ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 38 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(operation.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 38 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.GetArguments(operation.Parameters)));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 39 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
          if (!operation.ReturnType.IsNullable && GetTypeInfo(operation.ReturnType).Template != null && !operation.ReturnType.IsCollection) { 
            
            #line default
            #line hidden
            this.Write("        if (result == null) {\r\n            return new ResponseEntity<>(HttpStatus" +
                    ".NOT_FOUND);\r\n        }\r\n");
            
            #line 43 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
          } 
            
            #line default
            #line hidden
            this.Write("\r\n        return new ResponseEntity<>(");
            
            #line 45 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResultValue(operation)));
            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 45 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetHttpResponseCode()));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 46 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
      } else { 
            
            #line default
            #line hidden
            this.Write("        ");
            
            #line 47 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetServiceInterfaceName().ToCamelCase()));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 47 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(operation.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 47 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.GetArguments(operation.Parameters)));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 48 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
      }

        if (hasCheckedExceptions)
        {
            PopIndent();
            foreach (var checkedException in GetCheckedExceptions(operation))
            { 
            
            #line default
            #line hidden
            this.Write("        } catch (");
            
            #line 55 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(checkedException.Types));
            
            #line default
            #line hidden
            this.Write(" e) {\r\n");
            
            #line 56 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"

                if (checkedException.Log) { 
            
            #line default
            #line hidden
            this.Write("            log.error(e.getMessage(), e);\r\n");
            
            #line 59 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
              } 
            
            #line default
            #line hidden
            this.Write("            throw new ");
            
            #line 60 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ImportType("org.springframework.web.server.ResponseStatusException")));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 60 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ImportType("org.springframework.http.HttpStatus")));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 60 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(checkedException.HttpStatus));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 61 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
          } 
            
            #line default
            #line hidden
            this.Write("        }\r\n");
            
            #line 63 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"
      } 
            
            #line default
            #line hidden
            this.Write("    }\r\n");
            
            #line 65 "C:\Dev\Intent.Modules.Java\Modules\Intent.Modules.Java.SpringBoot\Templates\RestController\RestControllerTemplate.tt"

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
