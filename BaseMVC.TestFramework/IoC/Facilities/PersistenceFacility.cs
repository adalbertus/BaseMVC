using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Facilities;
using BaseMVC.Infrastructure;
using NHibernate;
using Castle.MicroKernel.Registration;

namespace BaseMVC.TestFramework.IoC.Facilities
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Component.For<ISession>()
                    .UsingFactoryMethod(k => DatabaseCreator.OpenSession())
                    .LifeStyle.PerThread);

        }
    }
}