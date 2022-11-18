using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class EmailCheckController : BaseController
    {
        public EmailCheckController() : base("") { }

        [HttpGet]
        public async Task<ActionResult> CorporateUserEmailCheck(int id, string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("CorporateUser", "IsUniqueEmail",id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> FleetOwnerEmailCheck(int id, string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("FleetOwner", "IsUniqueEmail", id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> SupervisorEmailCheck(int id, string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("Supervisor", "IsUniqueEmail", id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        public async Task<ActionResult> UserEmailCheck(int id, string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("User", "IsUniqueEmail", id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> ApprovalManagerEmailCheck(int id, string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("approvalManager", "IsUniqueEmail", id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> FinanceManagerEmailCheck(int id, string eamil)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("FinanceManager", "IsUniqueEmail", id.ToString(), eamil);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> InstallerEmailCheck(int id ,string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("Installer", "IsUniqueEmail", id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CrewEmailCheck(int id, string email)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("Crew", "IsUniqueEmail", id.ToString(), email);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var IsUniqueEmail = result as bool?;
                    return Json(IsUniqueEmail);
                }

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

    }
}
