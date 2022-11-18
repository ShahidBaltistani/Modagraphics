using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;

namespace WebApp.Controllers
{
    [Role(UserRoles.User,UserRoles.FleetOwner)]
    public class FleetOwnerController : BaseController
    {
        public FleetOwnerController(): base("FleetOwner") { }

        [HttpGet]
        public ActionResult AddFleetOwner() => View();

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
        public ActionResult AwardedSites() => View();

        [HttpGet]
        public ActionResult FleetOwnerEdit(int id) => View(id);

        [HttpGet]
        public ActionResult FleetOwnerList() => View();

        [HttpPost]
        public async Task<ActionResult> SaveFleetOwner(FleetOwnerModel model)
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
        public async Task<ActionResult> FleetOwnerList(FleetOwnerPaginationModel model)
        {
            try
            {
                model.CorporateUserName = model.CorporateUserName ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Company = model.Company ?? "";
                model.Email = model.Email ?? "";
                model.PhoneNo = model.PhoneNo ?? "";
                var result = await GetAsync<FleetOwnerModel, int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var fleetOwner = result as SuccessModel<List<FleetOwnerModel>,int>;
                    return Json(new { TotalRecords = fleetOwner.OtherData, fleetOwners = fleetOwner.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //[HttpPost]
        //[OverrideRole(UserRoles.FleetOwner)]
        //public async Task<ActionResult> GetSupervisorList(SupervisorPaginationModel model)
        //{
        //    try
        //    {
        //        model.SupervisorUserName = model.SupervisorUserName ?? "";
        //        model.FirstName = model.FirstName ?? "";
        //        model.LastName = model.LastName ?? "";
        //        model.Email = model.Email ?? "";
        //        model.PhoneNumber = model.PhoneNumber ?? "";
        //        var result = await GetAsync<SupervisorModel, int>(model);
        //        if (result is ActionResult actionResult)
        //        {
        //            return actionResult;
        //        }
        //        else
        //        {
        //            var supervisor = result as SuccessModel<List<SupervisorModel>, int>;
        //            return Json(new { TotalRecords = supervisor.OtherData, supervisor = supervisor.MainData });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex);
        //    }
        //}

        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> GetAwardedSites(SupervisorPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                // Code End


                var result = await PostAsync<SupervisorPaginationModel, List<BidModel>, int>("AwardedSites", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var AwardedSite = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { AwardedSites = AwardedSite.MainData, TotalRecords = AwardedSite.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }

        }

        [HttpGet]
        [OverrideRole(UserRoles.CorporateUser, UserRoles.User)]
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
        public async Task<ActionResult> GetFleetOwner(int id)
        {
            try
            {
                var result = await GetAsync<FleetOwnerModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var fleetOwner = result as FleetOwnerModel;
                    fleetOwner.Picture = fleetOwner.Picture is null ? "null" : $"data:image/jpg;base64,{fleetOwner.Picture}";
                    return Json(fleetOwner);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFleetOwner(FleetOwnerModel model)
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
        
        
        //Supervisor
        [OverrideRole(UserRoles.FleetOwner)]
        public ActionResult AddSupervisor()
        {
            return View();
        }

        [OverrideRole(UserRoles.FleetOwner)]
        [HttpPost]
        public async Task<ActionResult> AssignSite(ChildEntries obj)
        {
            try
            {
                return await PostAsync<ChildEntries>("AssignSite", obj);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        public ActionResult SupervisorList()
        {
            return View();
        }

        public ActionResult PendingInvoices()
        {
            return View();
        }

        [OverrideRole(UserRoles.FleetOwner)]
        public ActionResult IncidentReports()
        {
            return View();
        }


        [OverrideRole(UserRoles.FleetOwner)]
        public ActionResult VehiclesToRepair()
        {
            return View();
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
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
        #region Approve Jobs
        [OverrideRole(UserRoles.FleetOwner)]
        public ActionResult JobsForApproval()
        {
            return View();
        }
        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> GetJobsWaitingForApproval(SupervisorPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.AssignDate = model.AssignDate ?? null;
                // Code End


                var result = await PostAsync<SupervisorPaginationModel, List<JobModel>, int>("GetJobsWaitingForApproval", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var jobs = result as SuccessModel<List<JobModel>, int>;
                    return Json(new { TotalRecords = jobs.OtherData, Jobs = jobs.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> GetApprovalImages(int id)
        {
            try
            {
                //var result = await GetAsync<List<string>>(id, "GetApprovalImages");
                var result = await GetAsync<List<JobImagesModel>>(id, "GetApprovalImages");

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    //var data = result as SuccessModel<List<string>>;
                    //for (int i = 0; i < data.MainData.Count; i++)
                    //{
                    //    var str = data.MainData[i];
                    //    data.MainData[i] = str is null || str == "" ? "null" : $"data:image/jpg;base64,{str}";
                    //}
                    var data = result as List<JobImagesModel>;
                    for (int i = 0; i < data.Count; i++)
                    {
                        var str = data[i].Picture;
                        data[i].Picture = str is null || str == "" ? "null" : $"{str}";
                    }
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.ApprovalManager)]
        public async Task<ActionResult> ApproveJobWaitng(int id)
        {
            try
            {
                var result = await PostAsync<int>("ApproveJobWaiting", id);
                return result;
            }
            catch (Exception ex)
            {

                return Error(ex);
            }
        }
        #endregion
        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> GetVehiclesToRepair(SupervisorPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.Unit = model.Unit;
                // Code End



                var result = await PostAsync<SupervisorPaginationModel, List<IncidentReportModel>, int>("VehiclesToRepair", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var vehicalsToReport = result as SuccessModel<List<IncidentReportModel>, int>;
                    return Json(new { TotalRecords = vehicalsToReport.OtherData, vehicalsToReport = vehicalsToReport.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> Repaired(int id)
        {
            try
            {
                var result = await PostAsync<int>("Repaired", id);
                return result;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> IncidentReportList(SupervisorPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.Unit = model.Unit;
                // Code End




                var result = await PostAsync<SupervisorPaginationModel, List<IncidentReportModel>, int>("IncidentReports", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var incidentReports = result as SuccessModel<List<IncidentReportModel>, int>;
                    return Json(new { TotalRecords = incidentReports.OtherData, incidentReports = incidentReports.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> ApproveJob(int id)
        {
            try
            {
                var result = await PostAsync<int>("ApproveJob", id);
                return result;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> SentForRepair(int id)
        {
            try
            {
                var result = await PostAsync<int>("SentForRepair", id);
                return result;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> GetIncidentImage(int id)
        {
            try
            {
                var result = await GetAsync<string>(id, "IncidentImage");
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
                    var imgUrlData = imageString is null ? "null" : $"{imageString}";
                    return Json(imgUrlData);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ActionResult SitesByFleetId() => View();
        [HttpPost]
        public async Task<ActionResult> SitesFleetOwner(SitePaginationModel model)
        {
            try
            {
                model.SiteName = model.SiteName ?? "";
                model.PO = model.PO ?? "";
                model.ProjectName = model.ProjectName ?? "";
                model.FleetOwnerName = model.FleetOwnerName ?? "";

                //var result = await GetAsync<SiteModel, int>(model);
                 var result = await PostAsync<SitePaginationModel, List<SiteModel>, int>("SitesByFleetId", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sites = site.MainData, TotalRecords = site.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        public ActionResult JobsByFleetId() => View();
        [HttpPost]
        public async Task<ActionResult> JobsFleetOwner(CrewJobPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.JobStatus = model.JobStatus;

                var url = "FleetOwner/JobsByFleetId";

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
        [OverrideRole(UserRoles.FleetOwner)]
        public ActionResult SupervisorAwardedSites() => View();

        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner)]
        public async Task<ActionResult> GetSupervisorAwardedSites(SupervisorPaginationModel model)
        {
            try
            {

                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                


                var result = await PostAsync<SupervisorPaginationModel, List<BidModel>, int>("SupervisorAwardedSites", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var AwardedSite = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { AwardedSites = AwardedSite.MainData, TotalRecords = AwardedSite.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }

        }
    }
}