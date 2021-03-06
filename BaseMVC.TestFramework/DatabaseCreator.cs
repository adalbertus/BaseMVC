﻿using System;
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

        public User JohnSmith {
            get
            {
                if (Session == null)
                {
                    return null;
                }

                return GetUserByLoginName("john.smith");
            }
        }

        public User MarkTwain
        {
            get
            {
                if (Session == null)
                {
                    return null;
                }

                return GetUserByLoginName("mark.twain");
            }
        }

        public Project SampleProject
        {
            get
            {
                if (Session == null)
                {
                    return null;
                }
                return Session.QueryOver<Project>()
                    .Where(x => x.Name == "Sample project")
                    .SingleOrDefault();
            }
        }

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
            FillDatabaseWithProjects();
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

                Session.Save(new User
                {
                    FirstName = "Mark",
                    LastName = "Twain",
                    LoginName = "mark.twain",
                    Password = "niawt",
                    Description = "Writer",
                });

                tx.Commit();
                Session.Flush();
            }
        }

        public void FillDatabaseWithProjects()
        {
            var project = Project.CreateProject("Sample project", DateTime.Now.AddMonths(-6), null, JohnSmith, new[] { JohnSmith });
            using (var tx = Session.BeginTransaction())
            {
                Session.Save(project);
                tx.Commit();                
                Session.Flush();
            }
        }

        public void ClearProjectsFromDatabase()
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.Delete(SampleProject);
                tx.Commit();
                Session.Flush();
            }
        }

        private static void BuildSchema(Configuration configuration, ISession session)
        {
            SchemaExport export = new SchemaExport(configuration);
            export.Execute(false, true, false, session.Connection, null);
        }

        private User GetUserByLoginName(string loginName)
        {
            return Session.QueryOver<User>()
                .Where(x => x.LoginName == loginName)
                .SingleOrDefault();
        }
    }
}
