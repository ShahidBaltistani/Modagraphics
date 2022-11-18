using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Role(UserRoles.ApprovalManager)]
    public class ApprovalsController : BaseController
    {
        public ApprovalsController() : base("Approvals") { }

        public ActionResult PendingInstallers() => View();
        public ActionResult PendingFleetOwners() => View();
        public ActionResult PendingUsers() => View();
        public ActionResult PendingCorporateUsers() => View();

        #region User
        [HttpPost]
        public async Task<ActionResult> GetUsers(UserPaginationModel model)
        {
            try
            {
                model.Email = model.Email ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Phone = model.Phone ?? "";

                var result = await PostAsync<UserPaginationModel, List<UserModel>, int>("User/Page", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var users = result as SuccessModel<List<UserModel>, int>;
                    return Json(new { Users = users.MainData, TotalRecords = users.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ApproveUser(uint id)
        {
            try
            {
                return await GetAsync("User/Approve", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RejectUser(uint id)
        {
            try
            {
                return await GetAsync("User/Reject", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion

        #region Installer
        [HttpPost]
        public async Task<ActionResult> GetInstallers(InstallerPaginationModel model)
        {
            try
            {
                model.Email = model.Email ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Company = model.Company ?? "";
                
                var result = await PostAsync<InstallerPaginationModel, List<InstallerModel>, int>("Installer/Page", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var installers = result as SuccessModel<List<InstallerModel>, int>;
                    return Json(new { Installers = installers.MainData, TotalRecords = installers.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ApproveInstaller(uint id)
        {
            try
            {
                return await GetAsync("Installer/Approve", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RejectInstaller(uint id)
        {
            try
            {
                return await GetAsync("Installer/Reject", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion

        #region Fleet Owner
        [HttpPost]
        public async Task<ActionResult> GetFleetOwners(FleetOwnerPaginationModel model)
        {
            try
            {
                model.Email = model.Email ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Company = model.Company ?? "";
                model.PhoneNo = model.PhoneNo ?? "";

                var result = await PostAsync<FleetOwnerPaginationModel, List<FleetOwnerModel>, int>("FleetOwner/Page", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var fleetOwners = result as SuccessModel<List<FleetOwnerModel>, int>;
                    return Json(new { FleetOwners = fleetOwners.MainData, TotalRecords = fleetOwners.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ApproveFleetOwner(uint id)
        {
            try
            {
                return await GetAsync("FleetOwner/Approve", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RejectFleetOwner(uint id)
        {
            try
            {
                return await GetAsync("FleetOwner/Reject", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion

        #region Corporate User
        [HttpPost]
        public async Task<ActionResult> GetCorporateUsers(CorporateUserPaginationModel model)
        {
            try
            {
                model.Email = model.Email ?? "";
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.PhoneNumber = model.PhoneNumber ?? "";

                var result = await PostAsync<CorporateUserPaginationModel, List<CorporateUserModel>, int>("CorporateUser/Page", model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var corporateUsers = result as SuccessModel<List<CorporateUserModel>, int>;
                    return Json(new { CorporateUsers = corporateUsers.MainData, TotalRecords = corporateUsers.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ApproveCorporateUser(uint id)
        {
            try
            {
                return await GetAsync("CorporateUser/Approve", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RejectCorporateUser(uint id)
        {
            try
            {
                return await GetAsync("CorporateUser/Reject", id);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion

        #region Approval Manager
        [HttpGet]
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
        [OverrideRole(UserRoles.ApprovalManager)]
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

        [HttpPost]
        [OverrideRole( UserRoles.Supervisor, UserRoles.FleetOwner,UserRoles.ApprovalManager)]
        public async Task<ActionResult> RejectJob(RejectModel model)
        {
            try
            {
                var result = await PostAsync<RejectModel>("RejectJob", model);
                return result;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.Supervisor, UserRoles.FleetOwner)]
        public async Task<ActionResult> ApproveJobWaiting(int id)
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
    }
}