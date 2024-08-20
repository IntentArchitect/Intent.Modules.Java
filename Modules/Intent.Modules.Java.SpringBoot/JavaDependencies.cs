using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.SpringBoot;

public static class JavaDependencies
{
    public static JavaDependency SpringBootStarter = new("org.springframework.boot", "spring-boot-starter");
    public static JavaDependency SpringBootStarterWeb = new("org.springframework.boot", "spring-boot-starter-web");
    public static JavaDependency Lombok = new("org.projectlombok", "lombok", optional: true); // For now for other modules

    public static JavaDependency LombokVersioned(Settings.SpringBoot.TargetVersionOptionsEnum version)
    {
        return version switch
        {
            Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => new ("org.projectlombok", "lombok", optional: true),
            Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => new ("org.projectlombok", "lombok", version: "1.18.30", optional: true)
        };
    }
}