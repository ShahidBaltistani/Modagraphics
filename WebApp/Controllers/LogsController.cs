using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    //[Role(UserRoles.User)]
    [RoutePrefix("Logs")]
    public class LogsController : BaseController
    {
        public LogsController() : base("Logs") { }
        // GET: Logs
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult SiteStatusList() => View();

        [HttpPost]
        public async Task<ActionResult> GetSiteStatusLog(SitePaginationModel model)
        {
            try
            {
                model.SiteName = model.SiteName ?? "";
                model.PO = model.PO ?? "";
                model.ProjectName = model.ProjectName ?? "";
                model.FleetOwnerName = model.FleetOwnerName ?? "";
                model.SiteStatus = model.SiteStatus;
              
                var result = await GetAsync<SiteModel, int>(model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sitesStatus = site.MainData, TotalRecords = site.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        [HttpGet]
        public async Task<ActionResult> ViewSiteLogDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<SiteStatusLogModel>>(id, "ViewSiteLogDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { logs = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ViewJobLogDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<JobStatusLogModel>>(id, "ViewJobLogDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { JobLogs = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [HttpGet]
        public async Task<ActionResult> ViewJobLogDetailForInstaller(int id)
        {
            try
            {
                var result = await GetAsync<List<JobStatusLogModel>>(id, "ViewJobLogDetailForInstaller");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { JobLogs = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [HttpGet]
        public async Task<ActionResult> JobDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "JobDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { jobDetail = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }



        //job log
        [HttpGet]
        public ActionResult JobStatusList() => View();
        
        [HttpPost]
        public async Task<ActionResult> GetJobDetail(CrewJobPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.JobStatus = model.JobStatus;

                var url = "Logs/JobList";

                var result = await PostAsync<CrewSiteModel, int>(model, url);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as SuccessModel<List<CrewSiteModel>, int>;
                    return Json(new { TotalRecords = crew.OtherData, crews = crew.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}