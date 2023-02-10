using System;
using Intent.Configuration;
using Intent.Engine;
using Intent.Modules.Common.Templates;
using Intent.Modules.Metadata.RDBMS.Settings;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Settings.ModuleSettingsExtensions", Version = "1.0")]

namespace Intent.Modules.Java.Persistence.JPA.Settings
{

    public static class DatabaseSettingsExtensions
    {
        public static TableNamingConventionOptions TableNamingConvention(this DatabaseSettings groupSettings) => new TableNamingConventionOptions(groupSettings.GetSetting("8d56d60a-3778-4a95-ae26-ab640f7e5270")?.Value);

        public class TableNamingConventionOptions
        {
            public readonly string Value;

            public TableNamingConventionOptions(string value)
            {
                Value = value;
            }

            public TableNamingConventionOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "pluralized" => TableNamingConventionOptionsEnum.Pluralized,
                    "singularized" => TableNamingConventionOptionsEnum.Singularized,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool IsPluralized()
            {
                return Value == "pluralized";
            }

            public bool IsSingularized()
            {
                return Value == "singularized";
            }
        }

        public enum TableNamingConventionOptionsEnum
        {
            Pluralized,
            Singularized,
        }
        public static DatabaseProviderOptions DatabaseProvider(this DatabaseSettings groupSettings) => new DatabaseProviderOptions(groupSettings.GetSetting("976df8f4-09c5-4186-bb69-a946e6b00b9c")?.Value);

        public class DatabaseProviderOptions
        {
            public readonly string Value;

            public DatabaseProviderOptions(string value)
            {
                Value = value;
            }

            public DatabaseProviderOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "h2" => DatabaseProviderOptionsEnum.H2,
                    "mysql" => DatabaseProviderOptionsEnum.Mysql,
                    "postgresql" => DatabaseProviderOptionsEnum.Postgresql,
                    "sql-server" => DatabaseProviderOptionsEnum.SqlServer,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool IsH2()
            {
                return Value == "h2";
            }

            public bool IsMysql()
            {
                return Value == "mysql";
            }

            public bool IsPostgresql()
            {
                return Value == "postgresql";
            }

            public bool IsSqlServer()
            {
                return Value == "sql-server";
            }
        }

        public enum DatabaseProviderOptionsEnum
        {
            H2,
            Mysql,
            Postgresql,
            SqlServer,
        }
    }
}