using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Models;

namespace WebApp.Controllers
{
    [Role(UserRoles.Installer)]
    public class BidController : BaseController
    {
        public BidController() : base("Bid") { }
        // GET: Bid
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveBid(BidModel bid)
        {
            try
            {
                return await PostAsync(bid);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}