using System;
using Intent.Engine;
using Intent.Modules.Common;
using Intent.Modules.Common.Java.Events;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA.Settings;
using Intent.Modules.Metadata.RDBMS.Settings;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class DatabaseProviderSetupExtension : FactoryExtensionBase
    {
        public override string Id => "Intent.Java.Persistence.JPA.DatabaseProviderSetupExtension";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        protected override void OnBeforeTemplateExecution(IApplication application)
        {
            PublishDependency(application, JavaDependencies.SpringBootStarterDataJpa(application));
            PublishDependency(application, JavaDependencies.SpringBootStarterJdbc(application));

            // https://github.com/vladmihalcea/hibernate-types#installation
            PublishDependency(application, JavaDependencies.HibernateTypes(application));

            switch (application.Settings.GetDatabaseSettings().DatabaseProvider().AsEnum())
            {
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.H2:
                    PublishDependency(application, JavaDependencies.H2Database(application));
                    break;
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Mysql:
                    PublishDependency(application, JavaDependencies.MysqlConnector(application));
                    PublishApplicationProperty(application, "spring.datasource.url",
                        $"jdbc:mysql://localhost:3306/{application.Name.ToCamelCase()}?useUnicode=true");
                    PublishApplicationProperty(application, "spring.datasource.username",
                        $"{application.Name.ToCamelCase()}");
                    PublishApplicationProperty(application, "spring.datasource.password", "");
                    PublishApplicationProperty(application, "spring.jpa.hibernate.ddl-auto", "update");
                    break;
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.Postgresql:
                    PublishDependency(application, JavaDependencies.PostgreSql(application));
                    PublishApplicationProperty(application, "spring.datasource.url",
                        $"jdbc:postgresql://localhost:5432/{application.Name.ToCamelCase()}");
                    PublishApplicationProperty(application, "spring.datasource.username",
                        $"{application.Name.ToCamelCase()}");
                    PublishApplicationProperty(application, "spring.datasource.password", "");
                    PublishApplicationProperty(application, "spring.jpa.hibernate.ddl-auto", "update");
                    break;
                case DatabaseSettingsExtensions.DatabaseProviderOptionsEnum.SqlServer:
                    PublishDependency(application, JavaDependencies.MssqlJdbc(application));

                    // https://learn.microsoft.com/azure/developer/java/spring-framework/configure-spring-data-jpa-with-azure-sql-server#configure-spring-boot-to-use-azure-sql-database
                    PublishApplicationProperty(application, "spring.datasource.url",
                        $"jdbc:sqlserver://localhost:1433;database={application.Name.ToCamelCase()};encrypt=true;trustServerCertificate=true;loginTimeout=30;");
                    PublishApplicationProperty(application, "spring.datasource.username",
                        $"{application.Name.ToCamelCase()}");
                    PublishApplicationProperty(application, "spring.datasource.password", "");
                    PublishApplicationProperty(application, "spring.jpa.hibernate.ddl-auto", "update");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void PublishDependency(IApplication application, JavaDependency dependency)
        {
            application.EventDispatcher.Publish(dependency);
        }

        private static void PublishApplicationProperty(IApplication application, string name, string value, string profile = "")
        {
            application.EventDispatcher.Publish(new ApplicationPropertyRequiredEvent(name, value, profile));
        }
    }
}