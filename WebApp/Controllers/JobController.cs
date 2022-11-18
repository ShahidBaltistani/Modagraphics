using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;

namespace WebApp.Controllers
{
    [Role(UserRoles.User)]
    public class JobController : BaseController
    {
        public JobController() : base("Job") { }

        [HttpGet]
        [OverrideRole(UserRoles.Crew)]
        public async Task<ActionResult> GetImage(int id)
        {
            try
            {
                var result = await GetAsync<List<JobImagesModel>>(id, "GetImageForJobsInProcess");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ActionResult AddJob(string id)
        {
            try
            {
                var arr = id.Split('$');
                ViewBag.SiteName = arr[1];
                ViewBag.SiteId = arr[2];
                ViewBag.VehicleName = arr[3];
                
                return View(int.Parse(arr[0]));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        public ActionResult EditJob(string id)
        {
            try
            {
                var arr = id.Split('$');
                //ViewBag.SiteId = arr[1];
                ViewBag.SiteName = arr[2];
                ViewBag.VehicleName = arr[3];
                //ViewBag.VehicleId = arr[4];
                return View(int.Parse(arr[0])); // Job Id
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> SaveJob(JobModel model)
        {
            try
            {
                return await PostAsync(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetJobs(JobPaginationModel model)
        {
            try
            {
                model.LPN = model.LPN ?? "";
                model.StatusString = model.StatusString ?? "";
                model.VIN = model.VIN ?? "";
                var result = await GetAsync<JobModel, int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var jobs = result as SuccessModel<List<JobModel>, int>;
                    return Json(new { jobs = jobs.MainData, TotalRecords = jobs.OtherData });

                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetJob(int id)
        {
            try
            {
                var result = await GetAsync<JobModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var job = result as JobModel;
                    return Json(job);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> UpdateJob(JobModel model)
        {
            try
            {
                return await PutAsync(model.Id, model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

       
        public ActionResult Jobs(string id)
        {
            try
            {
                var arr = id.Split('$');
                ViewBag.SiteName = arr[1];
                ViewBag.SiteId = arr[2];
                ViewBag.VehicleName = arr[3];
                ViewBag.IsModificationDisabled = arr[4];
                return View(int.Parse(arr[0]));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //public async Task<ActionResult> CheckVIN(JobModel model)
        //{
        //    try
        //    {
        //        if (model.VIN!=null)
        //        {
        //            var result = await GetAsyncWithParams<bool>("Job", "IsUniqueName", model.VIN.ToString(), model.SiteId.ToString());
        //            if (result is ActionResult actionResult)
        //            {
        //                return actionResult;
        //            }
        //            else
        //            {
        //                var isUnqiue = result as bool?;
        //                return Json(isUnqiue);
        //            }
        //        }
        //        else
        //        {
        //            return Json("null");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex);
        //    }
        //}
        public async Task<ActionResult> CheckVIN(int id,int SiteId, string VIN)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("Job", "IsUniqueName", id.ToString(), SiteId.ToString() ,VIN);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var isUnqiue = result as bool?;
                    return Json(isUnqiue);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
