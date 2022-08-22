using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.SpringBoot.Security
{
    public static class JavaDependencies
    {
        public static JavaDependency SpringBootSecurity = new("org.springframework.boot", "spring-boot-starter-security");
        public static JavaDependency JsonWebTokenJjwtApi = new("io.jsonwebtoken", "jjwt-api", "0.11.5");
        public static JavaDependency JsonWebTokenJjwtImpl = new("io.jsonwebtoken", "jjwt-impl", "0.11.5");
        public static JavaDependency JsonWebTokenJjwtJackson = new("io.jsonwebtoken", "jjwt-jackson", "0.11.5");
    }
}
