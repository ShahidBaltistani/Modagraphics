using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Database.Models;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI.Extension;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    public class CorporateUserController : BaseController
    {
        public CorporateUserController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userCorporation = await ToListAsync<CorporateUser>(model => !model.IsDeleted);


                return Ok(userCorporation.Select(x => new CorporateUserModel
                {
                    FirstName = x.FirstName,
                    Address = x.Address,
                    EmailAddress = x.EmailAddress,
                    CityId = x.CityId,
                    Id = x.Id,
                    LastName = x.LastName,
                    PhoneNo = x.PhoneNo,
                    Status = x.Status,
                    ZipCode = x.ZipCode,
                }));
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
                var userCorporate = await FirstOrDefaultAsync<CorporateUser>(model => model.Id == id);

                return User is null ? (IActionResult)NotFound() : Ok(new CorporateUserModel
                {
                    FirstName = userCorporate.FirstName,
                    Address = userCorporate.Address,
                    EmailAddress = userCorporate.EmailAddress,
                    CityId = userCorporate.CityId,
                    Id = userCorporate.Id,
                    LastName = userCorporate.LastName,
                    PhoneNo = userCorporate.PhoneNo,
                    Status = userCorporate.Status,
                    ZipCode = userCorporate.ZipCode,
                    Picture = userCorporate.Picture
                });
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
                var user = await FirstOrDefaultAsync<CorporateUser>(model => model.Id == id);
                return user is null ? (IActionResult)NotFound() : Ok(user.Picture);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(CorporateUserPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<CorporateUser, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.PhoneNumber == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNumber.ToLower()))
                    && !x.IsDeleted;

                var user = await GetPageAsync(filter, paginationModel, x =>
                    new CorporateUserModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        PhoneNo = x.PhoneNo,
                        Status = x.Status
                    });

                return Ok(user.MainData, user.OtherData);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CorporateUserModel userCorporateModel)
        {
            try
            {
                await InsertAsync(new CorporateUser
                {
                    FirstName = userCorporateModel.FirstName,
                    LastName = userCorporateModel.LastName,
                    Address = userCorporateModel.Address,
                    CityId = userCorporateModel.CityId,
                    EmailAddress = userCorporateModel.EmailAddress,
                    PhoneNo = userCorporateModel.PhoneNo,
                    Picture = userCorporateModel.Picture,
                    Status = userCorporateModel.Status,
                    ZipCode = userCorporateModel.ZipCode,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]CorporateUserModel userCorporateModel)
        {
            try
            {
                if (!await AnyAsync<CorporateUser>(model => model.Id == id))
                {
                    return NotFound();
                }

                var userCoporation = await FirstOrDefaultAsync<CorporateUser>(model => model.Id == id);
                userCoporation.FirstName = userCorporateModel.FirstName;
                userCoporation.LastName = userCorporateModel.LastName;
                userCoporation.Address = userCorporateModel.Address;
                userCoporation.CityId = userCorporateModel.CityId;
                userCoporation.EmailAddress = userCorporateModel.EmailAddress;
                userCoporation.PhoneNo = userCorporateModel.PhoneNo;
                userCoporation.Picture = userCorporateModel.Picture;
                userCoporation.Status = userCorporateModel.Status;
                userCoporation.ZipCode = userCorporateModel.ZipCode;
                userCoporation.ModifiedBy = UserClaims.UserId;
                userCoporation.ModifiedDate = UserClaims.DateTime;
                userCoporation.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                Entities.Entry(userCoporation).State = EntityState.Modified;

                await UpdateAsync(userCoporation);
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
                var user = await FirstOrDefaultAsync<CorporateUser>(model => model.Id == id);
                if (user is null) return NotFound();
                user.IsDeleted = true;
                user.ModifiedBy = UserClaims.UserId;
                user.ModifiedDate = UserClaims.DateTime;
                user.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(user);
                return Ok();
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
                var userCorporate = await SelectAsync<CorporateUser, DropdownModel>(x => !x.IsDeleted && x.Status == CorporateUserStatus.Approved, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                foreach (var item in userCorporate.ToList())
                {
                    bool isExist = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.CorporateUser);
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

        [HttpGet("Dropdown")]
        public async Task<IActionResult> DropDown()
        {
            try
            {
                var userCorporate = await SelectAsync<CorporateUser, DropdownModel>(x => !x.IsDeleted && x.Status == CorporateUserStatus.Approved, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                return Ok(userCorporate.ToList());
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
        //        return Ok(!await AnyAsync<CorporateUser>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower())
                    
        //            );
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
                return Ok(!await AnyAsync<CorporateUser>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<User>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<FleetOwner>(x => x.EmailAddress.ToLower() == email.ToLower())
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
        [HttpPost("GetCorporateUserFleetOwner")]
        public async Task<IActionResult> GetCorporateUserFleetOwner(FleetOwnerPaginationModel paginationModel)
        {
            try
            {
                var CorporateUserId = await GetAssociatedId();
                Expression<Func<FleetOwner, bool>> filter = x =>
                (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                && (paginationModel.Company == "" || x.CompanyName.ToLower().Contains(paginationModel.Company.ToLower()))
                && (paginationModel.PhoneNo == "" || x.PhoneNo.ToLower().Contains(paginationModel.PhoneNo.ToLower()))
                && !x.IsDeleted && x.UserCorporateId == CorporateUserId;

                var joinResult = Entities.FleetOwner.Where(filter).OrderByDescending(x => x.Id).
                 Select(
                 a => new FleetOwnerModel
                 {
                     Id = a.Id,
                     FirstName = a.FirstName,
                     LastName = a.LastName,
                   
                     EmailAddress = a.EmailAddress,
                     CompanyName = a.CompanyName,
                     PhoneNo = a.PhoneNo,
                     Status = a.Status
                 });
                    
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

    }
}
