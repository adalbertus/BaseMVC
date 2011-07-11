using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using BaseMVC.MSpecTests.IoC.Facilities;

namespace BaseMVC.MSpecTests.IoC.Installers
{
    public class PersistenceInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<PersistenceFacility>();
        }

        #endregion
    }

}
