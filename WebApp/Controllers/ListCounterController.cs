using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [RoutePrefix("ListCounter")]
    public class ListCounterController : BaseController
    {
        public ListCounterController() : base("ListCounter") { }

        [HttpGet]
        public async Task<ActionResult> getdata()
        {
            try
            {
                var result = await GetAsync<List<ProjectModel>>("Get");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<ProjectModel>;
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAdminDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetAdminDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetInstallerDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetInstallerDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        
        [HttpGet]
        public async Task<ActionResult> GetFleetownerDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetFleetownerDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetSupervisorDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetSupervisorDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetApprovalManagerDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetApprovalManagerDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCrewDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetCrewDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCorporateUserDashboardData()
        {
            try
            {
                var result = await GetAsync<DashboardModel>("GetCorporateUserDashboardData");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var data = result as List<DashboardModel>;
                    var obj = data[0];
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}