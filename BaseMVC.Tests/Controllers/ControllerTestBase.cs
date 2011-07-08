using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using BaseMVC.Tests.IoC;
using BaseMVC.Tests.Infrastructure;

namespace BaseMVC.Tests.Controllers
{
    public abstract class ControllerTestBase
    {
        protected ISession Session { get; private set; }

        public ControllerTestBase()
        {
            var container = WindsorContainerInstaller.Install();
            Session = container.Resolve<ISession>();
            DatabaseFactory.FillDatabase(Session);
            BaseMVC.AutoMapper.AutoMapper.Configure(container);
        }
    }
}
