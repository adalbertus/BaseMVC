using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using BaseMVC.Infrastructure;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;
using BaseMVC.Domain;

namespace BaseMVC.MSpecTests.Infrastructure
{
    public static class DatabaseFactory
    {
        public static ISession OpenSession()
        {
            var configuration = ConfigurationBuilder.Build(false, true);
            var sessionFactory = configuration.BuildSessionFactory();
            var session = sessionFactory.OpenSession();
            BuildSchema(configuration, session);
            return session;
        }

        public static void Close(ISession session)
        {
            if (session.IsOpen)
            {
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                }
                session.Close();
            }
        }

        public static void FillDatabase(ISession session)
        {
            using (var tx = session.BeginTransaction())
            {
                session.Save(new User
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    LoginName = "jan.kowaslki",
                    Password = "haslo",
                    Description = "Jan Kowalski",
                });

                tx.Commit();
                session.Flush();
            }
        }

        private static void BuildSchema(Configuration configuration, ISession session)
        {
            SchemaExport export = new SchemaExport(configuration);
            export.Execute(false, true, false, session.Connection, null);
        }
    }
}
