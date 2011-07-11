using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using BaseMVC.Tests.IoC;
using BaseMVC.Tests.Infrastructure;
using Machine.Specifications;
using BaseMVC.Controllers;

namespace BaseMVC.Tests.Controllers
{
    public abstract class ControllerSpecsBase<TController> where TController : BaseMVCController
    {
        protected static ISession session;
        protected static TController controller;

        Establish context =() =>
        {
            var container = WindsorContainerInstaller.Install();
            session = container.Resolve<ISession>();
            DatabaseFactory.FillDatabase(session);
            BaseMVC.AutoMapper.AutoMapper.Configure(container);
            controller = Activator.CreateInstance(typeof(TController), session) as TController;
        };

        Cleanup establishedContext =() =>
        {
            DatabaseFactory.Close(session);
        };

    }
}
