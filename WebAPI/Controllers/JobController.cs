using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    public class JobController : BaseController
    {
        public JobController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        [Route("GetImageForJobsInProcess/{id}")]
        public IActionResult GetImage(int id)
        {
            try
            {
                Expression<Func<Job, bool>> filter = x => x.Id == id && !x.IsDeleted;
                var joinresult = Entities.Job.Where(filter).Join(Entities.JobImages.Where(x => x.Status != JobImagesStatus.IncidentReported && x.Status != JobImagesStatus.IsDeleted).Select(x => x), j => j.Id, ji => ji.JobId,
                (a, b) => new JobImages
                {
                   
                    Picture = b.Picture,
                });
                
                return Ok(joinresult);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        //[HttpGet("IsUniqueName/{vin}/{siteid}")]
        //public async Task<IActionResult> VerifyVIN(string vin,string siteid)
        //{
        //    try
        //    {
        //        return Ok(!await AnyAsync<Job>(x => x.VIN.ToLower() == vin.ToLower() && x.SiteId.ToString()==siteid));
        //    }
        //    catch (Exception ex)
        //    {
        //        return MethodFailure(ex);
        //    }
        //}
        [HttpGet("IsUniqueName/{id}/{SiteId}/{VIN}")]
        public async Task<IActionResult> VerifyVIN(uint id, uint siteid,string vin)
        {
            try
            {
                return Ok(!await AnyAsync<Job>(x => x.Id != id && x.SiteId==siteid && x.VIN==vin));
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var job = await ToListAsync<Job>(x => !x.IsDeleted);
                return Ok(job.Select(x =>
                    new JobModel
                    {
                        Id = x.Id,
                        SiteId = x.SiteId,
                        VIN = x.VIN,
                        LPN = x.LPN,
                        UnitNo = x.UnitNo,
                        Status = x.Status,
                        SiteVehicleTypeId = x.SiteVehicleTypeId
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
                var job = await FirstOrDefaultAsync<Job>(model => model.Id == id);
                return job == null ? (IActionResult)NotFound() : Ok(new JobModel
                {
                    Id = job.Id,
                    SiteId = job.SiteId,
                    VIN = job.VIN,
                    LPN = job.LPN,
                    UnitNo = job.UnitNo,
                    Status = job.Status,
                    SiteVehicleTypeId = job.SiteVehicleTypeId
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(JobPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Job, bool>> filter = x =>
                   (paginationModel.SiteVehicleTypeId == 0 || x.SiteVehicleTypeId == paginationModel.SiteVehicleTypeId)
                    && (paginationModel.VIN == "" || x.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                    && (paginationModel.LPN == "" || x.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                    && (paginationModel.UnitNo == 0 || x.UnitNo.ToString().Contains(paginationModel.UnitNo.ToString()))
                    && (paginationModel.JobStatus == null ||(int) x.Status == paginationModel.JobStatus)
                    && !x.IsDeleted;

                var jobs = await ToListAsync(filter);

                return Ok(jobs.OrderByDescending(x => x.Id).Select(x =>
                    new JobModel
                    {
                        Id = x.Id,
                        VIN = x.VIN,
                        LPN = x.LPN,
                        UnitNo = x.UnitNo,
                        Status = x.Status
                    }).GetPage(paginationModel), jobs.Count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]JobModel job)
        {
            try
            {
                var vJob = new Job
                {
                    Id = job.Id,
                    SiteId = job.SiteId,
                    VIN = job.VIN,
                    LPN = job.LPN,
                    UnitNo = job.UnitNo,
                    Status = job.Status,
                    SiteVehicleTypeId = job.SiteVehicleTypeId,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vJob);

                // Code for Job-Status-Log ---By :Kashif Shahzad
                
                var log = new JobStatusLogs
                {
                    Id=0,
                    JobId=vJob.Id,
                    Status=JobStatus.Pending,
                    CreatedBy=UserClaims.UserId,
                    CreatedDate=UserClaims.DateTime,
                    TZOSCreatedBy=UserClaims.TimeZoneOffset
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]JobModel job)
        {
            try
            {
                if (!await AnyAsync<Job>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vJob = Entities.Job.FirstOrDefault(model => model.Id == id);
                vJob.SiteId = job.SiteId;
                vJob.VIN = job.VIN;
                vJob.LPN = job.LPN;
                vJob.UnitNo = job.UnitNo;
                vJob.Status = job.Status;
                vJob.SiteVehicleTypeId = job.SiteVehicleTypeId;
                vJob.IsDeleted = false;
                vJob.ModifiedBy = UserClaims.UserId;
                vJob.ModifiedDate = UserClaims.DateTime;
                vJob.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vJob);
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
                var job = await FirstOrDefaultAsync<Job>(x => x.Id == id);
                if (job is null) return NotFound();
                job.IsDeleted = true;
                job.ModifiedBy = UserClaims.UserId;
                job.ModifiedDate = UserClaims.DateTime;
                job.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
    }
}
