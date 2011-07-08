using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using NHibernate;

namespace BaseMVC.Controllers
{
    public class BaseMVCController : Controller
    {
        public ILogger Logger { get; set; }

        public HttpSessionStateBase HttpSession
        {
            get { return base.Session; }
        }

        public new ISession Session { get; private set; }

        public bool IsAjaxRequest { get; set; }

        public BaseMVCController(ISession session)
        {
            Session = session;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            bool isXRequestWithNotEmpty = filterContext.HttpContext.Request.Headers["X-Requested-With"] != null;
            bool isXMLHttpRequest = filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            IsAjaxRequest = isXRequestWithNotEmpty && isXMLHttpRequest;
        }
    }
}