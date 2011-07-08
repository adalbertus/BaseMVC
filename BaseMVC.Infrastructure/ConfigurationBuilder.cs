using System;
using BaseMVC.Domain;
using Castle.Core.Internal;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using BaseMVC.Infrastructure.Mappings;
using System.IO;

namespace BaseMVC.Infrastructure
{
    public class ConfigurationBuilder
    {
        public static Configuration Build(bool isVerboseEnabled = false, bool inMemoryDatabase = false)
        {
            return Fluently.Configure()
                .ProxyFactoryFactory(typeof(ProxyFactoryFactory))
                .Database(SetupDatabase(isVerboseEnabled, inMemoryDatabase))
                .Mappings(m => m.AutoMappings.Add(CreateMappingModel()))
                .ExposeConfiguration(c => ConfigurePersistence(c, isVerboseEnabled, inMemoryDatabase))
                .BuildConfiguration();
        }

        private static AutoPersistenceModel CreateMappingModel()
        {
            var m = AutoMap.Assembly(typeof(Entity).Assembly)
                .Where(IsDomainEntity)
                .OverrideAll(ShouldIgnoreProperty)
                .IgnoreBase<Entity>()
                .UseOverridesFromAssemblyOf<TaskMappingOverride>()
                .Conventions.Add(new CustomForeignKeyConvention());

            return m;
        }

        private static IPersistenceConfigurer SetupDatabase(bool isVerboseEnabled, bool inMemoryDatabase)
        {
            if (inMemoryDatabase)
            {
                var sqlite = SQLiteConfiguration.Standard.InMemory();
                if (isVerboseEnabled)
                {
                    sqlite.ShowSql();
                }
                return sqlite;
            }

            var mssql = MsSqlConfiguration.MsSql2008
                            .UseOuterJoin()
                            .ConnectionString(x => x.FromConnectionStringWithKey("Default"));
            if (isVerboseEnabled)
            {
                mssql.ShowSql();
            }
            return mssql;
        }

        private static void ConfigurePersistence(Configuration config, bool isVerboseEnabled, bool inMemoryDatabase)
        {
            if (isVerboseEnabled)
            {
                config.DataBaseIntegration(x =>
                    {
                        x.LogFormatedSql  = true;
                        x.LogSqlInConsole = true;
                    }
                    );
            }
            if (!inMemoryDatabase)
            {
                new SchemaUpdate(config).Execute(false, true);
                //SchemaMetadataUpdater.QuoteTableAndColumns(config);
            }
        }

        private static bool IsDomainEntity(Type t)
        {
            return typeof(Entity).IsAssignableFrom(t);
        }

        private static void ShouldIgnoreProperty(IPropertyIgnorer property)
        {
            property.IgnoreProperties(p => p.MemberInfo.HasAttribute<DoNotMapAttribute>());
        }
    }
}
