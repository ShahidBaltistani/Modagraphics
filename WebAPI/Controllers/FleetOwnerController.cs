using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class FleetOwnerController : BaseController
    {
        public FleetOwnerController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var fleetOwners = await ToListAsync<FleetOwner>(x => !x.IsDeleted);

                var joinResult = Entities.FleetOwner.Join(Entities.CorporateUser, a => a.UserCorporateId, b => b.Id
                , (a, b) => new FleetOwnerModel {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    UserCorporateName = b.FirstName + " " + b.LastName,

                });

                var count = joinResult.Count();
                joinResult.Select(x => new FleetOwnerModel { Id = x.Id });

                return Ok(fleetOwners.Select(x =>
                    new FleetOwnerModel
                    {
                        Id = x.Id,
                        CorporateUserId = x.UserCorporateId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        PhoneNo = x.PhoneNo,
                        Address = x.Address,
                        PZipCode = x.PZipCode,
                        Picture = x.Picture,
                        CompanyName = x.CompanyName,
                        BillToAddress = x.BillToAddress,
                        CZipCode = x.CZipCode,
                        FaxNo = x.FaxNo,
                        CContactNo = x.CContactNo,
                        BIEmbededCode = x.BIEmbededCode,
                        Status = x.Status,
                        CCityId = x.CCityId,
                        PCityId = x.PCityId
                    }).ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var fleetOwner = await FirstOrDefaultAsync<FleetOwner>(model => model.Id == id);
                return fleetOwner == null ? (IActionResult)NotFound() : Ok(fleetOwner.Picture);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var fleetOwners = await FirstOrDefaultAsync<FleetOwner>(model => model.Id == id);
                return fleetOwners == null ? (IActionResult)NotFound() : Ok(new FleetOwnerModel
                {
                    Id = fleetOwners.Id,
                    CorporateUserId = fleetOwners.UserCorporateId,
                    FirstName = fleetOwners.FirstName,
                    LastName = fleetOwners.LastName,
                    EmailAddress = fleetOwners.EmailAddress,
                    PhoneNo = fleetOwners.PhoneNo,
                    Address = fleetOwners.Address,
                    PZipCode = fleetOwners.PZipCode,
                    Picture = fleetOwners.Picture,
                    CompanyName = fleetOwners.CompanyName,
                    BillToAddress = fleetOwners.BillToAddress,
                    CZipCode = fleetOwners.CZipCode,
                    FaxNo = fleetOwners.FaxNo,
                    CContactNo = fleetOwners.CContactNo,
                    BIEmbededCode = fleetOwners.BIEmbededCode,
                    Status = fleetOwners.Status,
                    CCityId = fleetOwners.CCityId,
                    PCityId = fleetOwners.PCityId
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("Page")]
        public async Task<IActionResult> PostPage(FleetOwnerPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<FleetOwner, bool>> filter = x =>
                (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                && (paginationModel.Company == "" || x.CompanyName.ToLower().Contains(paginationModel.Company.ToLower()))
                && (paginationModel.PhoneNo == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNo.ToLower()))
                && !x.IsDeleted;

                var joinResult = Entities.FleetOwner.Where(filter).OrderByDescending(x => x.Id).Join(Entities.CorporateUser, a => a.UserCorporateId, b => b.Id
                , (a, b) => new FleetOwnerModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    UserCorporateName = b.FirstName + " " + b.LastName,
                    EmailAddress = a.EmailAddress,
                    CompanyName = a.CompanyName,
                    PhoneNo = a.PhoneNo,
                    Status = a.Status
                }).Where(c => paginationModel.CorporateUserName == "" || c.UserCorporateName.ToLower().Contains(paginationModel.CorporateUserName.ToLower()));

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result)
                {
                    x.NumberOfProject = await CountAsync<Project>(y => y.FleetOwnerId == x.Id && !y.IsDeleted);
                    x.NumberOfSite = await CountAsync<Site>(y => y.FleetOwnerId == x.Id && !y.IsDeleted);
                    x.NumberOfSupervisor = await CountAsync<Supervisor>(y => y.FleetOwnerId == x.Id && !y.IsDeleted);
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("AwardedSites")]
        public async Task<IActionResult> PostAwardedSites([FromBody]SupervisorPaginationModel model)
        {
            try
            {
                var fleetOwnerID = await GetAssociatedId();
                Expression<Func<Site, bool>> filter = x => x.FleetOwnerId == fleetOwnerID && !x.IsDeleted;
                //var sites = await GetPageAsync(filter, model);
                //var sites = await GetPageAsync(filter, model, x => new Site
                //{
                //    Id = x.Id,
                //    ProjectId = x.ProjectId,
                //    Name = x.Name,
                //    SiteStatus = x.SiteStatus,
                //    SiteStartDate = x.SiteStartDate,
                //});
                //return Ok(sites.MainData, sites.OtherData);
                var joinResult = Entities.Site.Where(filter).Join(Entities.Project, s => s.ProjectId, p => p.Id, (a, b) => new BidModel
                {
                    SiteName = a.Name,
                    SiteId = a.Id,
                    SiteStatus = a.SiteStatus,
                    ProjectName = b.Name
                }).Where(x => !Entities.SupervisorSiteAssociation.Any(y => y.SiteId == x.SiteId && !y.IsDeleted)  
                && (model.FirstName=="" || x.ProjectName.ToLower().Contains(model.FirstName.ToLower()))
                &&(model.LastName=="" || x.SiteName.ToLower().Contains(model.LastName.ToLower()))
                
                );

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);

                foreach(var x in result)
                {
                    x.SiteVehicals = await CountAsync<Job>(y=> y.SiteId == x.SiteId);
                }
                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("AssignSite")]
        public async Task<IActionResult> PostAssignSite([FromBody] ChildEntries model)
        {
            try
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        await RemoveRangeAsync(await ToListAsync<SupervisorSiteAssociation>(m => m.SiteId == model.MainId));

                        foreach (var x in model.ChildIds)
                        {
                            var supervisor = new SupervisorSiteAssociation
                            {
                                SupervisorId = x,
                                SiteId = model.MainId,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync(supervisor);
                        }

                        tran.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FleetOwnerModel fleetOwners)
        {
            try
            {
                var vFO = new FleetOwner
                {
                    Id = fleetOwners.Id,
                    UserCorporateId = fleetOwners.CorporateUserId,
                    FirstName = fleetOwners.FirstName,
                    LastName = fleetOwners.LastName,
                    EmailAddress = fleetOwners.EmailAddress,
                    PhoneNo = fleetOwners.PhoneNo,
                    Address = fleetOwners.Address,
                    PZipCode = fleetOwners.PZipCode,
                    Picture = fleetOwners.Picture,
                    CompanyName = fleetOwners.CompanyName,
                    BillToAddress = fleetOwners.BillToAddress,
                    CZipCode = fleetOwners.CZipCode,
                    FaxNo = fleetOwners.FaxNo,
                    CContactNo = fleetOwners.CContactNo,
                    BIEmbededCode = fleetOwners.BIEmbededCode,
                    Status = fleetOwners.Status,
                    CCityId = fleetOwners.CCityId,
                    PCityId = fleetOwners.PCityId,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vFO);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]FleetOwnerModel fleetOwners)
        {
            try
            {
                if (!await AnyAsync<FleetOwner>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vFO = await FirstOrDefaultAsync<FleetOwner>(model => model.Id == id);
                vFO.UserCorporateId = fleetOwners.CorporateUserId;
                vFO.FirstName = fleetOwners.FirstName;
                vFO.LastName = fleetOwners.LastName;
                vFO.EmailAddress = fleetOwners.EmailAddress;
                vFO.PhoneNo = fleetOwners.PhoneNo;
                vFO.Address = fleetOwners.Address;
                vFO.PZipCode = fleetOwners.PZipCode;
                vFO.Picture = fleetOwners.Picture;
                vFO.CompanyName = fleetOwners.CompanyName;
                vFO.BillToAddress = fleetOwners.BillToAddress;
                vFO.CZipCode = fleetOwners.CZipCode;
                vFO.FaxNo = fleetOwners.FaxNo;
                vFO.CContactNo = fleetOwners.CContactNo;
                vFO.BIEmbededCode = fleetOwners.BIEmbededCode;
                vFO.Status = fleetOwners.Status;
                vFO.CCityId = fleetOwners.CCityId;
                vFO.PCityId = fleetOwners.PCityId;
                vFO.IsDeleted = false;
                vFO.ModifiedBy = UserClaims.UserId;
                vFO.ModifiedDate = UserClaims.DateTime;
                vFO.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vFO);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var fleetOwner = await FirstOrDefaultAsync<FleetOwner>(x => x.Id == id);
                if (fleetOwner is null) return NotFound();
                fleetOwner.IsDeleted = true;
                fleetOwner.ModifiedBy = UserClaims.UserId;
                fleetOwner.ModifiedDate = UserClaims.DateTime;
                fleetOwner.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(fleetOwner);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpGet("Dropdown")]
        public async Task<IActionResult> DropDown()
        {
            try
            {
                var fleetOwner = await SelectAsync<FleetOwner, DropdownModel>(x => !x.IsDeleted && x.Status == FleetOwnerStatus.Approved, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                return Ok(fleetOwner.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        
        [HttpGet("DropdownForAddLogin")]
        public async Task<IActionResult> DropDownForAddLogin()
        {
            try
            {
                List<DropdownModel> dropdowns = new List<DropdownModel>();
                var fleetOwner = await SelectAsync<FleetOwner, DropdownModel>(x => !x.IsDeleted && x.Status == FleetOwnerStatus.Approved, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                foreach (var item in fleetOwner.ToList())
                {
                    bool isExist = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.FleetOwner);
                    if (!isExist)
                    {
                        dropdowns.Add(item);
                    }
                }
                return Ok(dropdowns);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        //[HttpGet("IsUniqueEmail/{id}/{email}")]
        //public async Task<IActionResult> VerifyEmail(uint id, string email)
        //{
        //    try
        //    {
        //        return Ok(!await AnyAsync<FleetOwner>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower()));
        //    }
        //    catch (Exception ex)
        //    {
        //        return MethodFailure(ex);
        //    }
        //}
        [HttpGet("IsUniqueEmail/{id}/{email}")]
        public async Task<IActionResult> VerifyEmail(uint id, string email)
        {
            try
            {
                return Ok(!await AnyAsync<FleetOwner>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<CorporateUser>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<User>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Installer>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Crew>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Supervisor>(x => x.EmailAddress.ToLower() == email.ToLower())
                    );
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("GetVehiclesAgainstAssignedSites/{Id}")]
        public async Task<IActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var joinresult = Entities.VehicleTypeSiteAssociation.Where(x => x.SiteId == Id).Join(Entities.VehicleTypes, x => x.VehicleTypeId, v => v.Id,
                (a, b) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    VehicleTypeName = b.Name,
                });
                var newlist = joinresult.ToList();

                foreach (var item in newlist)
                {
                    item.Vehicles = await CountAsync<Job>(x => x.SiteVehicleTypeId == item.Id);
                }
                return Ok(newlist);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Approve Jobs
        [HttpPost("GetJobsWaitingForApproval")]
        public async Task<IActionResult> GetJobsWaitingForApproval([FromBody] SupervisorPaginationModel model)
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var _association = await ToListAsync<SupervisorSiteAssociation>(x => x.SupervisorId == AssociatedId);
                Expression<Func<Job, bool>> filter = x => x.Status == JobStatus.UnderReviewSupervisor && !x.IsDeleted
                
                // Code By : Kashif Shahzad
                && (model.LPN=="" || x.LPN==model.LPN)
                && (model.VIN=="" || x.VIN==model.VIN)
                //Code End
                
                
                ;//&& x.SiteId == _association[0].SiteId
                var joinResult = Entities.Job.Where(filter);
                var list = await GetPageAsync(filter, model, x => new JobModel
                {
                    Id = x.Id,
                    SiteId = x.SiteId,
                    VIN = x.VIN,
                    LPN = x.LPN,
                    Status = x.Status,
                });

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);
                foreach (var x in list.MainData.ToList())
                {
                    var site = await LastOrDefaultAsync<Site>(i => i.Id == x.SiteId && !i.IsDeleted && (model.LastName=="" || i.Name.ToLower().Contains(model.LastName.ToLower())) );
                    if (site!=null)
                    {
                        x.SiteName = site.Name;
                    }
                    else if (!string.IsNullOrEmpty(model.LastName))
                    {
                        list.MainData.Remove(x);
                    }
                    
                    var bid = await LastOrDefaultAsync<Bid>(i => i.SiteId == x.SiteId && !i.IsDeleted);
                    var installer = await LastOrDefaultAsync<Installer>(i => i.Id == bid.InstallerId && !i.IsDeleted && (model.FirstName=="" || (i.FirstName+""+i.LastName).ToLower().Contains(model.FirstName.ToLower())));
                    if (installer!=null)
                    {
                        x.InstallerName = installer.FirstName + " " + installer.LastName;
                    }
                    else if (!string.IsNullOrEmpty(model.FirstName))
                    {
                        list.MainData.Remove(x);
                    }
                    //var site_log = await LastOrDefaultAsync<SiteStatusLog>(i => i.SiteId == x.SiteId);
                    //x.AssignedDate = site_log.CreatedDate;


                    // CB:KS
                    var site_log = await LastOrDefaultAsync<SiteStatusLog>(i => i.SiteId == x.SiteId && (model.AssignDate==null || i.CreatedDate.Date==model.AssignDate.GetValueOrDefault().Date) );
                    if (site_log!=null)
                    {
                        x.AssignedDate = site_log.CreatedDate;
                    }
                    else if (!(model.AssignDate==null))
                    {
                        list.MainData.Remove(x);

                    }
                    //EC
                }

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
        [HttpPost("ApproveJobWaiting")]
        public async Task<IActionResult> ApproveJob([FromBody] int JobId)
        {
            try
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        var job = await FirstOrDefaultAsync<Job>(i => i.Id == JobId && !i.IsDeleted);
                        job.Status = JobStatus.Completed;
                        await UpdateAsync(job);

                        var TotalJobs = await CountAsync<Job>(x => x.SiteId == job.SiteId && !x.IsDeleted);
                        var CompletedJobs = await CountAsync<Job>(x => x.SiteId == job.SiteId && x.Status == JobStatus.Completed && !x.IsDeleted);
                        if (TotalJobs == CompletedJobs)
                        {
                            var site = await FirstOrDefaultAsync<Site>(x => x.Id == job.SiteId && !x.IsDeleted);
                            site.SiteStatus = SiteStatus.ReadyForInvoice;
                            await UpdateAsync(site);

                            var log = new SiteStatusLog
                            {
                                SiteId = site.Id,
                                SiteStatus = site.SiteStatus,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync<SiteStatusLog>(log);

                        }
                        tran.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
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

        [HttpPost("VehiclesToRepair")]
        public async Task<IActionResult> PostVehiclesToReports([FromBody] SupervisorPaginationModel model)
        {
            try
            {
                var FleetOwnerId = await GetAssociatedId();
                //Expression<Func<SupervisorSiteAssociation, bool>> filter = x => x.SupervisorId == SupervisorID;
                //var joinResult = Entities.SupervisorSiteAssociation.Where(filter).Join(Entities.Site, s => s.SiteId, p => p.Id,
                //    (a, b) => new
                //    {
                //        SupervisorID = a.Id,
                //        SiteID = a.SiteId,
                //        SiteName = b.Name,
                //        ProjectId = b.ProjectId,
                //    })
                var joinResult = Entities.Site.Where(x=> x.FleetOwnerId == FleetOwnerId).Join(Entities.Job, s => s.Id, j => j.SiteId,
                    (a, b) => new IncidentReportModel
                    {
                       
                        SiteId = a.Id,
                        SiteName = a.Name,
                        ProjectId = a.ProjectId,
                        JobId = b.Id,
                        SiteVehicleTypeId = b.SiteVehicleTypeId,
                        VIN = b.VIN,
                        LPN = b.LPN,
                        UnitNo = b.UnitNo,
                        JobStatus = b.Status
                    }).Where(x => x.JobStatus == JobStatus.SentForRepair && (model.LastName=="" || x.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    
                    &&(model.LPN==""||x.LPN==model.LPN)
                    &&(model.VIN==""||x.VIN==model.VIN)
                    &&(model.Unit==0||x.UnitNo==model.Unit)
                    
                    );

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);

                foreach (var x in result.ToList())
                {
                    var project = await FirstOrDefaultAsync<Project>(i => i.Id == x.ProjectId && !i.IsDeleted && (model.FirstName=="" || i.Name.ToLower().Contains(model.FirstName.ToLower())) );
                    if (project!=null)
                    {
                        x.ProjectName = project.Name;
                    }
                    else if (!string.IsNullOrEmpty(model.FirstName))
                    {
                        result.Remove(x);
                    }
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("Repaired")]
        public async Task<IActionResult> PostRepairJob([FromBody] int JobId)
        {
            try
            {
                var Job = await FirstOrDefaultAsync<Job>(x => x.Id == JobId);
                Job.Status = JobStatus.InQueue;
                Job.ModifiedBy = UserClaims.UserId;
                Job.ModifiedDate = UserClaims.DateTime;
                await UpdateAsync(Job);


                // Code for Job-Status-Log ---By :Kashif Shahzad

                var log = new JobStatusLogs
                {
                    Id = 0,
                    JobId = (uint)JobId,
                    Status = JobStatus.InQueue,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                await InsertAsync(log);
                // Code for Job-Status-Log

                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("incidentReports")]
        public async Task<IActionResult> PostIncidentReports([FromBody] SupervisorPaginationModel model)
        {
            try
            {
                var FleetOwnerID = await GetAssociatedId();
                //Expression<Func<SupervisorSiteAssociation, bool>> filter = x => x.SupervisorId == SupervisorID;
                //var joinResult = Entities.SupervisorSiteAssociation.Where(filter).Join(Entities.Site, s=>s.SiteId, p=>p.Id,
                //    (a,b) => new
                //    {
                //        SupervisorID = a.Id,
                //        SiteID = a.SiteId,
                //        SiteName = b.Name,
                //        ProjectId = b.ProjectId,
                //    }).Join(Entities.Job,s=>s.SiteID , j=>j.SiteId,(a,b)=>new {
                //        SupervisorID = a.SupervisorID,
                //        SiteID = a.SiteID,
                //        SiteName = a.SiteName,
                //        ProjectId = a.ProjectId,
                //        JobId = b.Id,
                //        VehicleTypeID = b.SiteVehicleTypeId,
                //        VIN = b.VIN,
                //        LPN = b.LPN,
                //        UnitNo = b.UnitNo,
                //        Status = b.Status
                //    }).Join(Entities.JobImages, x=>x.JobId, y=>y.JobId,
                //    (a,b)=> new IncidentReportModel{
                //        SupervisorId = a.SupervisorID,
                //        SiteId = a.SiteID,
                //        SiteName = a.SiteName,
                //        JobId = a.JobId,
                //        ProjectId = a.ProjectId,
                //        SiteVehicleTypeId = a.VehicleTypeID,
                //        VIN = a.VIN,
                //        LPN = a.LPN,
                //        UnitNo = a.UnitNo,
                //         = a.Status,
                //        Picture = b.Picture
                //    }).Where(x=>x.JobStatus == JobStatus.IncidentReported);
                //var joinResult = Entities.SupervisorSiteAssociation.Where(filter).Join(Entities.Site, a => a.SiteId, s => s.Id,
                //    (a, b) => new {
                //        SupervisorID = a.Id,
                //        SiteID = a.SiteId,
                //        SiteName = b.Name,
                //        ProjectId = b.ProjectId
                //    })
                    var joinResult = Entities.Site.Where(x=> x.FleetOwnerId == FleetOwnerID)
                    .Join(Entities.Job, x => x.Id, j => j.SiteId,
                    (a, b) => new IncidentReportModel
                    {
                        //SupervisorId = a.SupervisorID,
                        SiteId = a.Id,
                        SiteName = a.Name,
                        ProjectId = a.ProjectId,
                        JobId = b.Id,
                        SiteVehicleTypeId = b.SiteVehicleTypeId,
                        VIN = b.VIN,
                        LPN = b.LPN,
                        UnitNo = b.UnitNo,
                        JobStatus = b.Status
                    }).Where(x => x.JobStatus == JobStatus.IncidentReported  && (model.LastName=="" || x.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    
                    && (model.LPN==""||x.LPN==model.LPN)
                    && (model.VIN==""||x.VIN==model.VIN)
                    && (model.Unit==0||x.UnitNo==model.Unit)
                    );

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);
                foreach (var x in result.ToList())
                {
                    var reportImage = await LastOrDefaultAsync<JobImages>(i => i.JobId == x.JobId && !i.IsDeleted);
                    x.JobImageID = reportImage.Id;
                    var project = await FirstOrDefaultAsync<Project>(i => i.Id == x.ProjectId && !i.IsDeleted && (model.FirstName=="" || i.Name.ToLower().Contains(model.FirstName.ToLower())) );
                    if (project!=null)
                    {
                        x.ProjectName = project.Name;
                    }
                    else if (!string.IsNullOrEmpty(model.FirstName))
                    {
                        result.Remove(x);
                    }
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("ApproveJob")]
        public async Task<IActionResult> PostApproveJob([FromBody] int JobId)
        {
            try
            {
                var Job = await FirstOrDefaultAsync<Job>(x => x.Id == JobId);
                Job.Status = JobStatus.InQueue;
                Job.ModifiedBy = UserClaims.UserId;
                Job.ModifiedDate = UserClaims.DateTime;
                await UpdateAsync(Job);

                // Code for Job-Status-Log ---By :Kashif Shahzad
                var log = new JobStatusLogs
                {
                    Id = 0,
                    JobId = (uint)JobId,
                    Status = JobStatus.InQueue,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                await InsertAsync(log);
                // Code for Job-Status-Log

                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("SentForRepair")]
        public async Task<IActionResult> PostSentForRepair([FromBody] int JobId)
        {
            try
            {
                var Job = await FirstOrDefaultAsync<Job>(x => x.Id == JobId);
                Job.Status = JobStatus.SentForRepair;
                Job.ModifiedBy = UserClaims.UserId;
                Job.ModifiedDate = UserClaims.DateTime;
                await UpdateAsync(Job);


                // Code for Job-Status-Log ---By :Kashif Shahzad
                var log = new JobStatusLogs
                {
                    Id = 0,
                    JobId = (uint)JobId,
                    Status = JobStatus.SentForRepair,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                await InsertAsync(log);
                // Code for Job-Status-Log

                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("IncidentImage/{id}")]
        public async Task<IActionResult> GetIncidentImage(int id)
        {
            try
            {
                var jobImage = await FirstOrDefaultAsync<JobImages>(model => model.Id == id);
                return jobImage == null ? (IActionResult)NotFound() : Ok(jobImage.Picture);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }


        [HttpPost("SitesByFleetId")]
        public async Task<IActionResult> SitesByFleetId(SitePaginationModel paginationModel)
        {
            try
            {
                uint FleetOwnerId =await GetAssociatedId();
                Expression<Func<Site, bool>> filter = x =>
                (paginationModel.SiteName == "" || x.Name.ToLower().Contains(paginationModel.SiteName.ToLower()))
                && (paginationModel.PO == "" || x.PONo.ToLower().Contains(paginationModel.PO.ToLower()))
                && (paginationModel.SiteStatus == null || (int)x.SiteStatus == paginationModel.SiteStatus)
                && x.FleetOwnerId ==FleetOwnerId
                && !x.IsDeleted;

                var joinResult = Entities.Site.Where(filter).OrderByDescending(x => x.Id).Join(Entities.Project, s => s.ProjectId, p => p.Id
                , (a, b) => new
                {
                    a.Id,
                    a.Name,
                    ProjectName = b.Name,
                    a.PONo,
                    b.FleetOwnerId,
                    a.SiteStatus
                }).Join(Entities.FleetOwner, s => s.FleetOwnerId, f => f.Id
                , (s, f) => new SiteModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ProjectName = s.ProjectName,
                    FleetOwnerId = f.Id,
                    FleetOwnerName = f.FirstName + " " + f.LastName,
                    PONo = s.PONo,
                    SiteStatus = s.SiteStatus
                    //TotalVehicleTypes = 5,
                    //TotalVehicles = 30,
                    //TotalBids = 10
                }).Where(f => paginationModel.FleetOwnerName == "" || f.FleetOwnerName.ToLower().Contains(paginationModel.FleetOwnerName.ToLower()));

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result)
                {
                    x.TotalVehicleTypes = await CountAsync<SiteVehicleType>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.TotalBids = await CountAsync<Bid>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.TotalVehicles = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.JobsCompleted = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.Status == JobStatus.Completed);
                    x.JobsInQueue = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.Status == JobStatus.InQueue);
                    x.JobsInProgress = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.Status == JobStatus.InProcess);
                    x.JobsUnderReview = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && (y.Status == JobStatus.UnderReviewSupervisor || y.Status == JobStatus.UnderReviewApprovalManager));
                }
                result = result.Where(c => 
                   paginationModel.ProjectName == "" || c.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                    .ToList();
                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }



        [HttpPost("JobsByFleetId")]
        public async Task<IActionResult> JobsByFleetId(CrewJobPaginationModel paginationModel)
        {
            try
            {
                var crewId = await GetAssociatedId();
                Expression<Func<Job, bool>> filter = x =>
                    (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && (paginationModel.JobStatus == null || (int)x.Status == paginationModel.JobStatus)
                    && !x.IsDeleted;

                var joinResult = from j in Entities.Job.Where(filter)
                                 join s in Entities.Site
                                       on j.SiteId equals s.Id
                                    where s.FleetOwnerId == crewId
                                    && s.IsDeleted == false
                                   join p in Entities.Project
                                    on s.ProjectId equals p.Id
                                   where s.IsDeleted == false

                                 select new CrewSiteModel
                                 {
                                     ProjectName = p.Name,
                                     SiteName = s.Name,
                                     VehicleTypeName = string.Empty,
                                     VIN = j.VIN,
                                     LPN = j.LPN,
                                     UnitNo = j.UnitNo,
                                     Status = j.Status,
                                     SiteId = s.Id,
                                     JobId = j.Id,
                                     FleetOwnerId = s.FleetOwnerId
                                 };


                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                }

                result = result.Where(c =>
                       (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
                    && (paginationModel.ProjectName == "" || c.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                    && (paginationModel.SiteName == "" || c.SiteName.ToLower().Contains(paginationModel.SiteName.ToLower()))
                    ).ToList();


                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpPost("SupervisorAwardedSites")]
        public async Task<IActionResult> PostSupervisorAwardedSites([FromBody] SupervisorPaginationModel model)
        {
            try
            {
                var fleetOwnerID = await GetAssociatedId();
                Expression<Func<Site, bool>> filter = x => x.FleetOwnerId == fleetOwnerID && !x.IsDeleted;
                //var sites = await GetPageAsync(filter, model);
                //var sites = await GetPageAsync(filter, model, x => new Site
                //{
                //    Id = x.Id,
                //    ProjectId = x.ProjectId,
                //    Name = x.Name,
                //    SiteStatus = x.SiteStatus,
                //    SiteStartDate = x.SiteStartDate,
                //});
                //return Ok(sites.MainData, sites.OtherData);
                var joinResult = Entities.Site.Where(filter).Join(Entities.Project, s => s.ProjectId, p => p.Id, (a, b) => new BidModel
                {
                    SiteName = a.Name,
                    SiteId = a.Id,
                    SiteStatus = a.SiteStatus,
                    ProjectName = b.Name,
                    SiteSupervisors = Entities.SupervisorSiteAssociation.Where(asc => asc.SiteId.Equals(a.Id) && !asc.IsDeleted).Select(x => x.SupervisorId)
                }).Where(x => Entities.SupervisorSiteAssociation.Any(y => y.SiteId == x.SiteId && !y.IsDeleted)
                && (model.FirstName == "" || x.ProjectName.ToLower().Contains(model.FirstName.ToLower()))
                && (model.LastName == "" || x.SiteName.ToLower().Contains(model.LastName.ToLower()))

                );

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);

                foreach (var x in result)
                {
                    x.SiteVehicals = await CountAsync<Job>(y => y.SiteId == x.SiteId);
                }
                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

    }
}

            