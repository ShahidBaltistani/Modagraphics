using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;
using Microsoft.EntityFrameworkCore;
using Database;

namespace WebAPI.Controllers
{
    public class CrewController : BaseController
    {
        public CrewController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet("GetVehiclesAgainstAssignedSites/{Id}")]
        public async Task<IActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var joinresult = Entities.VehicleTypeSiteAssociation.Where(x => x.SiteId == Id && !x.IsDeleted).Join(Entities.VehicleTypes, x => x.VehicleTypeId, v => v.Id,
                (a, b) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    VehicleTypeName = b.Name,
                    VehiclePrice = b.BasePrice,
                    SideHeight = a.SideHeight,
                    SideWidth = a.SideWidth,
                    FrontHeight = a.FrontHeight,
                    FrontWidth = a.FrontWidth,
                    RearHeight = a.RearHeight,
                    RearWidth = a.RearWidth
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


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var crew = await ToListAsync<Crew>(x => !x.IsDeleted);
                return Ok(crew.Select(x =>
                    new CrewModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        PhoneNo = x.PhoneNo,
                        Address = x.Address,
                        CityId = x.CityId,
                        ZipCode = x.ZipCode,
                        InstallerId = x.InstallerId,
                        Picture = x.Picture
                    }).ToList());
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
                var crew = await FirstOrDefaultAsync<Crew>(model => model.Id == id);
                return crew == null ? (IActionResult)NotFound() : Ok(new CrewModel
                {
                    Id = crew.Id,
                    FirstName = crew.FirstName,
                    LastName = crew.LastName,
                    EmailAddress = crew.EmailAddress,
                    PhoneNo = crew.PhoneNo,
                    Address = crew.Address,
                    CityId = crew.CityId,
                    ZipCode = crew.ZipCode,
                    InstallerId = crew.InstallerId,
                    Picture = crew.Picture
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(CrewPaginationModel paginationModel)
        {
            try
            {
                var InstallerId = await GetAssociatedId();


                Expression<Func<Crew, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.PhoneNo == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNo.ToLower()))
                    && !x.IsDeleted
                    
                    && x.InstallerId==InstallerId;

                var joinResult = Entities.Crew.Where(filter).Join(Entities.Installer, cr => cr.InstallerId, inst => inst.Id
                , (cr, inst) => new CrewModel
                {
                    Id = cr.Id,
                    CrewUserName = "",
                    FirstName = cr.FirstName,
                    LastName = cr.LastName,
                    EmailAddress = cr.EmailAddress,
                    PhoneNo = cr.PhoneNo
                });
                    
                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var item in result.ToList())
                {
                    item.TotalSite = await CountAsync<CrewSite>(y => y.CrewId == item.Id);
                    item.IsLoginExists = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.Crew);
                    var Login = await FirstOrDefaultAsync<Login>(x=>x.AssociatedId==item.Id && x.Role==UserRoles.Crew && x.AssociatedId==item.Id && ( paginationModel.CrewUserName=="" || x.UserName.ToLower().Contains(paginationModel.CrewUserName.ToLower())) );
                    if (Login!=null)
                    {
                        item.CrewUserName = Login.UserName;
                    }
                    else if(!string.IsNullOrEmpty(paginationModel.CrewUserName) )
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
        public async Task<IActionResult> Post([FromBody]CrewModel crew)
        {
            try
            {
                var InstallerId = await GetAssociatedId();
                var vCrew = new Crew
                {
                    Id = crew.Id,
                    FirstName = crew.FirstName,
                    LastName = crew.LastName,
                    EmailAddress = crew.EmailAddress,
                    PhoneNo = crew.PhoneNo,
                    Address = crew.Address,
                    CityId = crew.CityId,
                    ZipCode = crew.ZipCode,
                    //InstallerId = crew.InstallerId,
                    InstallerId = InstallerId,
                    Picture = crew.Picture,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vCrew);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]CrewModel crew)
        {
            try
            {
                if (!await AnyAsync<Crew>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vCrew = Entities.Crew.FirstOrDefault(model => model.Id == id);
                vCrew.FirstName = crew.FirstName;
                vCrew.LastName = crew.LastName;
                vCrew.EmailAddress = crew.EmailAddress;
                vCrew.PhoneNo = crew.PhoneNo;
                vCrew.Address = crew.Address;
                vCrew.CityId = crew.CityId;
                vCrew.ZipCode = crew.ZipCode;
                vCrew.InstallerId = crew.InstallerId;
                vCrew.Picture = crew.Picture;
                vCrew.IsDeleted = false;
                vCrew.ModifiedBy = UserClaims.UserId;
                vCrew.ModifiedDate = UserClaims.DateTime;
                vCrew.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vCrew);
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
                var crew = await FirstOrDefaultAsync<Crew>(x => x.Id == id);
                if (crew is null) return NotFound();
                crew.IsDeleted = true;
                crew.ModifiedBy = UserClaims.UserId;
                crew.ModifiedDate = UserClaims.DateTime;
                crew.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(crew);
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
                var InstallerID = await GetAssociatedId();
                var project = await SelectAsync<Crew, DropdownModel>(x => !x.IsDeleted && x.InstallerId == InstallerID, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + "  " + x.LastName
                });
                return Ok(project.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpGet("DropdownForAssigningSite")]
        public async Task<IActionResult> DropdownForAssigningSite()
        {
            try
            {
                var InstallerID = await GetAssociatedId();
                var project = await SelectAsync<Crew, DropdownModel>(x => !x.IsDeleted && x.InstallerId == InstallerID, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + "  " + x.LastName
                });
                // Code By : Kashif Shahzad

                foreach (var item in project.ToList())
                {
                    var Login = await FirstOrDefaultAsync<Login>(x=>x.AssociatedId==item.Id && x.Role==UserRoles.Crew);
                    if (Login==null)
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

        //[HttpGet("IsUniqueEmail/{id}/{email}")]
        //public async Task<IActionResult> VerifyEmail(uint id, string email)
        //{
        //    try
        //    {
        //        return Ok(!await AnyAsync<Crew>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower()));
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
                return Ok(!await AnyAsync<Crew>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower())

                    && !await AnyAsync<CorporateUser>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<FleetOwner>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<User>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Installer>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Supervisor>(x => x.EmailAddress.ToLower() == email.ToLower())
                    );
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("CrewJobsInQue")]
        public async Task<IActionResult> PostCrewJob(CrewJobPaginationModel paginationModel)
        {
            try
            {
                var crewId = await GetAssociatedId();
                Expression<Func<Job, bool>> filter = x =>
                    (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && !x.IsDeleted
                    && (x.Status == JobStatus.InQueue);
                
                var joinResult = from j in Entities.Job.Where(filter)
                              join s in Entities.Site
                                    on j.SiteId equals s.Id
                              join cs in Entities.CrewSites
                                    on s.Id equals cs.SiteId
                              where s.IsDeleted == false && s.Id == paginationModel.SiteId
                              && s.SiteStatus == SiteStatus.Assigned
                              && cs.CrewId == crewId
                                 select new CrewSiteModel
                              {
                                  VehicleTypeName = string.Empty,
                                  VIN = j.VIN,
                                  LPN = j.LPN,
                                  UnitNo = j.UnitNo,
                                  Status = j.Status,
                                  SiteId = s.Id,
                                  CrewId = cs.CrewId,
                                  JobId = j.Id,
                                  StartImgCount=0,
                                  EndImgCount=0
                                 };
                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                    item.StartImgCount = await CountAsync<JobImages>(x => x.JobId == item.JobId && x.Status == JobImagesStatus.IsStart);
                    item.EndImgCount = await CountAsync<JobImages>(x => x.JobId == item.JobId && x.Status == JobImagesStatus.IsEnd);
                }

                result = result.Where(c => paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower())).ToList();

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("CrewJobsInProcess")]
        public async Task<IActionResult> PostCrewJobInProcess(CrewJobPaginationModel paginationModel)
        {
            try
            {
                var crewId = await GetAssociatedId();
                Expression<Func<Job, bool>> filter = x =>
                    (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && !x.IsDeleted
                    && x.Status == JobStatus.InProcess;

                var joinResult = from j in Entities.Job.Where(filter)
                                 join cs in Entities.CrewSites
                                       on j.SiteId equals cs.SiteId
                                 join s in Entities.Site
                                       on cs.SiteId equals s.Id
                                 //join f in Entities.FleetOwner
                                 //   on s.FleetOwnerId equals f.Id
                                 join p in Entities.Project
                                    on s.ProjectId equals p.Id
                                 where s.IsDeleted == false
                                 && s.SiteStatus == SiteStatus.Assigned
                                 && cs.CrewId == crewId

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
                                     CrewId = cs.CrewId,
                                     JobId = j.Id,
                                     StartDate = null
                                 };
                

               // var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                    item.StartDate = (await FirstOrDefaultAsync<JobImages>(x => x.JobId == item.JobId && x.Status == JobImagesStatus.IsStart))?.CreatedDate;
                }

                result = result.Where(c => (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
                    && (paginationModel.ProjectName == "" || c.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                    && (paginationModel.SiteName == "" || c.SiteName.ToLower().Contains(paginationModel.SiteName.ToLower()))

                    &&(paginationModel.UnitNo==0 || c.UnitNo==paginationModel.UnitNo)
                    &&(paginationModel.StartedDate==null || c.StartDate.GetValueOrDefault().Date==paginationModel.StartedDate.GetValueOrDefault().Date)

                    ).ToList();
                var count =  result.Count();

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("JobsInReview")]
        public async Task<IActionResult> PostCrewJobReview(CrewJobReviewPaginationModel paginationModel)
        {
            try
            {
                var crewId = await GetAssociatedId();
                Expression<Func<Job, bool>> filter = x =>
                    (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && !x.IsDeleted 
                    && (x.Status == JobStatus.UnderReviewApprovalManager || x.Status == JobStatus.UnderReviewSupervisor);
                Expression<Func<Site, bool>> filter_Site = x =>
                     (paginationModel.SiteName == "" || x.Name.ToLower().Contains(paginationModel.SiteName.ToLower()))
                     && !x.IsDeleted;

                var joinQuery = Entities.Site.Where(filter_Site).Join(Entities.Project, s => s.ProjectId, p => p.Id,
                               (s, p) => new { s, p })
                               .Join(Entities.Job.Where(filter), xsite => xsite.s.Id, j => j.SiteId, (xvt, j) => new { xvt, j })
                               .Join(Entities.CrewSites.Where(x => x.CrewId == crewId), xj => xj.j.SiteId, cs => cs.SiteId, (xj, cs) => new { xj, cs})
                               .Select(x => new CrewJobSiteStatusModel
                               {
                                   SiteId = x.xj.xvt.s.Id,
                                   Project = x.xj.xvt.p.Name,
                                   Site = x.xj.xvt.s.Name,
                                   VehicleTypeName = string.Empty,
                                   VIN = x.xj.j.VIN,
                                   LPN = x.xj.j.LPN,
                                   JobId = x.xj.j.Id,
                                   Status = x.xj.j.Status,
                                   UnitNo=x.xj.j.UnitNo // Code By : Kashif Shahzad
                               });


                var JobStart = Entities.JobImages.Where(x => x.Status == JobImagesStatus.IsStart).GroupBy(x => new { x.JobId, x.CreatedBy }).Select(js => new { js.Key.JobId, StartDate = js.Min().CreatedDate});
                var joinResult = joinQuery.Join(JobStart, j => j.JobId, ji => ji.JobId,
                    (j, ji) => new CrewJobSiteStatusModel {
                        SiteId = j.SiteId,
                        Project = j.Project,
                        Site = j.Site,
                        VehicleTypeName = j.VehicleTypeName,
                        VIN = j.VIN,
                        LPN = j.LPN,
                        UnitNo = j.UnitNo,
                        JobId = j.JobId,
                        Status = j.Status,
                        StartDate = ji.StartDate,
                        EndDate = null
                    }).Where(c => (paginationModel.ProjectName == "" || c.Project.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                    && (paginationModel.SiteName == "" || c.Site.ToLower().Contains(paginationModel.SiteName.ToLower()))
                    );
                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);
                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                    item.EndDate = (await LastOrDefaultAsync<JobImages>(x => x.JobId == item.JobId && x.Status==JobImagesStatus.IsEnd))?.CreatedDate;
                }

                result = result.Where(c => 
                
                (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
                &&(paginationModel.UnitNo==0 || c.UnitNo==paginationModel.UnitNo)
                &&(paginationModel.StartedDate==null || c.StartDate.GetValueOrDefault().Date==paginationModel.StartedDate.GetValueOrDefault().Date)
                &&(paginationModel.CompletedDate==null || c.EndDate.GetValueOrDefault().Date==paginationModel.CompletedDate.GetValueOrDefault().Date)
                
                ).ToList();

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("JobsCompleted")]
        public async Task<IActionResult> PostCrewCompletedJobs(CrewCompletedJobsPaginationModel paginationModel)
        {
            try
            {
                var crewId = await GetAssociatedId();
                Expression<Func<Job, bool>> filter = x =>
                    (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && !x.IsDeleted
                    && x.Status == JobStatus.Completed;
                Expression<Func<Site, bool>> filter_Site = x =>
                     (paginationModel.SiteName == "" || x.Name.ToLower().Contains(paginationModel.SiteName.ToLower()))
                     && !x.IsDeleted;
                var joinQuery1 = Entities.Site.Where(filter_Site).Join(Entities.Project, s => s.ProjectId, p => p.Id,
                              (s, p) => new { s, p })
                              .Join(Entities.Job.Where(filter), xsite => xsite.s.Id, j => j.SiteId, (xvt, j) => new { xvt, j })
                              .Join(Entities.CrewSites.Where(x => x.CrewId == crewId), xj => xj.j.SiteId, cs => cs.SiteId, (xj, cs) => new { xj, cs })
                              .Select(x => new CrewJobCompletedStatusModel
                              {
                                  SiteId = x.xj.xvt.s.Id,
                                  Project = x.xj.xvt.p.Name,
                                  Site = x.xj.xvt.s.Name,
                                  VehicleTypeName = string.Empty,
                                  VIN = x.xj.j.VIN,
                                  LPN = x.xj.j.LPN,
                                  JobId = x.xj.j.Id,
                                  Status = x.xj.j.Status,

                                  UnitNo = x.xj.j.UnitNo // Code By : Kashif Shahzad
                               }).ToList();
                var joinQuery = Entities.Site.Where(filter_Site).Join(Entities.Project, s => s.ProjectId, p => p.Id,
                               (s, p) => new { s, p })
                               .Join(Entities.Job.Where(filter), xsite => xsite.s.Id, j => j.SiteId, (xvt, j) => new { xvt, j })
                               .Join(Entities.CrewSites.Where(x => x.CrewId == crewId), xj => xj.j.SiteId, cs => cs.SiteId, (xj, cs) => new { xj, cs})
                               .Select(x => new CrewJobCompletedStatusModel
                               {
                                   SiteId = x.xj.xvt.s.Id,
                                   Project = x.xj.xvt.p.Name,
                                   Site = x.xj.xvt.s.Name,
                                   VehicleTypeName = string.Empty,
                                   VIN = x.xj.j.VIN,
                                   LPN = x.xj.j.LPN,
                                   JobId = x.xj.j.Id,
                                   Status = x.xj.j.Status,
                                   StartDate=null,
                                   EndDate = null,
                                   ApproveDate = null,

                                   UnitNo =x.xj.j.UnitNo // Code By : Kashif Shahzad
                               }).Where(c => (paginationModel.ProjectName == "" || c.Project.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                    && (paginationModel.SiteName == "" || c.Site.ToLower().Contains(paginationModel.SiteName.ToLower()))
                    ); ;

              //  var JobStart = Entities.JobImages.Where(x => x.Status == JobImagesStatus.IsStart && x.JobId== ).GroupBy(x => new { x.JobId, x.CreatedBy }).Select(js => new { js.Key.JobId, StartDate.Date = js.Min().CreatedDate });
                var JobStart1 = Entities.JobImages.Where(x => x.Status == JobImagesStatus.IsStart).GroupBy(x => new { x.JobId, x.CreatedBy }).Select(js => new { js.Key.JobId, StartDate = js.Min().CreatedDate }).ToList();
                //var joinResult = joinQuery.Join(JobStart, j => j.JobId, ji => ji.JobId,
                //    (j, ji) => new CrewJobCompletedStatusModel
                //    {
                //        SiteId = j.SiteId,
                //        Project = j.Project,
                //        Site = j.Site,
                //        VehicleTypeName = j.VehicleTypeName,
                //        VIN = j.VIN,
                //        LPN = j.LPN,
                //        UnitNo = j.UnitNo,
                //        JobId = j.JobId,
                //        Status = j.Status,
                //        StartDate = ji.StartDate,
                //        EndDate = null,
                //        ApproveDate = null
                //    });
                var count = await joinQuery.CountAsync();
                var result = await joinQuery.GetPageAsync(paginationModel);
               
                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                    item.EndDate = (await LastOrDefaultAsync<JobImages>(x => x.JobId == item.JobId && x.Status == JobImagesStatus.IsEnd))?.CreatedDate;
                    item.StartDate = (await FirstOrDefaultAsync<JobImages>(x => x.JobId == item.JobId && x.Status == JobImagesStatus.IsStart))?.CreatedDate;
                        //Entities.JobImages.Where(x=> x.Status == JobImagesStatus.IsStart && x.JobId == item.JobId).OrderBy(x=> x.CreatedDate).Select(x=> x.CreatedDate).FirstOrDefault();
                    // Code By : Kashif Shahzad
                    item.ApproveDate = (await LastOrDefaultAsync<JobStatusLogs>(x => x.JobId == item.JobId && x.Status == JobStatus.Completed))?.CreatedDate;
                    // End
                }

                result = result.Where(c => 
                
                (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
                
                // Code By : Kashif Shahzad
                &&(paginationModel.UnitNo==0 || c.UnitNo==paginationModel.UnitNo)
                // End Code
                
                ).ToList();

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("crewJobImage")]
        public async Task<IActionResult> SaveCrewJobImage([FromBody]JobImagesModel vJImage)
        {
            try
            {
                var job = await FirstOrDefaultAsync<Job>(x => x.Id == vJImage.JobId);
                if (job is null)
                {
                    return NotFound();
                }
                var site = await FirstOrDefaultAsync<Site>(x => x.Id == vJImage.SiteId);
                if (site is null)
                {
                    return NotFound();
                }
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        if (vJImage.ImagesDeleteStatus == JobImagesStatus.IsDeleted)
                        {
                            var JobImagesOld = await ToListAsync<JobImages>(x => x.JobId == vJImage.JobId);

                            foreach (var item in JobImagesOld.Where(x => x.IsDeleted == false))
                            {
                                item.Status = JobImagesStatus.IsDeleted;
                                await UpdateAsync(item);
                            }
                        }
                        job.Status= vJImage.Status;
                        //site log will also needs to enter when we update sitestatus
                        //job.Status = vJImage.IsStartingPicture ? JobStatus.InProcess : JobStatus.Completed;
                        var vJobImages = new JobImages
                        {
                            Id = vJImage.Id,
                            JobId = vJImage.JobId,
                            CrewId = vJImage.CrewId,
                            Status = vJImage.jobImagesStatus,
                            Picture = vJImage.Picture,
                            IsDeleted = false,
                            CreatedBy = UserClaims.UserId,
                            CreatedDate = UserClaims.DateTime,
                            TZOSCreatedBy = UserClaims.TimeZoneOffset,
                        };
                        await InsertAsync(vJobImages);

                        // Code for Job-Status-Log ---By :Kashif Shahzad
                        if (vJImage.jobImagesStatus==JobImagesStatus.IsStart)
                        {
                            var log = new JobStatusLogs
                            {
                                Id = 0,
                                JobId = vJImage.JobId,
                                Status = JobStatus.InProcess,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync(log);
                        }
                        else if(vJImage.jobImagesStatus == JobImagesStatus.IsEnd)
                        {
                            var log = new JobStatusLogs
                            {
                                Id = 0,
                                JobId = vJImage.JobId,
                                Status = JobStatus.UnderReviewApprovalManager,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync(log);
                        }
                        else if (vJImage.jobImagesStatus == JobImagesStatus.IncidentReported)
                        {
                            var log = new JobStatusLogs
                            {
                                Id = 0,
                                JobId = vJImage.JobId,
                                Status = JobStatus.IncidentReported,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync(log);
                        }                       
                        // Code for Job-Status-Log


                        tran.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("AssignedSitesList")]
        public async Task<IActionResult> GetAssignedSites([FromBody]CrewPaginationModel model)
        {
            try
            {
                var crewID = await GetAssociatedId();
                Expression<Func<CrewSite, bool>> filter = x => x.CrewId == crewID;

                var joinResult = Entities.CrewSites.Where(filter).Join(Entities.Site, c => c.SiteId, s => s.Id,
                    (a, b) => new {
                        a.CrewId,
                        a.SiteId,
                        a.CreatedDate,
                        b.Name,
                        b.ProjectId,
                    }).Join(Entities.Project, x=> x.ProjectId, p=>p.Id,(a,b) => new BidModel {
                        SiteId = a.SiteId,
                        SiteName = a.Name,
                        ProjectName = b.Name,
                        AssignDate = a.CreatedDate
                    }).Where(z=>
                    
                    (model.FirstName=="" || z.ProjectName.ToLower().Contains(model.FirstName.ToLower()))
                    && (model.LastName=="" || z.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    &&(model.AssignedDate==null || z.AssignDate.Date==model.AssignedDate.GetValueOrDefault().Date)
                    );
                var count = await joinResult.CountAsync();
                var page = await joinResult.GetPageAsync(model);

                foreach (var x in page)
                {
                    x.SiteVehicals = await CountAsync<Job>(y => y.SiteId == x.SiteId &&  !y.IsDeleted);
                    x.SiteVehicalsInQue = await CountAsync<Job>(y => y.SiteId == x.SiteId && y.Status==JobStatus.InQueue && !y.IsDeleted);
                }

                return Ok(page, count);
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var userCrew = await FirstOrDefaultAsync<Crew>(model => model.Id == id);
                return userCrew == null ? (IActionResult)NotFound() : Ok(userCrew.Picture);
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
                var result = await FirstOrDefaultAsync<Login>(x => x.AssociatedId == id && x.Role == UserRoles.Crew);
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



    }
}
