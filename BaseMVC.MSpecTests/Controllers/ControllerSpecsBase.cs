using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Machine.Specifications;
using BaseMVC.Controllers;
using BaseMVC.TestFramework.IoC;
using BaseMVC.TestFramework;
using System.Web.Mvc;

namespace BaseMVC.MSpecTests.Controllers
{
    public abstract class ControllerSpecsBase<TController> where TController : BaseMVCController
    {
        protected static ISession _session;
        protected static TController _controller;
        protected static DatabaseCreator _databaseCreator;
        protected static ActionResult _actionResult;
        protected static ViewResult ViewResult { get { return _actionResult as ViewResult; } }
        
        Establish context =() =>
        {
            var container = WindsorContainerInstaller.Install();
            
            _session = container.Resolve<ISession>();
            _databaseCreator = new DatabaseCreator(_session);
            _databaseCreator.FillDatabase();
            BaseMVC.AutoMapper.AutoMapper.Configure(container);
            _controller = Activator.CreateInstance(typeof(TController), _session) as TController;
        };

        Cleanup establishedContext =() =>
        {
            DatabaseCreator.Close(_session);
        };

    }
}
