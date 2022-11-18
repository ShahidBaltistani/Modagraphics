using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController() : base("") { }

        [Role(UserRoles.User)]
        public ActionResult Admin() => View();

        [Role(UserRoles.Installer)]
        public ActionResult Installer() => View();

        [Role(UserRoles.FleetOwner)]
        public ActionResult FleetOwner() => View();

        [Role(UserRoles.Crew)]
        public ActionResult Crew() => View();
       
        [Role(UserRoles.ApprovalManager)]
        public ActionResult ApprovalManager() => View();

        [Role(UserRoles.Supervisor)]
        public ActionResult Supervisor() => View();

        [Role(UserRoles.CorporateUser)]
        public ActionResult CorporateUser() => View();
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string old, string @new)
        {
            try
            {
                return await PostAsync("UserLogin", "ChangePassword", $"{old}$--${@new}");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}