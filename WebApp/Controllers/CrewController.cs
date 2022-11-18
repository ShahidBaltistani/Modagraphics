using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    //[Role(UserRoles.Installer, UserRoles.User)]
    public class CrewController : BaseController
    {
        public CrewController() : base("Crew") { }

        [HttpGet]
        public ActionResult AddCrew() => View();

        [HttpGet]
        public ActionResult EditCrew(int id) => View(id);

        [HttpGet]
        public ActionResult CrewList() => View();
        
        [HttpGet]
        [OverrideRole(UserRoles.Crew)]
        public ActionResult AssignedSites() => View();


        [HttpGet]
        [OverrideRole(UserRoles.Crew)]
        public ActionResult JobsInQue(int id) => View(id);

        [HttpPost]
        [OverrideRole(UserRoles.Crew)]
        public async Task<ActionResult> JobsInQue(CrewJobPaginationModel model)
        {
            try
            {
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";

                var url = "crew/CrewJobsInQue";

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
        [HttpPost]
        public async Task<ActionResult> saveJobImage(JobImagesModel jobimages)
        {
            try
            {
                return await PostAsync<JobImagesModel>("crewJobImage", jobimages);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        public ActionResult JobsInProcess()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> JobsInProcess(CrewJobPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";

                model.UnitNo = model.UnitNo;
                model.StartedDate = model.StartedDate ?? null;


                var url = "crew/CrewJobsInProcess";

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
        [HttpGet]
        public ActionResult JobsUnderReview() => View();
        [HttpPost]
        public async Task<ActionResult> JobsUnderReview(CrewJobReviewPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.UnitNo = model.UnitNo;
                model.StartedDate = model.StartedDate ?? null;
                model.CompletedDate = model.CompletedDate ?? null;

                var url = "crew/JobsInReview";

                var result = await PostAsync<CrewJobSiteStatusModel, int>(model, url);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as SuccessModel<List<CrewJobSiteStatusModel>, int>;
                    return Json(new { TotalRecords = crew.OtherData, crews = crew.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public ActionResult CompletedJobs()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompletedJobs(CrewCompletedJobsPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.UnitNo = model.UnitNo;

                var url = "crew/JobsCompleted";

                var result = await PostAsync<CrewJobCompletedStatusModel, int>(model, url);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as SuccessModel<List<CrewJobCompletedStatusModel>, int>;
                    return Json(new { TotalRecords = crew.OtherData, crews = crew.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult> SaveCrew(CrewModel model)
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
        public async Task<ActionResult> CrewList(CrewPaginationModel model)
        {
            try
            {


                model.CrewUserName = model.CrewUserName ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Email = model.Email ?? "";
                model.PhoneNo = model.PhoneNo ?? "";

                var result = await GetAsync<CrewModel, int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as SuccessModel<List<CrewModel>, int>;
                    return Json(new { TotalRecords = crew.OtherData, crews = crew.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        [OverrideRole(UserRoles.Crew)]
        public async Task<ActionResult> GetAssignedSites(CrewPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.AssignedDate = model.AssignedDate ?? null;
                // Code End


                var result = await PostAsync<CrewPaginationModel, List<BidModel>, int>("AssignedSitesList", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var AssignedSite = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { AssignedSite = AssignedSite.MainData, TotalRecords = AssignedSite.OtherData });
                }
            }
            catch (Exception ex) { return Error(ex); }
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(int id)
        {
            try
            {
                var result = await GetAsync<string>(id, "Image");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var imageString = result as string;
                    if (imageString == "")
                    {
                        return Json("null");
                    }
                    var imgUrlData = imageString is null ? "null" : $"data:image/jpg;base64,{imageString}";
                    return Json(imgUrlData);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCrew(int id)
        {
            try
            {
                var result = await GetAsync<CrewModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as CrewModel;
                    crew.Picture = crew.Picture is null ? "null" : $"data:image/jpg;base64,{crew.Picture}";
                    return Json(crew);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCrew(CrewModel model)
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

        [HttpPost]
        public async Task<ActionResult> ManageLogin(LoginModel model)
        {
            try
            {
                model.UserRole = UserRoles.Crew;
                model.IsActive = true;

                if(model.Id == 0)
                {
                    return await PostAsync("UserLogin", "", model);
                }
                else
                {
                    return await PutAsync(model.Id, "UserLogin", "", model);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetLoginInfo(int id)
        {
            try
            {
                return Json(await GetAsync<LoginModel>(id,  "GetByAssociatedId"));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.Crew,UserRoles.User, UserRoles.Supervisor, UserRoles.FleetOwner)]
        public async Task<ActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var result = await GetAsync<List<SiteVehicleTypeModel>>(Id, "GetVehiclesAgainstAssignedSites");
                return Json(new { VehiclesAgainstAssignedSites = result as List<SiteVehicleTypeModel> });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}