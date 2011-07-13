using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using BaseMVC.Tests.IoC;
using BaseMVC.TestFramework.IoC;
using BaseMVC.TestFramework;
using NUnit.Framework;

namespace BaseMVC.Tests.Controllers
{
    [TestFixture]
    public abstract class ControllerTestBase
    {
        protected ISession Session { get; private set; }
        protected DatabaseCreator DatabaseCreator { get; private set; }

        public ControllerTestBase()
        {
            var container = WindsorContainerInstaller.Install();
            Session = container.Resolve<ISession>();
            DatabaseCreator = new DatabaseCreator(Session);
            DatabaseCreator.FillDatabase();
            BaseMVC.AutoMapper.AutoMapper.Configure(container);
        }

        [TestFixtureTearDown]
        protected virtual void TearDownContext()
        {
            DatabaseCreator.Close(Session);
        }
    }
}
