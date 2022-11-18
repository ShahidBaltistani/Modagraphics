using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace WebAPI.Controllers
{
    
    public class UserLoginController : BaseController
    {
        public UserLoginController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var logins = await ToListAsync<Login>(x => !x.IsDeleted);
                return Ok(logins.Select(x =>
                    new LoginModel
                    {
                        Id = x.Id,
                        AssociatedId = x.AssociatedId,
                        UserRole = x.Role,
                        Username = x.UserName,
                        Password = x.Password,
                        IsActive = x.IsActive
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
                var login = await FirstOrDefaultAsync<Login>(model => model.Id == id);
                return login == null ? (IActionResult)NotFound() : Ok(new LoginModel
                {
                    Id = login.Id,
                    AssociatedId = login.AssociatedId,
                    UserRole = login.Role,
                    Username = login.UserName,
                    Password = login.Password,
                    IsActive = login.IsActive
                });
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
                var result = await FirstOrDefaultAsync<Login>(x => x.AssociatedId == id);
                if(result is null)
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

        [HttpPost]
        [Route("Page")]
        public async Task<IActionResult> PostPage(LoginPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Login, bool>> filter = x =>
                     (paginationModel.LoginName == "" || x.UserName.ToLower().Contains(paginationModel.LoginName.ToLower()))
                    && (paginationModel.RoleString == "" || x.Role == paginationModel.Role)
                    && (paginationModel.IsActiveString == "" || x.IsActive == paginationModel.IsActive) 
                    && !x.IsDeleted && x.CreatedBy != 0
                    && x.Role !=UserRoles.Crew && x.Role!=UserRoles.Supervisor;// Code By:Kashif Shahzad

                var result = await GetPageAsync(filter, paginationModel, x => new LoginModel
                {
                    Id = x.Id,
                    Username = x.UserName,
                    AssociatedId = x.AssociatedId,
                    UserRole = x.Role,
                    IsActive = x.IsActive
                });

                //result.MainData.ForEach(async x =>
                //{
                //    switch (x.UserRole)
                //    {
                //        case UserRoles.User:
                //            var user = await FirstOrDefaultAsync<User>(y => y.Id == x.AssociatedId);
                //            x.Name = user?.FirstName ?? "" + " " + user?.LastName ?? "";
                //            x.Email = user?.EmailAddress ?? "";
                //            break;
                //        case UserRoles.ApprovalManager:
                //            x.Name = "Approval Manager Name";
                //            x.Email = "Approval Email";
                //            break;
                //        case UserRoles.FinanceManager:
                //            x.Name = "Finance Manager Name";
                //            x.Email = "Approval Email";
                //            break;
                //        case UserRoles.CorporateUser:
                //            var corporateUser = await FirstOrDefaultAsync<CorporateUser>(y => y.Id == x.AssociatedId);
                //            if (corporateUser != null)
                //            {
                //                x.Name = corporateUser.FirstName + " " + corporateUser.LastName;
                //                x.Email = corporateUser.EmailAddress;
                //            }
                //            break;
                //        case UserRoles.Installer:
                //            var installer = await FirstOrDefaultAsync<Installer>(y => y.Id == x.AssociatedId);
                //            x.Name = installer.FirstName + " " + installer.LastName;
                //            x.Email = installer.EmailAddress;
                //            break;
                //        case UserRoles.FleetOwner:
                //            var fleetOwner = await FirstOrDefaultAsync<FleetOwner>(y => y.Id == x.AssociatedId);
                //            x.Name = fleetOwner?.FirstName ?? "" + " " + fleetOwner?.LastName ?? "";
                //            x.Email = fleetOwner?.EmailAddress ?? "";
                //            break;
                //        case UserRoles.Crew:
                //            var crew = await FirstOrDefaultAsync<Crew>(y => y.Id == x.AssociatedId);
                //            x.Name = crew?.FirstName ?? "" + " " + crew?.LastName ?? "";
                //            x.Email = crew?.EmailAddress ?? "";
                //            break;
                //        default:
                //            break;
                //    }
                //});

                foreach (var data in result.MainData)
                {
                    switch (data.UserRole)
                    {
                        case UserRoles.User:
                            var user = await FirstOrDefaultAsync<User>(y => y.Id == data.AssociatedId);
                            data.Name = user?.FirstName ?? "" + " " + user?.LastName ?? "";
                            data.Email = user?.EmailAddress ?? "";
                            break;
                        case UserRoles.ApprovalManager:
                            data.Name = "Approval Manager Name";
                            data.Email = "Approval Email";
                            break;
                        case UserRoles.FinanceManager:
                            data.Name = "Finance Manager Name";
                            data.Email = "Approval Email";
                            break;
                        case UserRoles.CorporateUser:
                            var corporateUser = await FirstOrDefaultAsync<CorporateUser>(y => y.Id == data.AssociatedId);
                            data.Name = corporateUser?.FirstName ?? "" + " " + corporateUser?.LastName ?? "";
                            data.Email = corporateUser?.EmailAddress ?? "";
                            break;
                        case UserRoles.Installer:
                            var installer = await FirstOrDefaultAsync<Installer>(y => y.Id == data.AssociatedId);
                            data.Name = installer?.FirstName ?? "" + " " + installer?.LastName ?? "";
                            data.Email = installer?.EmailAddress ?? "";
                            break;
                        case UserRoles.FleetOwner:
                            var fleetOwner = await FirstOrDefaultAsync<FleetOwner>(y => y.Id == data.AssociatedId);
                            data.Name = fleetOwner?.FirstName ?? "" + " " + fleetOwner?.LastName ?? "";
                            data.Email = fleetOwner?.EmailAddress ?? "";
                            break;
                        case UserRoles.Crew:
                            var crew = await FirstOrDefaultAsync<Crew>(y => y.Id == data.AssociatedId);
                            data.Name = crew?.FirstName ?? "" + " " + crew?.LastName ?? "";
                            data.Email = crew?.EmailAddress ?? "";
                            break;
                        default:
                            break;
                    }
                }
                result.MainData = result.MainData.Where(x => (paginationModel.UserName == ""
                   || x.Name.ToLower().Contains(paginationModel.UserName.ToLower())) &&
                    (paginationModel.Email == "" || x.Email.ToLower().Contains(paginationModel.Email.ToLower()))).ToList();
                //result.OtherData = result.MainData.Count;
                return Ok(result.MainData, result.OtherData);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginModel login)
        {
            try
            {
                if (await AnyAsync<Login>(x => x.UserName.ToLower() == login.Username.ToLower()))
                {
                    return Conflict("Username already exists");
                }
                var vLogin = new Login
                {
                    Id = login.Id,
                    AssociatedId = login.AssociatedId,
                    Role = login.UserRole,
                    UserName = login.Username,
                    Password = login.Password,
                    IsActive = login.IsActive,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vLogin);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]LoginModel login)
        {
            try
            {
                var vLogin = await FirstOrDefaultAsync<Login>(model => model.Id == id);

                if (vLogin is null)
                {
                    return NotFound();
                }
                else if (await AnyAsync<Login>(x => x.Id != login.Id && x.UserName.ToLower() == login.Username.ToLower()))
                {
                    return Conflict("Username already exists");
                }

                vLogin.AssociatedId = login.AssociatedId;
                vLogin.Role = login.UserRole;
                vLogin.UserName = login.Username;
                vLogin.Password = login.Password;
                vLogin.IsActive = login.IsActive;
                vLogin.IsDeleted = false;
                vLogin.ModifiedBy = UserClaims.UserId;
                vLogin.ModifiedDate = UserClaims.DateTime;
                vLogin.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vLogin);
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
                var login = await FirstOrDefaultAsync<Login>(x => x.Id == id);
                if (login is null) return NotFound();
                login.IsDeleted = true;
                login.ModifiedBy = UserClaims.UserId;
                login.ModifiedDate = UserClaims.DateTime;
                login.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(login);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("IsUniqueName/{id}/{userName}")]
        public async Task<IActionResult> VerifyEmail(uint id, string userName)
        {
            try
            {
                return Ok(!await AnyAsync<Login>(x => x.Id != id && x.UserName.ToLower() == userName.ToLower()));
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] string body)
        {
            try
            {
                var arr = body.Split("$--$");
                var user = await FirstOrDefaultAsync<Login>(x => x.Id == UserClaims.UserId);
                if(user is null)
                {
                    return NotFound();
                }
                else if(user.Password != arr[0])
                {
                    return Unauthorized();
                }
                else
                {
                    user.Password = arr[1];
                    await UpdateAsync(user);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
    }
}
