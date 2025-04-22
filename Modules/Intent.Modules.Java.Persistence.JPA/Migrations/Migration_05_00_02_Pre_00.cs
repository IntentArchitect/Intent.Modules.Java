using System.Linq;
using Intent.Engine;
using Intent.IArchitect.Agent.Persistence.Model;
using Intent.Modelers.Domain.Api;
using Intent.Plugins;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Migrations.OnVersionMigration", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Migrations
{
    public class Migration_05_00_02_Pre_00 : IModuleMigration
    {
        private readonly IApplicationConfigurationProvider _configurationProvider;

        public Migration_05_00_02_Pre_00(IApplicationConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        [IntentFully]
        public string ModuleId => "Intent.Java.Persistence.JPA";
        [IntentFully]
        public string ModuleVersion => "5.0.2-pre.0";

        public void Up()
        {
            var app = ApplicationPersistable.Load(_configurationProvider.GetApplicationConfig().FilePath);
            var designer = app.GetDesigner(ApiMetadataDesignerExtensions.DomainDesignerId);
            var packages = designer.GetPackages();

            foreach (var package in packages)
            {
                var changed = false;
                
                const string classModelId = "04e12b51-ed12-42a3-9667-a6aa81bb6d10";
                var classes = package.ChildElements.Where(p => p.SpecializationTypeId == classModelId);
                const string attributeModelId = "0090fb93-483e-41af-a11d-5ad2dc796adf";
                var attributes = classes.SelectMany(x => x.ChildElements.Where(p => p.SpecializationTypeId == attributeModelId));
                const string primaryKeyDefinitionId = "b99aac21-9ca4-467f-a3a6-046255a9eed6";
                var attributesWithPks = attributes.Where(p => p.Stereotypes.Any(x => x.DefinitionId == primaryKeyDefinitionId)).ToArray();

                foreach (var attr in attributesWithPks)
                {
                    var stereotype = attr.Stereotypes.First(x => x.DefinitionId == primaryKeyDefinitionId);
                    var identityProp = stereotype.Properties.Find(x => x.Name == "Identity");
                    
                    // No Identity property, no attempt to migrate that stereotype.
                    if (identityProp is null)
                    {
                        continue;
                    }

                    if (!bool.TryParse(identityProp.Value, out var identityPropValue))
                    {
                        identityPropValue = false;
                    }
                    var dataSourceProp = stereotype.Properties.Find(x => x.Name == "Data source");
                    if (dataSourceProp is null)
                    {
                        continue;
                    }
                    var dataSourcePropValue = dataSourceProp.Value;

                    // If the Data Source value is not Default, don't adjust since we can assume the developer has set this explicitly.
                    if (dataSourcePropValue != "Default" || identityPropValue == false)
                    {
                        continue;
                    }

                    dataSourceProp.Value = "Auto-generated";
                    changed = true;
                }

                if (changed)
                {
                    package.Save(true);
                }
            }
        }

        public void Down()
        {
        }
    }
}