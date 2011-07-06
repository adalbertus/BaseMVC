using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using AutoMapper;
using BaseMVC.AutoMapper;
using Castle.MicroKernel.SubSystems.Configuration;

namespace BaseMVC.IoC.Installers
{
    public class AutoMapperTypeConvertersInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register((AllTypes.FromThisAssembly()
            //        .Where(Component.IsInSameNamespaceAs<UtcToLocalTimeConverter>())
            //        .If(t => t.Name.Contains("Converter"))
            //        .Configure((c => c.LifeStyle.Transient))));
        }

        #endregion
    }
}