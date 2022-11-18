using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : base("Home") { }
        // GET: Home
        public ActionResult FAQ()
        {
            return View();
        }
    }
}