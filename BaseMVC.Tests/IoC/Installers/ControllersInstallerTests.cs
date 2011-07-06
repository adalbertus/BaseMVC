using System;
using System.Linq;
using System.Web.Mvc;
using BaseMVC.Controllers;
using BaseMVC.IoC.Installers;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using NUnit.Framework;

namespace BaseMVC.Tests.IoC.Installers
{
    [TestFixture]
    public class ControllersInstallerTests
    {
        private IWindsorContainer _containerWithControllers;

        [SetUp]
        public void SetupContext()
        {
            _containerWithControllers = new WindsorContainer()
                .Install(new ControllersInstaller());
        }

        [Test]
        public void All_and_only_controllers_have_Controllers_suffix()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            var registeredControllers = GetImplementationTypesFor(typeof(IController));
            Assert.That(allControllers, Is.EqualTo(registeredControllers));
        }

        [Test]
        public void All_and_only_controllers_live_in_Controllers_namespace()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
            var registeredControllers = GetImplementationTypesFor(typeof(IController));
            Assert.That(allControllers, Is.EqualTo(registeredControllers));
        }

        [Test]
        public void All_controllers_are_registered()
        {
            // Is<TType> is an helper, extension method from Windsor
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            var registeredControllers = GetImplementationTypesFor(typeof(IController));
            Assert.That(allControllers, Is.EqualTo(registeredControllers));
        }

        [Test]
        public void All_controllers_are_transient()
        {
            var nonTransientControllers = GetHandlersFor(typeof(IController))
                .Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();

            Assert.That(nonTransientControllers, Is.Empty);
        }

        [Test]
        public void All_controllers_expose_themselves_as_service()
        {
            var controllersWithWrongName = GetHandlersFor(typeof(IController))
                .Where(controller => controller.Service != controller.ComponentModel.Implementation)
                .ToArray();

            Assert.That(controllersWithWrongName, Is.Empty);
        }

        [Test]
        public void All_controllers_implement_IController()
        {
            var allHandlers = GetAllHandlers();
            var controllerHandlers = GetHandlersFor(typeof(IController));

            Assert.That(allHandlers, Is.Not.Empty);
            Assert.That(allHandlers, Is.EqualTo(controllerHandlers));
        }

        private IHandler[] GetAllHandlers()
        {
            return GetHandlersFor(typeof(object));
        }

        private IHandler[] GetHandlersFor(Type type)
        {
            return _containerWithControllers.Kernel.GetAssignableHandlers(type);
        }

        private Type[] GetImplementationTypesFor(Type type)
        {
            return GetHandlersFor(type)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        private Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
        {
            return typeof(HomeController).Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }
    }
}
