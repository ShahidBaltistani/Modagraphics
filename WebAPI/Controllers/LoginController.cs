using Database;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Linq;
using WebAPI.ActionResults;
using WebAPI.JWT;

namespace WebAPI.Controllers
{
    [Route("")]
    [Produces("application/json")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpPost("Login")]
        public IActionResult Post([FromBody] LoginModel model)
        {
            try
            {
                using (var entitis = new RapidusContextFactory().CreateDbContext(null))
                {
                    var login = entitis.Login.FirstOrDefault(x => x.UserName.ToLower().Equals(model.Username.ToLower()) && x.IsActive && !x.IsDeleted);
                    if(login != null && login?.Password == model.Password)
                    {
                        try
                        {
                            entitis.Logs.Add(new Database.Models.Log
                            {
                                IP = model.IP,
                                UserAgent = model.UserAgent,
                                EnteredOn = DateTime.UtcNow.AddMinutes(model.TimeZoneOffset)
                            });
                            entitis.SaveChanges();
                        }
                        catch { }
                        var name = "";
                        var email = "";
                        var image = "";

                        switch (login.Role)
                        {
                            case UserRoles.User:
                                var user = entitis.User.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (user?.FirstName ?? "Dummy") + " " + (user?.LastName ?? "Name");
                                email = user?.EmailAddress ?? "user@dummy.com";
                                image = user?.Picture ?? "";
                                break;
                            case UserRoles.ApprovalManager:
                                var approvalManager = entitis.User.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (approvalManager?.FirstName ?? "Dummy") + " " + (approvalManager?.LastName ?? "Name");
                                email = approvalManager?.EmailAddress ?? "user@dummy.com";
                                image = approvalManager?.Picture ?? "";
                                break;
                            case UserRoles.FinanceManager:
                                var financeManager = entitis.User.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (financeManager?.FirstName ?? "Dummy") + " " + (financeManager?.LastName ?? "Name");
                                email = financeManager?.EmailAddress ?? "user@dummy.com";
                                image = financeManager?.Picture ?? "";
                                break;
                            case UserRoles.CorporateUser:
                                var corporateUser = entitis.CorporateUser.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (corporateUser?.FirstName ?? "Dummy") + " " + (corporateUser?.LastName ?? "Name");
                                email = corporateUser?.EmailAddress ?? "user@dummy.com";
                                image = corporateUser?.Picture ?? "";
                                break;
                            case UserRoles.Installer:
                                var installer = entitis.Installer.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (installer?.FirstName ?? "Dummy") + " " + (installer?.LastName ?? "Name");
                                email = installer?.EmailAddress ?? "user@dummy.com";
                                image = installer?.Picture ?? "";
                                break;
                            case UserRoles.FleetOwner:
                                var fleetOwner = entitis.FleetOwner.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (fleetOwner?.FirstName ?? "Dummy") + " " + (fleetOwner?.LastName ?? "Name");
                                email = fleetOwner?.EmailAddress ?? "user@dummy.com";
                                image = fleetOwner?.Picture ?? "";
                                break;
                            case UserRoles.Crew:
                                var crew = entitis.Crew.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (crew?.FirstName ?? "Dummy") + " " + (crew?.LastName ?? "Name");
                                email = crew?.EmailAddress ?? "user@dummy.com";
                                image = crew?.Picture ?? "";
                                break;
                            case UserRoles.Supervisor:
                                var supervisor = entitis.Supervisor.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = (supervisor?.FirstName ?? "Dummy") + " " + (supervisor?.LastName ?? "Name");
                                email = supervisor?.EmailAddress ?? "user@dummy.com";
                                image = supervisor?.Picture ?? "";
                                break;
                            default:
                                name = "Dummy User";
                                email = "user@dummy.com";
                                image = "";
                                break;
                        }

                        var userModel = new UserModel() { Name = name, Role = login.Role, Email = email, Image = image };
                        var token = JwtAuthentication.GetToken(login.Id, model.TimeZoneOffset.ToString(), login.Role.ToString());
                        return Ok(new SuccessModel<UserModel, TokenModel>(userModel, token));
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
            }
            catch (Exception ex)
            {
                return new MethodFailureObjectResult(ex);
            }
        }

        [HttpGet("RenewToken")]
        public IActionResult Get([FromHeader] string Authorization)
        {
            try
            {
                Authorization = Authorization.Replace("Bearer ", string.Empty);
                return string.IsNullOrEmpty(Authorization)
                    ? Unauthorized() : (IActionResult)Ok(new SuccessModel<TokenModel>(JwtAuthentication.GetToken(Authorization)));
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("GetUserLogin")]
        public IActionResult GetUserLogin([FromBody] string userID)
        {
            try
            {
                using (var entitis = new RapidusContextFactory().CreateDbContext(null))
                {
                    var login = entitis.Login.FirstOrDefault(x => x.UserName.ToLower().Equals(userID.ToLower()) && x.IsActive && !x.IsDeleted);
                    if (login != null )
                    {
                       
                        var name = "";
                        var email = "";
                       
                        
                        switch (login.Role)
                        {
                            case UserRoles.User:
                              var  user = entitis.User.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = user.FirstName + " " + user.LastName ;
                                email = user?.EmailAddress ;
                               
                                break;
                            case UserRoles.ApprovalManager:
                                var approvalManager = entitis.User.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = approvalManager.FirstName  + " " + approvalManager.LastName ;
                                email = approvalManager.EmailAddress ;
                                
                                break;
                            case UserRoles.FinanceManager:
                                var financeManager = entitis.User.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = financeManager.FirstName  + " " + financeManager.LastName ;
                                email = financeManager.EmailAddress ;
                                break;
                            case UserRoles.CorporateUser:
                                var corporateUser = entitis.CorporateUser.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = corporateUser.FirstName  + " " + corporateUser.LastName;
                                email = corporateUser.EmailAddress ;
                                
                                break;
                            case UserRoles.Installer:
                                var installer = entitis.Installer.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = installer.FirstName + " " + installer.LastName;
                                email = installer.EmailAddress;
                              
                                break;
                            case UserRoles.FleetOwner:
                                var fleetOwner = entitis.FleetOwner.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = fleetOwner.FirstName  + " " + fleetOwner.LastName ;
                                email = fleetOwner.EmailAddress;
                               
                                break;
                            case UserRoles.Crew:
                                var crew = entitis.Crew.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = crew.FirstName + " " + crew.LastName ;
                                email = crew.EmailAddress ;
                               
                                break;
                            case UserRoles.Supervisor:
                                var supervisor = entitis.Supervisor.FirstOrDefault(x => x.Id == login.AssociatedId);
                                name = supervisor.FirstName  + " " + supervisor.LastName ;
                                email = supervisor.EmailAddress ;
                                
                                break;
                            default:
                                name = "";
                                email = "";
                                break;
                        }
                        if (!string.IsNullOrEmpty(email) )
                        {
                            var LoginModel = new LoginModel() { Name = name, UserRole = login.Role, Email = email, Password= login.Password };

                            return Ok(LoginModel);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                }
            }
            catch (Exception ex)
            {
                return new MethodFailureObjectResult(ex);
            }
        }
    }
}
