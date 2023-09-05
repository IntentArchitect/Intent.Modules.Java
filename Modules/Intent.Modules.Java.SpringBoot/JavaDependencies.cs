using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.SpringBoot;

public static class JavaDependencies
{
    public static JavaDependency SpringBootStarter = new("org.springframework.boot", "spring-boot-starter");
    public static JavaDependency SpringBootStarterWeb = new("org.springframework.boot", "spring-boot-starter-web");
    public static JavaDependency Lombok = new("org.projectlombok", "lombok", optional: true);
}