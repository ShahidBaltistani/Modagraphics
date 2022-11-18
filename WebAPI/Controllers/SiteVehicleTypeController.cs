using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class SiteVehicleTypeController : BaseController
    {
        public SiteVehicleTypeController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var VTAssociation = await ToListAsync<SiteVehicleType>();
                return Ok(VTAssociation.Select(x =>
                    new SiteVehicleTypeModel
                    {
                        Id = x.Id,
                        VehicleTypeId = x.VehicleTypeId,
                        SiteId = x.SiteId,
                        VehicleSpecificationId = x.VehicleSpecificationId,
                        VehiclePrice = x.VehiclePrice,
                        SideHeight = x.SideHeight,
                        SideWidth = x.SideWidth,
                        FrontHeight = x.FrontHeight,
                        FrontWidth = x.FrontWidth,
                        RearHeight = x.RearHeight,
                        RearWidth = x.RearWidth
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
                var VTSpecs = await Entities.VehicleTypeSiteAssociation.Join(Entities.Site, a => a.SiteId, s => s.Id
                , (a, s) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    SiteId = a.SiteId,
                    SiteName = s.Name,
                    VehicleTypeId = a.VehicleTypeId,
                    VehicleSpecificationId = a.VehicleSpecificationId,
                    VehiclePrice = a.VehiclePrice,
                    SideHeight = a.SideHeight,
                    SideWidth = a.SideWidth,
                    FrontHeight = a.FrontHeight,
                    FrontWidth = a.FrontWidth,
                    RearHeight = a.RearHeight,
                    RearWidth = a.RearWidth
                }).Join(Entities.VehicleTypes, a => a.VehicleTypeId, v => v.Id
                , (a, v) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    SiteId = a.SiteId,
                    SiteName = a.SiteName,
                    VehicleTypeId = a.VehicleTypeId,
                    VehicleTypeName = v.Name,
                    VehicleSpecificationId = a.VehicleSpecificationId,
                    VehiclePrice = a.VehiclePrice,
                    SideHeight = a.SideHeight,
                    SideWidth = a.SideWidth,
                    FrontHeight = a.FrontHeight,
                    FrontWidth = a.FrontWidth,
                    RearHeight = a.RearHeight,
                    RearWidth = a.RearWidth
                }).FirstOrDefaultAsync(model => model.Id == id);

                return VTSpecs == null ? (IActionResult)NotFound() : Ok(new SiteVehicleTypeModel
                {
                    Id = VTSpecs.Id,
                    SiteId = VTSpecs.SiteId,
                    SiteName = VTSpecs.SiteName,
                    VehicleTypeId = VTSpecs.VehicleTypeId,
                    VehicleTypeName = VTSpecs.VehicleTypeName,
                    VehicleSpecificationId = VTSpecs.VehicleSpecificationId,
                    VehiclePrice = VTSpecs.VehiclePrice,
                    SideHeight = VTSpecs.SideHeight,
                    SideWidth = VTSpecs.SideWidth,
                    FrontHeight = VTSpecs.FrontHeight,
                    FrontWidth = VTSpecs.FrontWidth,
                    RearHeight = VTSpecs.RearHeight,
                    RearWidth = VTSpecs.RearWidth,
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(VehicleTypeSiteAssociationPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<SiteVehicleType, bool>> filter = x => !x.IsDeleted && (x.SiteId == paginationModel.SiteId);

                var joinResult = Entities.VehicleTypeSiteAssociation.Where(filter).OrderByDescending(x => x.Id).Join(Entities.Site, a => a.SiteId, s => s.Id
                , (a, s) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    SiteId = s.Id,
                    SiteName = s.Name,
                    VehicleTypeId = a.VehicleTypeId,
                    VehicleSpecificationId = a.VehicleSpecificationId
                }).Join(Entities.VehicleTypes, x => x.VehicleTypeId, v => v.Id, (x, v) => new SiteVehicleTypeModel
                {
                    Id = x.Id,
                    SiteId = x.SiteId,
                    SiteName = x.SiteName,
                    VehicleTypeId = x.VehicleTypeId,
                    VehicleTypeName = v.Name,
                    VehicleSpecificationId = x.VehicleSpecificationId
                }).Where(z=>
                    (paginationModel.VehicleType=="" || z.VehicleTypeName.ToLower().Contains(paginationModel.VehicleType.ToLower()))
                    );

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result.ToList())
                {
                    x.TotalVehicles = await CountAsync<Job>(y => y.SiteId == x.SiteId && y.SiteVehicleTypeId == x.Id && !y.IsDeleted);
                    x.JobsCompleted = await CountAsync<Job>(y => y.SiteId == x.SiteId && y.SiteVehicleTypeId == x.Id && !y.IsDeleted && y.Status == JobStatus.Completed);
                    x.JobsInQueue = await CountAsync<Job>(y => y.SiteId == x.SiteId && y.SiteVehicleTypeId == x.Id && !y.IsDeleted && y.Status == JobStatus.InQueue);
                    x.JobsInProgress = await CountAsync<Job>(y => y.SiteId == x.SiteId && y.SiteVehicleTypeId == x.Id && !y.IsDeleted && y.Status == JobStatus.InProcess);
                    //x.VehicleSpecificationName = (await FirstOrDefaultAsync<VehicleTypeSpecifications>(y => y.VehicleTypeId == x.VehicleTypeId))?.Name ?? "0";


                    var VehicleSpecificationName = await FirstOrDefaultAsync<VehicleTypeSpecifications>(y => y.Id == x.VehicleSpecificationId &&
                    (paginationModel.Specification == "" || y.Name.ToLower().Contains(paginationModel.Specification.ToLower()))
                    );
                    //x.VehicleSpecificationName = VehicleSpecificationName == null ? "0" : VehicleSpecificationName.Name;
                    if (VehicleSpecificationName != null)
                    {
                        x.VehicleSpecificationName = VehicleSpecificationName.Name;
                    }
                    else if (!string.IsNullOrEmpty(paginationModel.Specification))
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SiteVehicleTypeModel associationModel)
        {
            try
            {
                var vAssociation = new SiteVehicleType
                {
                    Id = associationModel.Id,
                    VehicleTypeId = associationModel.VehicleTypeId,
                    SiteId = associationModel.SiteId,
                    VehicleSpecificationId = associationModel.VehicleSpecificationId,
                    VehiclePrice = associationModel.VehiclePrice,
                    SideHeight = associationModel.SideHeight,
                    SideWidth = associationModel.SideWidth,
                    FrontHeight = associationModel.FrontHeight,
                    FrontWidth = associationModel.FrontWidth,
                    RearHeight = associationModel.RearHeight,
                    RearWidth = associationModel.RearWidth,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vAssociation);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]SiteVehicleTypeModel associationModel)
        {
            try
            {
                if (!await AnyAsync<SiteVehicleType>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vAssociation = await FirstOrDefaultAsync<SiteVehicleType>(model => model.Id == id);
                vAssociation.VehicleTypeId = associationModel.VehicleTypeId;
                vAssociation.SiteId = associationModel.SiteId;
                vAssociation.VehicleSpecificationId = associationModel.VehicleSpecificationId;
                vAssociation.VehiclePrice = associationModel.VehiclePrice;
                vAssociation.SideHeight = associationModel.SideHeight;
                vAssociation.SideWidth = associationModel.SideWidth;
                vAssociation.FrontHeight = associationModel.FrontHeight;
                vAssociation.FrontWidth = associationModel.FrontWidth;
                vAssociation.RearHeight = associationModel.RearHeight;
                vAssociation.RearWidth = associationModel.RearWidth;
                vAssociation.IsDeleted = false;
                vAssociation.ModifiedBy = UserClaims.UserId;
                vAssociation.ModifiedDate = UserClaims.DateTime;
                vAssociation.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vAssociation);
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
                var association = await FirstOrDefaultAsync<SiteVehicleType>(x => x.Id == id);
                if (association is null) return NotFound();
                association.IsDeleted = true;
                association.ModifiedBy = UserClaims.UserId;
                association.ModifiedDate = UserClaims.DateTime;
                association.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(association);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        //[HttpGet("{siteId}/{vehicleTypeId}")]
        //public async Task<IActionResult> IsRecordExists(uint siteId, uint vehicleTypeId)
        //{
        //    return Ok(await AnyAsync<SiteVehicleType>(x => x.SiteId == siteId && x.VehicleTypeId == vehicleTypeId));
        //}

    }
}
