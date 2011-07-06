using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using BaseMVC.Infrastructure.Repositories;

namespace BaseMVC.IoC.Installers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining<IProjectRepository>()
                                .Where(Component.IsInSameNamespaceAs<IProjectRepository>())
                                .WithService.DefaultInterface()
                                .Configure(c => c.LifeStyle.Transient
                                                 .DependsOn(new { pageSize = 5 })));
        }
    }

}