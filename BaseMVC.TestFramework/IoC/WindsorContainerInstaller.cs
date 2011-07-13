using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using BaseMVC.IoC.Installers;

namespace BaseMVC.TestFramework.IoC
{
    public static class WindsorContainerInstaller
    {
        public static IWindsorContainer Install()
        {
            var container = new WindsorContainer()
                    .Install(
                        new AutoMapperResolversInstaller(),
                        new IoC.Installers.PersistenceInstaller());

            return container;
        }
    }
}
