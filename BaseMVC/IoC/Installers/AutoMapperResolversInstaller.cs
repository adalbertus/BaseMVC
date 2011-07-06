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
    public class AutoMapperResolversInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register((AllTypes.FromThisAssembly()
                    .BasedOn<IValueResolver>()
                    .If(Component.IsInSameNamespaceAs<AvaiableProductsResolver>())
                    .If(t => t.Name.Contains("Resolver"))
                    .Configure((c => c.LifeStyle.Transient))));

        }

        #endregion
    }
}