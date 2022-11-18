using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class ApprovalsController : BaseController
    {
        public ApprovalsController(IHttpContextAccessor httpContext) : base(httpContext) { }

        #region User
        [HttpPost("User/Page")]
        public async Task<IActionResult> PendingUsersPage(UserPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<User, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.Phone == "" || x.PhoneNo.ToLower().Contains(paginationModel.Phone.ToLower()))
                    && !x.IsDeleted && x.Status == UserStatus.WaitingForApproval;
                var page = await GetPageAsync(filter, paginationModel, x => new UserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.EmailAddress,
                    PhoneNo = x.PhoneNo,
                    UserType = x.UserType
                });
                return Ok(page.MainData, page.OtherData);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("User/Approve/{id}")]
        public async Task<IActionResult> ApproveUser(uint id)
        {
            try
            {
                var user = await FirstOrDefaultAsync<User>(x => x.Id == id);
                if (user is null)
                {
                    return NotFound();
                }
                else
                {
                    user.Status = UserStatus.Approved;
                    user.ModifiedBy = UserClaims.UserId;
                    user.ModifiedDate = UserClaims.DateTime;
                    user.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(user);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("User/Reject/{id}")]
        public async Task<IActionResult> RejectUser(uint id)
        {
            try
            {
                var user = await FirstOrDefaultAsync<User>(x => x.Id == id);
                if (user is null)
                {
                    return NotFound();
                }
                else
                {
                    user.Status = UserStatus.Rejected;
                    user.ModifiedBy = UserClaims.UserId;
                    user.ModifiedDate = UserClaims.DateTime;
                    user.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(user);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        #endregion

        #region Installer
        [HttpPost("Installer/Page")]
        public async Task<IActionResult> PendingInstallersPage(InstallerPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Installer, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.Company == "" || x.CompanyName.ToLower().Contains(paginationModel.Company.ToLower()))
                    && !x.IsDeleted && x.Status == InstallerStatus.WaitingForApproval;

                var page = await GetPageAsync(filter, paginationModel, x => new InstallerModel {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmailAddress = x.EmailAddress,
                    CompanyName = x.CompanyName
                });

                return Ok(page.MainData, page.OtherData);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Installer/Approve/{id}")]
        public async Task<IActionResult> ApproveInstaller(uint id)
        {
            try
            {
                var installer = await FirstOrDefaultAsync<Installer>(x => x.Id == id);
                if (installer is null)
                {
                    return NotFound();
                }
                else
                {
                    installer.Status = InstallerStatus.Approved;
                    installer.ModifiedBy = UserClaims.UserId;
                    installer.ModifiedDate = UserClaims.DateTime;
                    installer.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(installer);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Installer/Reject/{id}")]
        public async Task<IActionResult> RejectInstaller(uint id)
        {
            try
            {
                var installer = await FirstOrDefaultAsync<Installer>(x => x.Id == id);
                if (installer is null)
                {
                    return NotFound();
                }
                else
                {
                    installer.Status = InstallerStatus.Rejected;
                    installer.ModifiedBy = UserClaims.UserId;
                    installer.ModifiedDate = UserClaims.DateTime;
                    installer.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(installer);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        #endregion

        #region Fleet Owner
        [HttpPost("FleetOwner/Page")]
        public async Task<IActionResult> PendingFleetOwnersPage(FleetOwnerPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<FleetOwner, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.Company == "" || x.CompanyName.ToLower().Contains(paginationModel.Company.ToLower()))
                    && (paginationModel.PhoneNo == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNo.ToLower()))
                    && !x.IsDeleted && x.Status == FleetOwnerStatus.WaitingForApproval;

                var joinResult = Entities.FleetOwner.Where(filter).OrderByDescending(x => x.Id).Join(Entities.CorporateUser, a => a.UserCorporateId,
                    b => b.Id, (a, b) => new FleetOwnerModel {
                        Id = a.Id,
                        UserCorporateName = b.FirstName + " " + b.LastName,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        EmailAddress = a.EmailAddress,
                        CompanyName = a.CompanyName,
                        PhoneNo = a.PhoneNo
                    });

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("FleetOwner/Approve/{id}")]
        public async Task<IActionResult> ApproveFleetOwner(uint id)
        {
            try
            {
                var fleetOwner = await FirstOrDefaultAsync<FleetOwner>(x => x.Id == id);
                if (fleetOwner is null)
                {
                    return NotFound();
                }
                else
                {
                    fleetOwner.Status = FleetOwnerStatus.Approved;
                    fleetOwner.ModifiedBy = UserClaims.UserId;
                    fleetOwner.ModifiedDate = UserClaims.DateTime;
                    fleetOwner.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(fleetOwner);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("FleetOwner/Reject/{id}")]
        public async Task<IActionResult> RejectFleetOwner(uint id)
        {
            try
            {
                var fleetOwner = await FirstOrDefaultAsync<FleetOwner>(x => x.Id == id);
                if (fleetOwner is null)
                {
                    return NotFound();
                }
                else
                {
                    fleetOwner.Status = FleetOwnerStatus.Rejected;
                    fleetOwner.ModifiedBy = UserClaims.UserId;
                    fleetOwner.ModifiedDate = UserClaims.DateTime;
                    fleetOwner.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(fleetOwner);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        #endregion

        #region Corporate User
        [HttpPost("CorporateUser/Page")]
        public async Task<IActionResult> CorporateUserPage(CorporateUserPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<CorporateUser, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.PhoneNumber == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNumber.ToLower()))
                    && !x.IsDeleted && x.Status == CorporateUserStatus.WaitingForApproval;

                var page = await GetPageAsync(filter, paginationModel, x => new CorporateUserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmailAddress = x.EmailAddress,
                    PhoneNo = x.PhoneNo
                });

                return Ok(page.MainData, page.OtherData);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("CorporateUser/Approve/{id}")]
        public async Task<IActionResult> ApproveCorporateUser(uint id)
        {
            try
            {
                var corporateUser = await FirstOrDefaultAsync<CorporateUser>(x => x.Id == id);
                if (corporateUser is null)
                {
                    return NotFound();
                }
                else
                {
                    corporateUser.Status = CorporateUserStatus.Approved;
                    corporateUser.ModifiedBy = UserClaims.UserId;
                    corporateUser.ModifiedDate = UserClaims.DateTime;
                    corporateUser.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(corporateUser);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("CorporateUser/Reject/{id}")]
        public async Task<IActionResult> RejectCorporateUser(uint id)
        {
            try
            {
                var corporateUser = await FirstOrDefaultAsync<CorporateUser>(x => x.Id == id);
                if (corporateUser is null)
                {
                    return NotFound();
                }
                else
                {
                    corporateUser.Status = CorporateUserStatus.Rejected;
                    corporateUser.ModifiedBy = UserClaims.UserId;
                    corporateUser.ModifiedDate = UserClaims.DateTime;
                    corporateUser.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                    await UpdateAsync(corporateUser);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        #endregion

        #region Approval Manager
        [HttpPost("GetJobsWaitingForApproval")]
        public async Task<IActionResult> GetJobsWaitingForApproval([FromBody]SupervisorPaginationModel model)
        {
            try
            {
                Expression<Func<Job, bool>> filter = x => x.Status == JobStatus.UnderReviewApprovalManager && !x.IsDeleted;
                var joinResult = Entities.Job.Where(filter);
                var list = await GetPageAsync(filter, model, x => new JobModel
                {
                    Id = x.Id,
                    SiteId = x.SiteId,
                    VIN = x.VIN,
                    LPN = x.LPN,
                    Status = x.Status,
                    AssignedDate=x.CreatedDate
                });

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);
                foreach (var x in list.MainData)
                {
                    var site = await LastOrDefaultAsync<Site>(i => i.Id == x.SiteId && !i.IsDeleted);
                    x.SiteName = site.Name;
                    var bid = await LastOrDefaultAsync<Bid>(i => i.SiteId == x.SiteId && !i.IsDeleted);
                    var installer = await LastOrDefaultAsync<Installer>(i => i.Id == bid.InstallerId && !i.IsDeleted);
                    x.InstallerName = installer.FirstName + " " + installer.LastName;
                    var site_log = await LastOrDefaultAsync<SiteStatusLog>(i => i.SiteId == x.SiteId);
                    x.AssignedDate = site_log.CreatedDate;
                }


                list.MainData = (list.MainData
                   .Where(c =>
                          (model.FirstName=="" || c.InstallerName.ToLower().Contains(model.FirstName.ToLower()))
                          && (model.LastName == "" || c.SiteName.ToLower().Contains(model.LastName.ToLower()))
                          && (model.LPN == "" || c.LPN.ToLower() == model.LPN.ToLower())
                          && (model.VIN == "" || c.VIN.ToLower() == model.VIN.ToLower())
                          && (model.AssignDate == null || c.AssignedDate.Date == model.AssignDate.Value.Date)

                   )).ToList();

                return Ok(list.MainData, list.OtherData);
            }
            catch (Exception ex)
            {

                return MethodFailure(ex);
            }
        }
        [HttpGet("GetApprovalImages/{id}")]
        public async Task<IActionResult> GetApprovalImages(int id)
        {
            try
            {
                var images = await ToListAsync<JobImages>(i => i.JobId == id && i.Status != JobImagesStatus.IncidentReported && i.Status != JobImagesStatus.IsDeleted);
                return images == null ? (IActionResult)NotFound() : Ok(images);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpPost("ApproveJob")]
        public async Task<IActionResult> ApproveJob([FromBody] int JobId)
        {
            try
            {
                var job = await FirstOrDefaultAsync<Job>(i => i.Id == JobId && !i.IsDeleted);
                job.Status = JobStatus.UnderReviewSupervisor;
                var AssociatedId = await GetAssociatedId();
                var JobStatusLog = new JobStatusLogs
                {
                    JobId = job.Id,
                    Status = JobStatus.UnderReviewSupervisor,
                    CreatedBy = AssociatedId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                using (var trans = BeginTransaction())
                {
                    try
                    {
                        await UpdateAsync(job);
                        await InsertAsync(JobStatusLog);
                        trans.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("ApproveJobWaiting")]
        public async Task<IActionResult> ApproveJobWaiting([FromBody] int JobId)
        {
            try
            {
                var job = await FirstOrDefaultAsync<Job>(i => i.Id == JobId && !i.IsDeleted);
                job.Status = JobStatus.Completed;
                var AssociatedId = await GetAssociatedId();
                var jobStatus_log = new JobStatusLogs
                {
                    JobId = job.Id,
                    Status = JobStatus.Completed,
                    CreatedBy = AssociatedId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                using (var trans = BeginTransaction())
                {
                    try
                    {
                        await UpdateAsync(job);
                        await InsertAsync(jobStatus_log);
                        trans.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("RejectJob")]
        public async Task<IActionResult> RejectJob([FromBody] RejectModel model)
        {
            try
            {
                var job = await FirstOrDefaultAsync<Job>(i => i.Id == model.Id && !i.IsDeleted);
                job.Status = JobStatus.InQueue;
                var AssociatedId = await GetAssociatedId();
                var rejected_job = new RejectedJob
                {
                    JobId = model.Id,
                    Status = JobStatus.Rejected,
                    isActive = true,
                    Comments = model.comments,
                    CreatedBy = AssociatedId,
                    CreatedDate = DateTime.Now,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                var jobStatus = new JobStatusLogs
                {
                    JobId = model.Id,
                    Status = JobStatus.InQueue, // Code:By Kashif Shahzad Before--Status = JobStatus.Rejected
                    CreatedBy = AssociatedId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                using(var trans = BeginTransaction())
                {
                    try
                    {
                        await UpdateAsync(job);
                        await InsertAsync(rejected_job);
                        await InsertAsync(jobStatus);
                        trans.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
            #endregion
    }
}