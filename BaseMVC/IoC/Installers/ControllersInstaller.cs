using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Web.Mvc;
using BaseMVC.Controllers;

namespace BaseMVC.IoC.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                .BasedOn<IController>()
                                .If(Component.IsInSameNamespaceAs<HomeController>())
                                .If(t => t.Name.EndsWith("Controller"))
                                .Configure((c => c.LifeStyle.Transient)));
        }

        #endregion
    }
}
