using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Facilities;
using BaseMVC.Infrastructure;
using NHibernate;
using Castle.MicroKernel.Registration;
using BaseMVC.Tests.Infrastructure;

namespace BaseMVC.Tests.IoC.Facilities
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            //var session = DatabaseFactory.OpenSession();
            Kernel.Register(
                Component.For<ISession>()
                    .UsingFactoryMethod(k => DatabaseFactory.OpenSession())
                    .LifeStyle.PerThread);

        }
    }
}