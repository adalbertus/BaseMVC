using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using BaseMVC.IoC;
using AutoMapper;
using BaseMVC.Domain;
using BaseMVC.ViewModels.Task;

namespace BaseMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;
        //public static IWindsorContainer IoC { get { return _container; } }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.MapRoute(
                "Css",
                "css/{files}",
                new { controller = "css", action = "Merge", files = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "AddNewTaskForProject", // Route name
                "{controller}/{action}/{projectId}", // URL with parameters
                new { controller = "Task", action = "Add", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Project", // Route name
                "{controller}/{action}/{pageNumber}/{orderBy}/{name}", // URL with parameters
                new
                {
                    controller = "Project",
                    action     = "Search",
                    pageNumber = UrlParameter.Optional,
                    orderBy    = UrlParameter.Optional,
                    name       = UrlParameter.Optional
                } // Parameter defaults
            );
        }

        protected void Application_Start()
        {            
#if DEBUG
            Console.SetOut(new CustomDebugWriter());
#endif

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Bootstrap();
        }

        public void Bootstrap()
        {
            ConfigureContainer();
            AutoMapper.AutoMapper.Configure(_container);
        }

        protected void Application_End()
        {
            _container.Dispose();
        }

        private static void ConfigureContainer()
        {
            _container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

    }
}