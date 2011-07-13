using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Machine.Specifications;
using BaseMVC.Controllers;
using BaseMVC.TestFramework.IoC;
using BaseMVC.TestFramework;

namespace BaseMVC.MSpecTests.Controllers
{
    public abstract class ControllerSpecsBase<TController> where TController : BaseMVCController
    {
        protected static ISession session;
        protected static TController controller;
        protected static DatabaseCreator databaseCreator;

        Establish context =() =>
        {
            var container = WindsorContainerInstaller.Install();
            
            session = container.Resolve<ISession>();
            databaseCreator = new DatabaseCreator(session);
            databaseCreator.FillDatabase();
            BaseMVC.AutoMapper.AutoMapper.Configure(container);
            controller = Activator.CreateInstance(typeof(TController), session) as TController;
        };

        Cleanup establishedContext =() =>
        {
            DatabaseCreator.Close(session);
        };

    }
}
