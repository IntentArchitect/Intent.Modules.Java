using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.Services
{
    public static class JavaDependencies
    {
        public static JavaDependency Lombok = new("org.projectlombok", "lombok", optional: true);
    }
}
