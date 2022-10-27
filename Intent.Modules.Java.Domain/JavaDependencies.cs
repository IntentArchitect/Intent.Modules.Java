using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.Domain
{
    public static class JavaDependencies
    {
        public static JavaDependency Lombok = new("org.projectlombok", "lombok", optional: true);
    }
}
