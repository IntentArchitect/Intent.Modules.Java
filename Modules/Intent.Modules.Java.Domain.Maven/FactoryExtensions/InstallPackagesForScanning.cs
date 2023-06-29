using System.Linq;
using System.Text;
using Intent.Engine;
using Intent.Java.Domain.Maven.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Maven.FactoryExtensions;

[IntentManaged(Mode.Fully, Body = Mode.Merge)]
public class InstallPackagesForScanning : FactoryExtensionBase
{
    public override string Id => "Intent.Java.Domain.Maven.InstallPackagesForScanning";

    [IntentManaged(Mode.Ignore)] public override int Order => 0;

    protected override void OnAfterTemplateRegistrations(IApplication application)
    {
        // Parking this until we can get hold of packages that are marked as External
        
        // var appTemplate = application.FindTemplateInstance<IJavaFileBuilderTemplate>(TemplateDependency.OnTemplate("Startup.Application"));
        // if (appTemplate is null) { return; }
        //
        // var packages = application.MetadataManager.Domain(application.Id).GetDomainPackageModels();
        // if (!packages.Any(p => p.HasMavenDependency())) { return; }
        //
        // appTemplate.JavaFile.OnBuild(file =>
        // {
        //     var priClass = file.Classes.First();
        //     var method = priClass.FindMethod("main");
        //     var annotation = method?.Annotations.FirstOrDefault(p => p.ToString().Contains("SpringBootApplication"));
        //     if (annotation is null) { return; }
        //
        //     method.Annotations.Remove(annotation);
        //     method.AddAnnotation("SpringBootApplication", annotation =>
        //     {
        //         var sb = new StringBuilder(128);
        //         sb.Append("scanBasePackages = { ");
        //         sb.Append(@$"""{appTemplate.Namespace}""");
        //         foreach (var package in packages)
        //         {
        //             sb.Append(@$", ""{package.GetMavenDependency().Package()}""");
        //         }
        //         sb.Append(" }");
        //         annotation.AddArgument(sb.ToString());
        //     });
        // });
    }
}