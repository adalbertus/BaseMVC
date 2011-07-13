using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using BaseMVC.Infrastructure;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;
using BaseMVC.Domain;

namespace BaseMVC.TestFramework
{
    public class DatabaseCreator
    {
        public ISession Session { get; private set; }
        public DatabaseCreator(ISession session)
        {
            if (session == null)
            {
                Session = OpenSession();
            }
            else
            {
                Session = session;
            }
        }

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

        public void FillDatabase()
        {
            FillDatabaseWithUsers();
        }

        public void FillDatabaseWithUsers()
        {
            using (var tx = Session.BeginTransaction())
            {                
                Session.Save(new User
                {
                    FirstName = "John",
                    LastName = "Smith",
                    LoginName = "john.smith",
                    Password = "secret",
                    Description = "John the Smith",
                });

                tx.Commit();
                Session.Flush();
            }
        }

        private static void BuildSchema(Configuration configuration, ISession session)
        {
            SchemaExport export = new SchemaExport(configuration);
            export.Execute(false, true, false, session.Connection, null);
        }
    }
}
