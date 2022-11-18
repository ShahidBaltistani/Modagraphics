using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class VehicleController : BaseController
    {
        public VehicleController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var vehicles = await ToListAsync<VehicleType>(x => !x.IsDeleted);
                return Ok(vehicles.Select(x =>
                    new VehicleTypeModel
                    {
                        Id = x.Id,
                        VehicleType = x.Name,
                        BasePrice = x.BasePrice,
                        IsActive = x.IsActive,
                        Specifications = Entities.VehicleTypeSpecifications.Where(model => model.VehicleTypeId == x.Id)
                            .Select(z => new VehicleTypeModel.SpecificationModel()
                            { Id = z.Id, Specification = z.Name, Value = z.Value }).ToList()
                    }).ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet]
        [Route("Image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var vehicleType = await FirstOrDefaultAsync<VehicleType>(model => model.Id == id);
                return vehicleType == null ? (IActionResult)NotFound() : Ok(vehicleType.Image);
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
                var vehicleType = await FirstOrDefaultAsync<VehicleType>(model => model.Id == id);
                return vehicleType == null ? (IActionResult)NotFound() : Ok(new VehicleTypeModel
                {
                    Id = vehicleType.Id,
                    VehicleType = vehicleType.Name,
                    BasePrice = vehicleType.BasePrice,
                    ImageString = vehicleType.Image,
                    IsActive = vehicleType.IsActive,
                    Specifications = Entities.VehicleTypeSpecifications.Where(model => model.VehicleTypeId == id)
                        .Select(z => new VehicleTypeModel.SpecificationModel()
                        { Id = z.Id, Specification = z.Name, Value = z.Value }).ToList()
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(VehicleTypePaginationModel paginationModel)
        {
            try
            {
                Expression<Func<VehicleType, bool>> filter = x =>
                    (paginationModel.VType == "" || x.Name.ToLower().Contains(paginationModel.VType.ToLower()))
                    && (paginationModel.BasePrice == null || paginationModel.BasePrice == "0" || x.BasePrice.ToString().Contains(paginationModel.BasePrice))
                    && (paginationModel.IsAll || x.IsActive == paginationModel.IsActive) && !x.IsDeleted;

                var vehicles = await ToListAsync(filter);

                return Ok(vehicles.OrderByDescending(x => x.Id).Select(x =>
                    new VehicleTypeModel
                    {
                        Id = x.Id,
                        VehicleType = x.Name,
                        BasePrice = x.BasePrice,
                        IsActive = x.IsActive,
                        Specifications = Entities.VehicleTypeSpecifications.Where(model => model.VehicleTypeId == x.Id)
                            .Select(z => new VehicleTypeModel.SpecificationModel()
                            { Id = z.Id, Specification = z.Name, Value = z.Value }).ToList()
                    }).GetPage(paginationModel), vehicles.Count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]VehicleTypeModel vehicle)
        {
            try
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        var vehicleType = new VehicleType
                        {
                            Id = vehicle.Id,
                            Name = vehicle.VehicleType,
                            BasePrice = vehicle.BasePrice,
                            IsActive = vehicle.IsActive,
                            Image = vehicle.ImageString,
                            IsDeleted = false,
                            CreatedBy = UserClaims.UserId,
                            CreatedDate = UserClaims.DateTime,
                            TZOSCreatedBy = UserClaims.TimeZoneOffset,
                        };

                        await InsertAsync(vehicleType);

                        foreach (var specification in vehicle.Specifications)
                            await InsertAsync(new VehicleTypeSpecifications
                            {
                                Id = specification.Id,
                                Name = specification.Specification,
                                Value = specification.Value,
                                VehicleTypeId = vehicleType.Id
                            });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]VehicleTypeModel vehicle)
        {
            try
            {
                if (!await AnyAsync<VehicleType>(model => model.Id == id))
                {
                    return NotFound();
                }

                using (var tran = BeginTransaction())
                {
                    try
                    {
                        var vehicleType = await FirstOrDefaultAsync<VehicleType>(model => model.Id == id);
                        vehicleType.Name = vehicle.VehicleType;
                        vehicleType.BasePrice = vehicle.BasePrice;
                        vehicleType.IsActive = vehicle.IsActive;
                        vehicleType.Image = vehicle.ImageString;
                        vehicleType.ModifiedBy = UserClaims.UserId;
                        vehicleType.ModifiedDate = UserClaims.DateTime;
                        vehicleType.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                        await UpdateAsync(vehicleType);

                        await RemoveRangeAsync(await ToListAsync<VehicleTypeSpecifications>(model => model.VehicleTypeId == id));

                        await InsertRangeAsync(vehicle.Specifications.Select(x => new VehicleTypeSpecifications
                        {
                            Name = x.Specification,
                            Value = x.Value,
                            VehicleTypeId = vehicleType.Id
                        }).ToList());

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vehicleType = await FirstOrDefaultAsync<VehicleType>(x => x.Id == id);
                if (vehicleType is null) return NotFound();
                vehicleType.IsDeleted = true;
                vehicleType.ModifiedBy = UserClaims.UserId;
                vehicleType.ModifiedDate = UserClaims.DateTime;
                vehicleType.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(vehicleType);
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
                var vehicleType = await SelectAsync<VehicleType, DropdownModel>(x => !x.IsDeleted && x.IsActive, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.Name
                });
                return Ok(vehicleType.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("BasePrice/{id}")]
        public async Task<IActionResult> VehicleBasePrice(int id)
        {
            try
            {
                var vehicleBasePrice = await FirstOrDefaultAsync<VehicleType>(x => x.Id == id && !x.IsDeleted && x.IsActive);

                return Ok(vehicleBasePrice.BasePrice);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("DropDownVehicleTypeSpecification/{id}")]
        public async Task<IActionResult> DropDownVehicleTypeSpecification(uint id)
        {
            try
            {
                var result = await ToListAsync<VehicleTypeSpecifications>(x => x.VehicleTypeId == id);
                var vehicleTypeSpecs = result.Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.Name + "  ( " + x.Value.ToString() + " )"
                });
                return Ok(vehicleTypeSpecs.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
    }
}
