using System;
using Intent.Engine;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.SpringBoot.Settings;

namespace Intent.Modules.Java.BeanValidation;

public static class JavaDependencies
{
    public static JavaDependency ValidationApi(IApplication application)
    {
        return application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
        {
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 =>
                new JavaDependency("javax.validation", "validation-api"),
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => null,
            _ => null
        };
    }
}