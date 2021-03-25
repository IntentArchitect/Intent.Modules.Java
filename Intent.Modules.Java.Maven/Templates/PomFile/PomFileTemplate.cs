// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Intent.Modules.Java.Maven.Templates.PomFile
{
    using System.Collections.Generic;
    using System.Linq;
    using Intent.Modules.Common;
    using Intent.Modules.Common.Templates;
    using Intent.Metadata.Models;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Maven\Templates\PomFile\PomFileTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class PomFileTemplate : IntentTemplateBase<object>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" 
         xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
		 xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">
	<modelVersion>4.0.0</modelVersion>

	<groupId>");
            
            #line 14 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Maven\Templates\PomFile\PomFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GroupId));
            
            #line default
            #line hidden
            this.Write("</groupId>\r\n\t<artifactId>");
            
            #line 15 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Maven\Templates\PomFile\PomFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ArtifactId));
            
            #line default
            #line hidden
            this.Write("</artifactId>\r\n\t<version>");
            
            #line 16 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Maven\Templates\PomFile\PomFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Version));
            
            #line default
            #line hidden
            this.Write("</version>\r\n\t<name>");
            
            #line 17 "C:\Dev\Intent.Modules.Java\Intent.Modules.Java.Maven\Templates\PomFile\PomFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            
            #line default
            #line hidden
            this.Write("</name>\r\n\t<description></description>\r\n\r\n\t<properties>\r\n\t\t<java.version>14</java." +
                    "version>\r\n\t</properties>\r\n\r\n\t<distributionManagement>\r\n\t\t\r\n\t</distributionManage" +
                    "ment>\r\n\r\n\t<dependencies>\r\n\r\n\t</dependencies>\r\n\r\n\t<profiles>\r\n\t\t\r\n\t</profiles>\r\n\r" +
                    "\n</project>\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
