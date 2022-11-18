using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Antlr.Runtime;
using Models;

namespace WebApp.Controllers
{
    public class SupervisorController : BaseController
    {
        public SupervisorController() : base("Supervisor") { }


        [HttpGet]
        public ActionResult AddSupervisor() => View();

        [HttpGet]
        public ActionResult SupervisorEdit(int id) => View(id);

        [HttpGet]
        public ActionResult SupervisorList() => View();

        [HttpGet]
        public ActionResult PendingInvoices() => View();

        [HttpGet]
        [OverrideRole(UserRoles.Supervisor)]
        public ActionResult IncidentReports() => View();

        [HttpGet]
        public ActionResult CompletedJobs() => View();

        [HttpGet]
        [OverrideRole(UserRoles.Supervisor)]
        public ActionResult VehiclesToRepair() => View();

        [HttpGet]
        [OverrideRole(UserRoles.Supervisor)]
        public ActionResult AssignedSites() => View();
        [HttpPost]
        public async Task<ActionResult> SaveSupervisor(SupervisorModel model)
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


        [HttpGet]
        [OverrideRole(UserRoles.Supervisor)]
        public async Task<ActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var result = await GetAsync<List<SiteVehicleTypeModel>>(Id, "GetVehiclesAgainstAssignedSites");
                return Json(new { VehiclesAgainstAssignedSites=result as List<SiteVehicleTypeModel> });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




            [HttpPost]
        [OverrideRole(UserRoles.Supervisor)]
        public async Task<ActionResult> AssignedSites(SupervisorPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.AssignDate = model.AssignDate ?? null;
                // Code End


                var result = await PostAsync<SupervisorPaginationModel, List<IncidentReportModel>,int>("AssignedSites", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var AssignedSites = result as SuccessModel<List<IncidentReportModel>, int>;
                    return Json(new { TotalRecords = AssignedSites.OtherData, AssignedSites = AssignedSites.MainData });
                }
            }
            catch (Exception ex)
            {

                return Error(ex);
            }
        }

        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner, UserRoles.Supervisor)]
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
        [OverrideRole(UserRoles.FleetOwner, UserRoles.Supervisor)]
        public async Task<ActionResult> ApproveJob(int id)
        {
            try
            {
                var result = await PostAsync<int>("ApproveJob",id);
                return result;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner, UserRoles.Supervisor)]
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

        [HttpPost]
        public async Task<ActionResult> SupervisorList(SupervisorPaginationModel model)
        {
            try
            {
                model.SupervisorUserName = model.SupervisorUserName ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Email = model.Email ?? "";
                model.PhoneNumber = model.PhoneNumber ?? "";
                var result = await GetAsync<SupervisorModel, int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var supervisor = result as SuccessModel<List<SupervisorModel>, int>;
                    return Json(new { TotalRecords = supervisor.OtherData, supervisors = supervisor.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        [OverrideRole(UserRoles.FleetOwner, UserRoles.Supervisor)]
        public async Task<ActionResult> GetVehiclesToRepair(SupervisorPaginationModel model) {
            try
            {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.Unit = model.Unit;
                // Code End


                var result = await PostAsync<SupervisorPaginationModel, List<IncidentReportModel>,int>("VehiclesToRepair", model);
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
        [OverrideRole(UserRoles.FleetOwner, UserRoles.Supervisor)]
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

        [HttpGet]
        [OverrideRole(UserRoles.FleetOwner,UserRoles.Supervisor)]
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
        [OverrideRole(UserRoles.FleetOwner, UserRoles.Supervisor)]
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


        [HttpGet]
        public async Task<ActionResult> GetSupervisor(int id)
        {
            try
            {
                var result = await GetAsync<SupervisorModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var supervisor = result as SupervisorModel;
                    supervisor.Picture = supervisor.Picture is null ? "null" : $"data:image/jpg;base64,{supervisor.Picture}";
                    return Json(supervisor);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSupervisor(SupervisorModel model)
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

        [HttpPost]
        public async Task<ActionResult> ManageLogin(LoginModel model)
        {
            try
            {
                model.UserRole = UserRoles.Supervisor;
                model.IsActive = true;

                if (model.Id == 0)
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


        #region Approve Jobs
        [HttpGet]
        [OverrideRole(UserRoles.Supervisor)]
        public ActionResult JobsForApproval() => View();

        [HttpPost]
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
        [OverrideRole(UserRoles.Supervisor)]
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

        [HttpGet]
        public async Task<ActionResult> SiteDetails(int id)
        {
            try
            {
                var result = await GetAsync<List<SiteModel>>(id, "SiteDetails");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { SiteDetail = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        //Log
        public ActionResult SiteLog() => View();

        [HttpPost]
        public async Task<ActionResult> SiteListLog(SitePaginationModel model)
        {
            try
            {
                model.SiteName = model.SiteName ?? "";
                model.PO = model.PO ?? "";
                model.ProjectName = model.ProjectName ?? "";
                model.SiteStatus = model.SiteStatus;
                //model.FleetOwnerName = model.FleetOwnerName ?? "";
                var url = "Supervisor/SiteListLog";

                var result = await PostAsync<SiteModel, int>(model, url);

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

        //Job Log
        public ActionResult JobList() => View();

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

                var url = "Supervisor/JobList";

                var result = await PostAsync<CrewSiteModel, int>(model, url);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as SuccessModel<List<CrewSiteModel>, int>;
                    return Json(new { crews = crew.MainData, TotalRecords = crew.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

    }
}