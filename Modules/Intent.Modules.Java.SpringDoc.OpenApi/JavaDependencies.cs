using System;
using Intent.Engine;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.SpringBoot.Settings;

namespace Intent.Modules.Java.SpringDoc.OpenApi;

public static class JavaDependencies
{
    public static JavaDependency SpringDocOpenapiUi(IApplication application)
    {
        return application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
        {
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => 
                new JavaDependency("org.springdoc", "springdoc-openapi-ui", "1.6.12"),
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => 
                new JavaDependency("org.springdoc", "springdoc-openapi-starter-webmvc-ui", "2.2.0"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static JavaDependency SwaggerAnnotations(IApplication application)
    {
        return application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
        {
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => 
                new JavaDependency("io.swagger.core.v3", "swagger-annotations", "2.2.4"),
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => 
                new JavaDependency("io.swagger.core.v3", "swagger-annotations-jakarta", "2.2.15"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}