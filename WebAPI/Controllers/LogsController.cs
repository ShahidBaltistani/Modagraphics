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

    public class LogsController : BaseController
    {
        public LogsController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpPost("Page")]
        public async Task<IActionResult> GetSiteLog(SitePaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Site, bool>> filter = x =>
                (paginationModel.SiteName == "" || x.Name.ToLower().Contains(paginationModel.SiteName.ToLower()))
                && (paginationModel.PO == "" || x.PONo.ToLower().Contains(paginationModel.PO.ToLower()))
                && (paginationModel.SiteStatus == null ||(int) x.SiteStatus == paginationModel.SiteStatus)
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
                }
                result = result
                  .Where(c =>
                     paginationModel.ProjectName == "" || c.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower())
                  ).ToList();
                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }


         [HttpGet("ViewSiteLogDetail/{SiteId}")]
        public async Task<IActionResult> ViewSiteLogDetail(uint SiteId)
        {
            try
            {
                Expression<Func<SiteStatusLog, bool>> filter = x => x.SiteId == SiteId;
                var temp_result = Entities.SiteStatusLog.Where(filter).OrderByDescending(x=>x.CreatedDate).Join(Entities.Site, b => b.SiteId, i => i.Id
              , (s, f) => new SiteStatusLogModel
              {
                  Id = s.Id,
                  SiteName = f.Name,
                  CreatedDate = s.CreatedDate,
                  CreatedBy = s.CreatedBy.ToString(),
                  SiteStatus = s.SiteStatus,
              });
                var result = await temp_result.ToListAsync();
                foreach (var item in result)
                {
                    var user = await Entities.Login.Where(x => x.Id == Convert.ToInt32(item.CreatedBy)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        if (user.Role == 0 || (int)user.Role == 1 || (int)user.Role == 2)
                        {
                            var USER = Entities.User.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 4)
                        {
                            var USER = Entities.Installer.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 5)
                        {
                            var USER = Entities.FleetOwner.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 6)
                        {
                            var USER = Entities.Crew.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 7)
                        {
                            var USER = Entities.Supervisor.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                    }
                   
                }
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("ViewJobLogDetail/{JobId}")]
        public async Task<IActionResult> ViewJobLogDetail(uint JobId)
        {
            try
            {
                Expression<Func<JobStatusLogs, bool>> filter = x => x.JobId == JobId;
                var temp_result = Entities.JobStatusLogs.Where(filter).OrderByDescending(x=>x.CreatedDate).Join(Entities.Job, b => b.JobId, i => i.Id
              , (s, f) => new JobStatusLogModel
              {
                  Id = s.Id,
                  LPN = f.LPN,
                  CreatedDate = s.CreatedDate,
                  CreatedBy = s.CreatedBy.ToString(),
                  JobStatus = s.Status,
              });
                var result = await temp_result.ToListAsync();
                foreach (var item in result)
                {
                    var user = await Entities.Login.Where(x => x.Id == Convert.ToInt32(item.CreatedBy)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        if (user.Role == 0 ||(int) user.Role == 1 || (int)user.Role == 2)
                        {
                            var USER = Entities.User.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 4)
                        {
                            var USER = Entities.Installer.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 5)
                        {
                            var USER = Entities.FleetOwner.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 6)
                        {
                            var USER = Entities.Crew.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 7)
                        {
                            var USER = Entities.Supervisor.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }

                    }

                }
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("ViewJobLogDetailForInstaller/{JobId}")]
        public async Task<IActionResult> ViewJobLogDetailForInstaller(uint JobId)
        {
            try
            {
                Expression<Func<JobStatusLogs, bool>> filter = x => x.JobId == JobId && x.Status != 0;
                var temp_result = Entities.JobStatusLogs.Where(filter).OrderByDescending(x => x.CreatedDate).Join(Entities.Job, b => b.JobId, i => i.Id
              , (s, f) => new JobStatusLogModel
              {
                  Id = s.Id,
                  LPN = f.LPN,
                  CreatedDate = s.CreatedDate,
                  CreatedBy = s.CreatedBy.ToString(),
                  JobStatus = s.Status,
              });
                var result = await temp_result.ToListAsync();
                foreach (var item in result)
                {
                    var user = await Entities.Login.Where(x => x.Id == Convert.ToInt32(item.CreatedBy)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        if (user.Role == 0 || (int)user.Role == 1 || (int)user.Role == 2)
                        {
                            var USER = Entities.User.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 4)
                        {
                            var USER = Entities.Installer.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 5)
                        {
                            var USER = Entities.FleetOwner.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 6)
                        {
                            var USER = Entities.Crew.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }
                        else if ((int)user.Role == 7)
                        {
                            var USER = Entities.Supervisor.Where(x => x.Id == user.AssociatedId).FirstOrDefault();
                            item.CreatedBy = USER.FirstName + " " + USER.LastName;
                        }

                    }

                }
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("JobDetail/{SiteId}")]
        public async Task<IActionResult> JobDetail(int SiteId)
        {
            try
            {
                //var result = await Entities.Job
                //             .Where(x => x.SiteId == SiteId && x.IsDeleted == false).ToListAsync();
                var joinResult = Entities.Job.Where(x => x.SiteId == SiteId && x.IsDeleted == false).Join(Entities.VehicleTypeSiteAssociation, b => b.SiteVehicleTypeId, s => s.Id,
                    (a, b) => new
                    {
                        Id = a.Id,
                        VIN = a.VIN,
                        LPN = a.LPN,
                        UnitNo = a.UnitNo,
                        JobStatus = a.Status,
                        VehicleTypeId = b.VehicleTypeId

                    }).Join(Entities.VehicleTypes, s => s.VehicleTypeId, p => p.Id,
                    (x, y) => new JobModel
                    {
                        Id = x.Id,
                        VIN = x.VIN,
                        LPN = x.LPN,
                        UnitNo = x.UnitNo,
                        Status = x.JobStatus,
                        VehicleName = y.Name,

                    });



                return Ok(joinResult);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost("JobList")]
        public async Task<IActionResult> GetJobLog(CrewJobPaginationModel paginationModel)
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
                                 };


                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                }

                result = result.Where(c => (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
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


    }
}
