using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController() : base("SomeController")
        {

        }
        public ActionResult Index()
        {
            //WebRequest request = WebRequest.CreateHttp("https://app.powerbi.com/view?r=eyJrIjoiODI0MzZiNDMtNjU4Zi00ZjZiLWE5ZDgtNjFhZjEwOGQ1MmRkIiwidCI6IjU1MzZhYzk0LTU1ODAtNDI3My1iODA0LWVjOWQ4MzNlMTQzNCIsImMiOjl9");
            //request.Method = "Get";
            //using (var response = request.GetResponse())
            //{
            //    using (var stream = response.GetResponseStream())
            //    {
            //        using (var reader = new StreamReader(stream))
            //        {
            //            var r = reader.ReadToEnd();
            //            var result = new HtmlResult(Encoding.UTF8.GetBytes(r));
            //            return result;
            //        }
            //    }
            //}
            //return Redirect("https://app.powerbi.com/view?r=eyJrIjoiODI0MzZiNDMtNjU4Zi00ZjZiLWE5ZDgtNjFhZjEwOGQ1MmRkIiwidCI6IjU1MzZhYzk0LTU1ODAtNDI3My1iODA0LWVjOWQ4MzNlMTQzNCIsImMiOjl9");
            return View();
        }

        public ActionResult GetReport()
        {
            return Redirect("https://app.powerbi.com/view?r=eyJrIjoiODI0MzZiNDMtNjU4Zi00ZjZiLWE5ZDgtNjFhZjEwOGQ1MmRkIiwidCI6IjU1MzZhYzk0LTU1ODAtNDI3My1iODA0LWVjOWQ4MzNlMTQzNCIsImMiOjl9");
        }
    }



    public class HtmlResult : ActionResult
    {
        private byte[] content { get; }
        public HtmlResult(byte[] vs)
        {
            content = vs;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.BinaryWrite(content);
        }
    }
}