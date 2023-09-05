using System;
using Intent.Engine;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.SpringBoot.Settings;

namespace Intent.Modules.Java.Persistence.JPA;

public static class JavaDependencies
{
    public static JavaDependency Lombok = new("org.projectlombok", "lombok", optional: true);
    
    public static JavaDependency SpringBootStarterDataJpa(IApplication application)
    {
        return new JavaDependency("org.springframework.boot", "spring-boot-starter-data-jpa");
    }
    
    public static JavaDependency SpringBootStarterJdbc(IApplication application)
    {
        return new JavaDependency("org.springframework.boot", "spring-boot-starter-jdbc");
    }

    public static JavaDependency HibernateTypes(IApplication application)
    {
        return application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
        {
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 =>
                new JavaDependency("com.vladmihalcea", "hibernate-types-55", "2.20.0"),
            SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 =>
                new JavaDependency("com.vladmihalcea", "hibernate-types-60", "2.21.1"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static JavaDependency H2Database(IApplication application)
    {
        return new JavaDependency("com.h2database", "h2");
    }
    
    public static JavaDependency MysqlConnector(IApplication application)
    {
        return new JavaDependency("com.mysql", "mysql-connector-j");
    }
    
    public static JavaDependency PostgreSql(IApplication application)
    {
        return new JavaDependency("org.postgresql", "postgresql");
    }
    
    public static JavaDependency MssqlJdbc(IApplication application)
    {
        return new JavaDependency("com.microsoft.sqlserver", "mssql-jdbc");
    }
}