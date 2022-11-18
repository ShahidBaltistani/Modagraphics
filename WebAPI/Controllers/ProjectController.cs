using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class ProjectController : BaseController
    {
        public ProjectController(IHttpContextAccessor httpContext) : base(httpContext) { }

        // GET: <controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var project = await ToListAsync<Project>(x => !x.IsDeleted);
                return Ok(project.Select(x =>
                    new ProjectModel
                    {
                        Id = x.Id,
                        FleetOwnerId = x.FleetOwnerId,
                        Name = x.Name,
                        IsPoolVehicle = x.IsPoolVehicle,
                        StatusByFleetOwner = x.StatusByFleetOwner,
                        Status = x.Status,
                    }).ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        

        // GET <controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var project = await FirstOrDefaultAsync<Project>(model => model.Id == id);
                return project == null ? (IActionResult)NotFound() : Ok(new ProjectModel
                {
                    Id = project.Id,
                    FleetOwnerId = project.FleetOwnerId,
                    Name = project.Name,
                    IsPoolVehicle = project.IsPoolVehicle,
                    StatusByFleetOwner = project.StatusByFleetOwner,
                    Status = project.Status,
                    Address = project.Address,
                    ContactName = project.ContactName,
                    ContactPhone = project.ContactPhone
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(ProjectPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Project, bool>> filter = x =>
                    (paginationModel.ProjectName == "" || x.Name.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                   // && (((paginationModel.FromDate.ToString() == "1/1/0001 12:00:00 AM") && (paginationModel.ToDate.ToString() == "1/1/0001 12:00:00 AM")) || (paginationModel.FromDate <= x.CreatedDate.Date && paginationModel.ToDate >= x.CreatedDate.Date))
                   && (paginationModel.ProjectContactName == "" || x.ContactName.ToLower().Contains(paginationModel.ProjectContactName.ToLower()))
                    && (paginationModel.ProjectContactPhone == "" || x.ContactPhone.ToLower().Contains(paginationModel.ProjectContactPhone.ToLower()))
                     && (paginationModel.ProjectAddress == "" || x.Address.ToLower().Contains(paginationModel.ProjectAddress.ToLower()))
                    && !x.IsDeleted;

                var joinResult = Entities.Project.Where(filter).OrderByDescending(x => x.Id).Join(Entities.FleetOwner, a => a.FleetOwnerId, b => b.Id
                , (a, b) => new ProjectModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    FleetOwnerId = a.FleetOwnerId,
                    FleetOwnerName = b.FirstName + " " + b.LastName,
                    Status = a.Status,
                    StatusByFleetOwner = a.StatusByFleetOwner,
                    CreatedDate = a.CreatedDate,
                    IsPoolVehicle = a.IsPoolVehicle,
                    Address = a.Address,
                    ContactName = a.ContactName,
                    ContactPhone = a.ContactPhone
                }).Where(f => paginationModel.FleetOwnerName == "" || f.FleetOwnerName.ToLower().Contains(paginationModel.FleetOwnerName.ToLower())); 
                    
                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result)
                {
                    x.TotalSites = await CountAsync<Site>(y => y.ProjectId == x.Id && !y.IsDeleted);
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        // POST <controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProjectModel project)
        {
            try
            {
                var vProject = new Project
                {
                    Id = project.Id,
                    Name = project.Name,
                    FleetOwnerId = project.FleetOwnerId,
                    IsPoolVehicle = project.IsPoolVehicle,
                    StatusByFleetOwner = project.StatusByFleetOwner,
                    Status = project.Status,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                    Address = project.Address,
                    ContactName = project.ContactName,
                    ContactPhone = project.ContactPhone
                };
                await InsertAsync(vProject);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        // PUT <controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ProjectModel project)
        {
            try
            {
                if (!await AnyAsync<Project>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vProject = await FirstOrDefaultAsync<Project>(model => model.Id == id);
                vProject.Name = project.Name;
                vProject.FleetOwnerId = project.FleetOwnerId;
                vProject.IsPoolVehicle = project.IsPoolVehicle;
                vProject.StatusByFleetOwner = project.StatusByFleetOwner;
                vProject.Status = project.Status;
                vProject.IsDeleted = false;
                vProject.ModifiedBy = UserClaims.UserId;
                vProject.ModifiedDate = UserClaims.DateTime;
                vProject.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                vProject.Address = project.Address;
                vProject.ContactName = project.ContactName;
                vProject.ContactPhone = project.ContactPhone;


                await UpdateAsync(vProject);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        // DELETE <controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var project = await FirstOrDefaultAsync<Project>(x => x.Id == id);
                if (project is null) return NotFound();
                project.IsDeleted = true;
                project.ModifiedBy = UserClaims.UserId;
                project.ModifiedDate = UserClaims.DateTime;
                project.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(project);
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
                var project = await SelectAsync<Project, DropdownModel>(x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.Name
                });
                return Ok(project.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Dropdown/{id}")]
        public async Task<IActionResult> DropDown(int id)
        {
            try
            {
                var result = await ToListAsync<Project>(x => x.FleetOwnerId == id && !x.IsDeleted);
                var project = result.Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.Name
                });
                return Ok(project.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("IsUniqueName/{id}/{name}")]
        public async Task<IActionResult> VerifyName(uint id, string name)
        {
            try
            {
                return Ok(!await AnyAsync<Project>(x => x.Id != id && x.Name.ToLower() == name.ToLower()));
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
    }
}
