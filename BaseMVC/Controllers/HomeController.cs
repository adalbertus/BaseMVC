using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using NHibernate;

namespace BaseMVC.Controllers
{
    [Authorize]
    public class HomeController : BaseMVCController
    {
        public HomeController(ISession session)
            : base(session)
        {            
        }

        public ActionResult Index()
        {
            Logger.Debug("HomeController::Index");
            ViewBag.Message = "Welcome to ASP.NET MVC!";

#if DEBUG
            //return RedirectToAction("Details", "Project", new { Id = 1 });
            //return RedirectToAction("Search", "Project");
            return View();
#else
            return View();
#endif
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
