using System;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ExceptionController : Controller
    {
        public ActionResult Index()
        {
            var ex = System.Web.HttpContext.Current.Session["EX"] as Exception;
            System.Web.HttpContext.Current.Session["EX"] = null;
            ViewData["Ex"] = ex;
            return View();
        }
    }
}