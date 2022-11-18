using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Welcome()
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + "Welcome.html";
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "text/html");
        }
    }
}
