using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models;
using Database.Models;
using System.Linq.Expressions;
using WebAPI.Extension;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await ToListAsync<User>(x => !x.IsDeleted);
                return Ok(user.Select(x => new UserModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    CityId = x.CityId,
                    Email = x.EmailAddress,
                    Status = x.Status,
                    Name = x.FirstName,
                    PhoneNo = x.PhoneNo,
                    ZipCode = x.ZipCode,
                    Id = x.Id,
                    UserType = x.UserType
                }));
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await FirstOrDefaultAsync<User>(x => x.Id == id);
                
                return user is null ? (IActionResult)NotFound() : Ok(new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.EmailAddress,
                    Image = user.Picture,
                    PhoneNo = user.PhoneNo,
                    ZipCode = user.ZipCode,
                    CityId = user.CityId,
                    Status = user.Status,
                    UserType = user.UserType
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
                var user = await FirstOrDefaultAsync<User>(model => model.Id == id);
                return user is null ? (IActionResult)NotFound() : Ok(user.Picture);
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(UserPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<User, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.Phone == "" || x.PhoneNo.ToLower().Contains(paginationModel.Phone.ToLower()))
                    && !x.IsDeleted;

                var user = await ToListAsync(filter);

                return Ok(user.OrderByDescending(x => x.Id).Select(x =>
                    new UserModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.EmailAddress,
                        PhoneNo = x.PhoneNo,
                        Status = x.Status,
                        UserType = x.UserType
                    }).GetPage(paginationModel), user.Count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]UserModel userModel)
        {
            try
            {
                await InsertAsync(new User
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Address = userModel.Address,
                    CityId = userModel.CityId,
                    EmailAddress = userModel.Email,
                    Status = userModel.Status,
                    Picture = userModel.Image,
                    PhoneNo = userModel.PhoneNo,
                    ZipCode = userModel.ZipCode,
                    UserType = userModel.UserType,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                    IsDeleted = false,
                    IsEmailVerified = false
                });
                return Ok();
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserModel userModel)
        {
            try
            {
                if(!await AnyAsync<User>(model => model.Id == id))
                {
                    return NotFound();
                }

                var user = await FirstOrDefaultAsync<User>(model => model.Id == id);

                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Address = userModel.Address;
                user.CityId = userModel.CityId;
                user.EmailAddress = userModel.Email;
                user.Status = userModel.Status;
                user.Picture = userModel.Image;
                user.PhoneNo = userModel.PhoneNo;
                user.ZipCode = userModel.ZipCode;
                user.UserType = userModel.UserType;
                user.IsEmailVerified = userModel.IsEmailVerified;
                user.ModifiedBy = UserClaims.UserId;
                user.ModifiedDate = UserClaims.DateTime;
                user.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(user);
                return Ok();
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await FirstOrDefaultAsync<User>(model => model.Id == id);
                if (user is null) return NotFound();
                user.IsDeleted = true;
                user.ModifiedBy = UserClaims.UserId;
                user.ModifiedDate = UserClaims.DateTime;
                user.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(user);
                return Ok();
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Dropdown")]
        public async Task<IActionResult> DropDown()
        {
            try
            {
                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted && x.Status == UserStatus.Approved, x => new DropdownModel
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
        //        return Ok(!await AnyAsync<User>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower()));
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
                return Ok(!await AnyAsync<User>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower()) 
                    && !await AnyAsync<CorporateUser>(x => x.EmailAddress.ToLower() == email.ToLower())
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

        [HttpGet("Dropdown/DataEntryOperator")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted &&
                x.Status == UserStatus.Approved && x.UserType == UserType.DateEntryOperator, x => new DropdownModel
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
        [HttpGet("Dropdown/ApprovalManager")]
        public async Task<IActionResult> GetApprovalManager()
        {
            try
            {
                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted &&
                x.Status == UserStatus.Approved && x.UserType == UserType.ApprovalManager, x => new DropdownModel
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

        [HttpGet("Dropdown/FinanceManager")]
        public async Task<IActionResult> GetFinanceManager()
        {
            try
            {
                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted &&
                x.Status == UserStatus.Approved && x.UserType == UserType.FinanceManager, x => new DropdownModel
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
        [HttpGet("Dropdown/DataEntryOperatorForAddLogin")]
        public async Task<IActionResult> GetUsersForAddLogin()
        {
            try
            {
                List<DropdownModel> dropdowns = new List<DropdownModel>();


                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted &&
                x.Status == UserStatus.Approved && x.UserType == UserType.DateEntryOperator, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });

                foreach (var item in project.ToList())
                {
                    bool isExist = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.User);
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

        [HttpGet("Dropdown/ApprovalManagerForAddLogin")]
        public async Task<IActionResult> GetApprovalManagerForAddLogin()
        {
            try
            {
                List<DropdownModel> dropdowns = new List<DropdownModel>();

                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted &&
                x.Status == UserStatus.Approved && x.UserType == UserType.ApprovalManager, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                foreach (var item in project.ToList())
                {
                    bool isExist = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.ApprovalManager);
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

        [HttpGet("Dropdown/FinanceManagerForAddLogin")]
        public async Task<IActionResult> GetFinanceManagerForAddLogin()
        {
            try
            {
                List<DropdownModel> dropdowns = new List<DropdownModel>();

                var project = await SelectAsync<User, DropdownModel>(x => !x.IsDeleted &&
                x.Status == UserStatus.Approved && x.UserType == UserType.FinanceManager, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                foreach (var item in project.ToList())
                {
                    bool isExist = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.FinanceManager);
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
    }
}
