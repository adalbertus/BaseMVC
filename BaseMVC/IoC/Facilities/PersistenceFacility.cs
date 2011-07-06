using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Facilities;
using BaseMVC.Infrastructure;
using NHibernate;
using Castle.MicroKernel.Registration;

namespace BaseMVC.IoC.Facilities
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            var config = ConfigurationBuilder.Build();

            Kernel.Register(
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(config.BuildSessionFactory),
                Component.For<ISession>()
                    .UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenSession())
                    .LifeStyle.PerWebRequest);

        }
    }
}