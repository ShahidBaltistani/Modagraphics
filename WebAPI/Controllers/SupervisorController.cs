using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class SupervisorController : BaseController
    {
        public SupervisorController(IHttpContextAccessor httpContext) : base(httpContext) { }


        [HttpGet("GetVehiclesAgainstAssignedSites/{Id}")]
        public async Task<IActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var joinresult =Entities.VehicleTypeSiteAssociation.Where(x => x.SiteId == Id).Join(Entities.VehicleTypes,x=>x.VehicleTypeId,v=>v.Id,
                (a, b)=>new SiteVehicleTypeModel
                {
                    Id=a.Id,
                    VehicleTypeName=b.Name,
                });
                var newlist = joinresult.ToList();

                foreach (var item in newlist)
                {
                    item.Vehicles =await CountAsync<Job>(x => x.SiteVehicleTypeId == item.Id);
                }
                return Ok(newlist);



                //var SiteVehicleType=await ToListAsync<SiteVehicleType>(x=>x.SiteId==Id);
                //var VehiclesTypes = await ToListAsync<VehicleType>();

                //List<SiteVehicleTypeModel> siteVehicleTypeModels = new List<SiteVehicleTypeModel>();

                //foreach (var item in SiteVehicleType)
                //{
                //    foreach (var item1 in VehiclesTypes)
                //    {
                //        if (item.VehicleTypeId ==item1.Id)
                //        {

                //            var data = new SiteVehicleTypeModel
                //            {
                //                VehicleTypeName = item1.Name,
                //                Vehicles = await CountAsync<Job>(x => x.SiteVehicleTypeId == item.Id)
                //            };
                //            siteVehicleTypeModels.Add(data);

                //        }
                //    }
                    
                //}
                //return siteVehicleTypeModels == null ? (IActionResult)NotFound() : Ok(siteVehicleTypeModels);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userSupervisors = await ToListAsync<Supervisor>(x => !x.IsDeleted);
                return Ok(userSupervisors.Select(x =>
                    new SupervisorModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        PhoneNo = x.PhoneNo,
                        Address = x.Address,
                        ZipCode = x.ZipCode,
                        CityId = x.CityId,
                        Picture = x.Picture,
                        Status = x.Status
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
                var userSupervisor = await FirstOrDefaultAsync<Supervisor>(model => model.Id == id);
                return userSupervisor == null ? (IActionResult)NotFound() : Ok(userSupervisor.Picture);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var userSupervisor = await FirstOrDefaultAsync<Supervisor>(model => model.Id == id);
                return userSupervisor == null ? (IActionResult)NotFound() : Ok(new SupervisorModel
                {
                    Id = userSupervisor.Id,
                    FirstName = userSupervisor.FirstName,
                    LastName = userSupervisor.LastName,
                    EmailAddress = userSupervisor.EmailAddress,
                    PhoneNo = userSupervisor.PhoneNo,
                    Address = userSupervisor.Address,
                    ZipCode = userSupervisor.ZipCode,
                    CityId = userSupervisor.CityId,
                    Picture = userSupervisor.Picture,
                    Status = userSupervisor.Status,
                    FleetOwnerId = userSupervisor.FleetOwnerId
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("Page")]
        public async Task<IActionResult> PostPage(SupervisorPaginationModel paginationModel)
        {
            try
            {
                var FleetOwnerId = await GetAssociatedId();

                Expression<Func<Supervisor, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.PhoneNumber == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNumber.ToLower()))
                    && !x.IsDeleted
                    && x.FleetOwnerId==FleetOwnerId;

                var user = await ToListAsync(filter);

                var joinResult = Entities.Supervisor.Where(filter).OrderByDescending(x => x.Id).Join(Entities.FleetOwner, sv => sv.FleetOwnerId, fh => fh.Id
                , (sv, fh) => new SupervisorModel
                {
                    Id = sv.Id,
                    SupervisorUserName = "",
                    FirstName = sv.FirstName,
                    LastName = sv.LastName,
                    EmailAddress = sv.EmailAddress,
                    PhoneNo = sv.PhoneNo,
                    Status = sv.Status
                });

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);
                foreach (var item in result.ToList())
                {
                    item.NumberOfSite = await CountAsync<SupervisorSiteAssociation>(x => x.SupervisorId == item.Id);
                    item.IsLoginExists = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.Supervisor);
                    var Login = await FirstOrDefaultAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.Supervisor && x.AssociatedId==item.Id && ( paginationModel.SupervisorUserName=="" || x.UserName.ToLower().Contains(paginationModel.SupervisorUserName.ToLower()) ));
                    if (Login!=null)
                    {
                        item.SupervisorUserName = Login.UserName;
                    }
                    else if(!string.IsNullOrEmpty(paginationModel.SupervisorUserName))
                    {
                        result.Remove(item);
                    }
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupervisorModel userSupervisor)
        {
            try
            {
                var FleetOwnerId = await GetAssociatedId();
                var vSV = new Supervisor
                {
                    Id = userSupervisor.Id,
                    FirstName = userSupervisor.FirstName,
                    LastName = userSupervisor.LastName,
                    EmailAddress = userSupervisor.EmailAddress,
                    PhoneNo = userSupervisor.PhoneNo,
                    Address = userSupervisor.Address,
                    ZipCode = userSupervisor.ZipCode,
                    CityId = userSupervisor.CityId,
                    Picture = userSupervisor.Picture,
                    Status = userSupervisor.Status,
                    //FleetOwnerId = userSupervisor.FleetOwnerId,
                    FleetOwnerId = FleetOwnerId,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vSV);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("incidentReports")]
        public async Task<IActionResult> PostIncidentReports([FromBody]SupervisorPaginationModel model)
        {
            try
            {
                var SupervisorID = await GetAssociatedId();
                Expression<Func<SupervisorSiteAssociation, bool>> filter = x => x.SupervisorId == SupervisorID;
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
                var joinResult = Entities.SupervisorSiteAssociation.Where(filter).Join(Entities.Site, a => a.SiteId, s => s.Id,
                    (a, b) => new {
                        SupervisorID = a.Id,
                        SiteID = a.SiteId,
                        SiteName = b.Name,
                        ProjectId = b.ProjectId
                    }).Join(Entities.Job, x => x.SiteID, j => j.SiteId,
                    (a, b) => new IncidentReportModel
                    {
                        SupervisorId = a.SupervisorID,
                        SiteId = a.SiteID,
                        SiteName = a.SiteName,
                        ProjectId = a.ProjectId,
                        JobId = b.Id,
                        SiteVehicleTypeId = b.SiteVehicleTypeId,
                        VIN = b.VIN,
                        LPN = b.LPN,
                        UnitNo = b.UnitNo,
                        JobStatus = b.Status
                    }).Where(x => x.JobStatus == JobStatus.IncidentReported && (model.LastName=="" || x.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    
                    &&(model.LPN==""||x.LPN==model.LPN)
                    &&(model.VIN==""||x.VIN==model.VIN)
                    &&(model.Unit==0||x.UnitNo==model.Unit)
                    
                    );
                
                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);
                foreach (var x in result.ToList())
                {
                    var reportImage = await LastOrDefaultAsync<JobImages>(i => i.JobId == x.JobId && !i.IsDeleted);
                    x.JobImageID = reportImage.Id;
                    var project = await FirstOrDefaultAsync<Project>(i => i.Id == x.ProjectId && !i.IsDeleted && ( model.FirstName=="" || i.Name.ToLower().Contains(model.FirstName.ToLower()) ) );
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
        public async Task<IActionResult> PostApproveJob([FromBody]int JobId)
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
        public async Task<IActionResult> PostSentForRepair([FromBody]int JobId)
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

        [HttpPost("VehiclesToRepair")]
        public async Task<IActionResult> PostVehiclesToReports([FromBody] SupervisorPaginationModel model)
        {
            try
            {
                var SupervisorID = await GetAssociatedId();
                Expression<Func<SupervisorSiteAssociation, bool>> filter = x => x.SupervisorId == SupervisorID;
                var joinResult = Entities.SupervisorSiteAssociation.Where(filter).Join(Entities.Site, s => s.SiteId, p => p.Id,
                    (a, b) => new
                    {
                        SupervisorID = a.Id,
                        SiteID = a.SiteId,
                        SiteName = b.Name,
                        ProjectId = b.ProjectId,
                    }).Join(Entities.Job, s => s.SiteID, j => j.SiteId, 
                    (a, b) => new IncidentReportModel
                    {
                        SupervisorId = a.SupervisorID,
                        SiteId = a.SiteID,
                        SiteName = a.SiteName,
                        ProjectId = a.ProjectId,
                        JobId = b.Id,
                        SiteVehicleTypeId = b.SiteVehicleTypeId,
                        VIN = b.VIN,
                        LPN = b.LPN,
                        UnitNo = b.UnitNo,
                        JobStatus = b.Status
                    }).Where(x=> x.JobStatus == JobStatus.SentForRepair && (model.LastName=="" || x.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    
                    &&(model.LPN==""||x.LPN==model.LPN)
                    &&(model.VIN==""||x.VIN==model.VIN)
                    &&(model.Unit==0||x.UnitNo==model.Unit)
                    );

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(model);

                foreach (var x in result.ToList())
                {
                    var project = await FirstOrDefaultAsync<Project>(i => i.Id == x.ProjectId && !i.IsDeleted  && (model.FirstName=="" || i.Name.ToLower().Contains(model.FirstName.ToLower())) );
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
        public async Task<IActionResult> PostRepairJob([FromBody]int JobId)
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
                    JobId =(uint)JobId,
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

        [HttpPost("AssignedSites")]
        public async Task<IActionResult> PostAssignedSites([FromBody] SupervisorPaginationModel model)
        {
            try
            {
                var supervisorID = await GetAssociatedId();
                Expression<Func<SupervisorSiteAssociation, bool>> filtr = x => x.SupervisorId == supervisorID && !x.IsDeleted;
                var joinResult = Entities.SupervisorSiteAssociation.Where(filtr).Join(Entities.Site, x => x.SiteId, s => s.Id,
                    (a, b) => new
                    {
                        a.SupervisorId,
                        a.SiteId,
                        SiteName = b.Name,
                        AssignedDate = a.CreatedDate,
                        b.ProjectId
                    }).Join(Entities.Project, x => x.ProjectId, p => p.Id,
                    (a, b) => new IncidentReportModel
                    {
                        SupervisorId = a.SupervisorId,
                        SiteId = a.SiteId,
                        SiteName = a.SiteName,
                        ProjectName = b.Name,
                        AssignedDate = a.AssignedDate,
                    }).Where(z=>
                    
                    (model.FirstName=="" || z.ProjectName.ToLower().Contains(model.FirstName.ToLower()))
                    &&(model.LastName=="" || z.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    &&(model.AssignDate==null||z.AssignedDate.Date==model.AssignDate.GetValueOrDefault().Date)
                    );

                var newlist = joinResult.ToList();
                foreach (var x in newlist)
                {
                    x.vechicleCount = await CountAsync<Job>(i => i.SiteId == x.SiteId && !i.IsDeleted);
                }
                var count = await joinResult.CountAsync();
                var result = newlist.GetPage(model);
                return Ok(result, count);

            }
            catch (Exception ex)
            {

                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]SupervisorModel userSupervisor)
        {
            try
            {
                if (!await AnyAsync<Supervisor>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vSV = await FirstOrDefaultAsync<Supervisor>(model => model.Id == id);
                //vSV.Id = userSupervisor.Id;
                vSV.FirstName = userSupervisor.FirstName;
                vSV.LastName = userSupervisor.LastName;
                vSV.EmailAddress = userSupervisor.EmailAddress;
                vSV.PhoneNo = userSupervisor.PhoneNo;
                vSV.Address = userSupervisor.Address;
                vSV.ZipCode = userSupervisor.ZipCode;
                vSV.CityId = userSupervisor.CityId;
                vSV.Picture = userSupervisor.Picture;
                vSV.Status = userSupervisor.Status;
                vSV.FleetOwnerId = userSupervisor.FleetOwnerId;
                vSV.IsDeleted = false;
                vSV.ModifiedBy = UserClaims.UserId;
                vSV.ModifiedDate = UserClaims.DateTime;
                vSV.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vSV);
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
                var userSupervisor = await FirstOrDefaultAsync<Supervisor>(x => x.Id == id);
                if (userSupervisor is null) return NotFound();
                userSupervisor.IsDeleted = true;
                userSupervisor.ModifiedBy = UserClaims.UserId;
                userSupervisor.ModifiedDate = UserClaims.DateTime;
                userSupervisor.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(userSupervisor);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }


        // Seperate Code Written By Kashif Shahzad
        [HttpGet("DropdownForSiteAssigning")]
        public async Task<IActionResult> DropdownForSiteAssigning()
        {
            try
            {
                var fleetOwnerID = await GetAssociatedId();
                var project = await SelectAsync<Supervisor, DropdownModel>(x => !x.IsDeleted && x.Status == SupervisorStatus.Approved && x.FleetOwnerId == fleetOwnerID, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });



                // Code By:Kashif Shahzad
                foreach (var item in project.ToList())
                {
                    var Login = await FirstOrDefaultAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.Supervisor);
                    if (Login == null)
                    {
                        project.Remove(item);
                    }
                }
                // End Code.....



                return Ok(project.ToList());
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
                var fleetOwnerID = await GetAssociatedId();
                var project = await SelectAsync<Supervisor, DropdownModel>(x => !x.IsDeleted && x.Status == SupervisorStatus.Approved && x.FleetOwnerId == fleetOwnerID, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                return Ok(project.ToList());
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
        //        return Ok(!await AnyAsync<Supervisor>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower()));
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
                return Ok(!await AnyAsync<Supervisor>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<CorporateUser>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<FleetOwner>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Installer>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Crew>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<User>(x => x.EmailAddress.ToLower() == email.ToLower())
                    );
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpGet("GetByAssociatedId/{id}")]
        public async Task<IActionResult> GetByAssociatedId(int id)
        {
            try
            {
                var result = await FirstOrDefaultAsync<Login>(x => x.AssociatedId == id && x.Role == UserRoles.Supervisor);
                if (result is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new LoginModel
                    {
                        Id = result.Id,
                        Username = result.UserName,
                        Password = result.Password,
                        AssociatedId = result.AssociatedId,
                        IsActive = result.IsActive
                    });
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        #region Approve Jobs
        [HttpPost("GetJobsWaitingForApproval")]
        public async Task<IActionResult> GetJobsWaitingForApproval([FromBody]SupervisorPaginationModel model)
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var _association = await ToListAsync <SupervisorSiteAssociation>(x => x.SupervisorId == AssociatedId);
                Expression<Func<Job, bool>> filter = x => x.Status == JobStatus.UnderReviewSupervisor  && !x.IsDeleted
                
                //CB:KS

                &&(model.LPN==""||x.LPN==model.LPN)
                &&(model.VIN==""||x.VIN==model.VIN)
                //EC
                
                
                
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
                    var site = await LastOrDefaultAsync<Site>(i => i.Id == x.SiteId && !i.IsDeleted && (model.LastName=="" || i.Name.ToLower().Contains(model.LastName.ToLower()))  );
                    if (site!=null)
                    {
                        x.SiteName = site.Name;
                    }
                    else if (!string.IsNullOrEmpty(model.LastName))
                    {
                        list.MainData.Remove(x);
                    }
                    var bid = await LastOrDefaultAsync<Bid>(i => i.SiteId == x.SiteId && !i.IsDeleted);
                    var installer = await LastOrDefaultAsync<Installer>(i => i.Id == bid.InstallerId && !i.IsDeleted  && (model.FirstName=="" || (i.FirstName+""+i.LastName).ToLower().Contains(model.FirstName.ToLower())));
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
                    var site_log = await LastOrDefaultAsync<SiteStatusLog>(i => i.SiteId == x.SiteId && (model.AssignDate==null || i.CreatedDate.Date==model.AssignDate.GetValueOrDefault().Date) );
                    if (site_log!=null)
                    {
                        x.AssignedDate = site_log.CreatedDate;
                    }
                    else if (!(model.AssignDate==null))
                    {
                        list.MainData.Remove(x);
                    }
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
                    try {
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
                    catch(Exception ex)
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
        [HttpGet("SiteDetails/{id}")]
        public async Task<IActionResult> SiteDetails(int Id)
        {
            try
            {
                var data = await ToListAsync<SupervisorSiteAssociation>(x => x.SupervisorId == Id);
                var sites = new List<SiteModel>();

                if (data != null)
                {
                    var model = new Project();
                    foreach (var item in data)
                    {
                        var site = new SiteModel();
                        var result = await FirstOrDefaultAsync<Site>(x => x.Id == item.SiteId);
                        if (result != null)
                        {
                            model = await FirstOrDefaultAsync<Project>(x => x.Id == result.ProjectId);
                            site.Name = result.Name;
                            site.PONo = result.PONo;
                            site.ProjectName = model.Name;
                            site.SiteStatus = result.SiteStatus;
                            site.TotalVehicleTypes = await CountAsync<SiteVehicleType>(y => y.SiteId == result.Id && !y.IsDeleted);
                            site.TotalVehicles = await CountAsync<Job>(y => y.SiteId == result.Id && !y.IsDeleted);
                        }


                        sites.Add(site);
                    }
                    return Ok(sites);
                }

                return NotFound("Not found");

            }
            catch (Exception ex)
            {

                return MethodFailure(ex);
            }
        }
        

        [HttpPost("SiteListLog")]
        public async Task<IActionResult> SiteLog(SitePaginationModel paginationModel)
        {
            try
            {
                var SupervisorID = await GetAssociatedId();
                Expression<Func<SupervisorSiteAssociation, bool>> filter = x => x.SupervisorId == SupervisorID;
                var joinResult = Entities.SupervisorSiteAssociation.Where(filter).Join(Entities.Site, a => a.SiteId, s => s.Id,
                    (a, b) => new
                    {
                        ProjectId = b.ProjectId,
                        Id = b.Id,
                        PONo = b.PONo,
                        SiteStatus = b.SiteStatus,
                        FleetOwnerId = b.FleetOwnerId,
                        SiteName = b.Name

                    }).Where(x => (paginationModel.SiteName == "" || x.SiteName.ToLower().Contains(paginationModel.SiteName.ToLower()))
                    && ( paginationModel.PO == "" || x.PONo == paginationModel.PO)
                    && ( paginationModel.SiteStatus == null || (int)x.SiteStatus == paginationModel.SiteStatus))
                   .OrderByDescending(x => x.Id).Join(Entities.Project, x => x.ProjectId, j => j.Id,
                    (a, b) => new
                    {
                        Id = a.Id,
                        SiteName = a.SiteName,
                        ProjectName = b.Name,
                        FleetOwnerId = b.FleetOwnerId,
                        ProjectId = a.ProjectId,
                        PONo = a.PONo,
                        SiteStatus = a.SiteStatus,
                    }).Where(p => paginationModel.ProjectName == "" || p.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                    .Join(Entities.FleetOwner, x => x.FleetOwnerId, f => f.Id,
                (a, f) => new SiteModel
                {
                    Id = a.Id,
                    Name = a.SiteName,
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    FleetOwnerId = a.FleetOwnerId,
                    FleetOwnerName = f.FirstName + " " + f.LastName,
                    PONo = a.PONo,
                    SiteStatus = a.SiteStatus,
                });


                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);
                foreach (var x in result)
                {
                    x.TotalVehicleTypes = await CountAsync<SiteVehicleType>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.TotalVehicles = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted);
                }
                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        // Job Log

        [HttpPost("JobList")]
        public async Task<IActionResult> GetJobLog(CrewJobPaginationModel paginationModel)
        {
            try
            {
                var supId = await GetAssociatedId();
                Expression<Func<SupervisorSiteAssociation, bool>> filter1 = x =>
                  x.SupervisorId == supId
                  && !x.IsDeleted;
                Expression<Func<Job, bool>> filter = x =>
                    (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && (paginationModel.JobStatus == null || (int)x.Status == paginationModel.JobStatus)
                    && !x.IsDeleted;

                var joinResult = from j in Entities.SupervisorSiteAssociation.Where(filter1)
                                 join cs in Entities.Job
                                       on j.SiteId equals cs.SiteId
                                 join s in Entities.Site
                                       on cs.SiteId equals s.Id
                                 join p in Entities.Project
                                    on s.ProjectId equals p.Id
                                 where s.IsDeleted == false

                                 select new CrewSiteModel
                                 {
                                     ProjectName = p.Name,
                                     SiteName = s.Name,
                                     VehicleTypeName = string.Empty,
                                     VIN = cs.VIN,
                                     LPN = cs.LPN,
                                     UnitNo = cs.UnitNo,
                                     Status = cs.Status,
                                     SiteId = s.Id,
                                     CrewId = j.SupervisorId,
                                     JobId = cs.Id,
                                     StartDate = null
                                 };




                var result = (joinResult
                  .Where(c =>
                         (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
                      && (paginationModel.SiteName == "" || c.SiteName.ToLower().Contains(paginationModel.SiteName.ToLower()))
                      && (paginationModel.ProjectName == "" || c.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                      && (paginationModel.VIN == "" || c.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                      && (paginationModel.LPN == "" || c.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                      && (paginationModel.JobStatus == null || (int)c.Status == paginationModel.JobStatus))
                  ).ToList();
                 var count =  result.Count();
                 result =  result.GetPage(paginationModel);

                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        #endregion
    }
}
