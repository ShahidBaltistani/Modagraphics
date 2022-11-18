using Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Threading.Tasks;
using Http = System.Web.HttpContext;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(bool id = false)
        {
            ViewBag.ErrorMessage = id;

            if (Http.Current.Session["Role"] is UserRoles role)
            {
                Http.Current.Session.Timeout = Utility.SessionTimeout;

                var index = "";

                switch (role)
                {
                    case UserRoles.User: index = "Admin"; break;
                    case UserRoles.Installer: index = "Installer"; break;
                    case UserRoles.FleetOwner: index = "FleetOwner"; break;
                    case UserRoles.Crew: index = "Crew"; break;
                    case UserRoles.ApprovalManager: index = "ApprovalManager"; break;
                    case UserRoles.Supervisor: index = "Supervisor"; break;
                }

                return RedirectToAction(index, "Dashboard");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> LoginToAccount(LoginModel model)
        {
            try
            {
                model.IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
                model.UserAgent = Request.UserAgent;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Utility.APIBaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync("Login", model);
                    var result = await response.Content.ReadAsAsync<SuccessModel<UserModel, TokenModel>>();
                    if (response.IsSuccessStatusCode)
                    {
                        Http.Current.Session["UserToken"] = result.OtherData;
                        Http.Current.Session["Role"] = result.MainData.Role;
                        Http.Current.Session["UserModel"] = result.MainData;
                        Http.Current.Session.Timeout = Utility.SessionTimeout;

                        var index = "";
                        switch (result.MainData.Role)
                        {
                            case UserRoles.User: index = "Admin"; break;
                            case UserRoles.Installer: index = "Installer"; break;
                            case UserRoles.FleetOwner: index = "FleetOwner"; break;
                            case UserRoles.Crew: index = "Crew"; break;
                            case UserRoles.ApprovalManager: index = "ApprovalManager"; break;
                            case UserRoles.Supervisor: index = "Supervisor"; break;
                            case UserRoles.CorporateUser: index = "CorporateUser"; break;
                        }

                        return RedirectToAction(index, "Dashboard");
                    }
                    return RedirectToAction("Index", new { id = true });
                }
            }
            catch (Exception ex)
            {
                Http.Current.Session["EX"] = ex;
                return RedirectToAction("", "Exception");
            }
        }

        public ActionResult Off()
        {
            Http.Current.Session.Clear();
            Http.Current.Session.Abandon();
            return RedirectToAction("");
        }

        public void Refresh()
        {
            Http.Current.Session.Timeout = Utility.SessionTimeout;
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword(string user)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Utility.APIBaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync("GetUserLogin", user);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var model = await response.Content.ReadAsAsync<LoginModel>();
                        var emailresponse =  await new Email().ForgotPassword(model);
                        if(emailresponse == true)
                        {
                            return Json(new { success = true, message = "Password has been sent to your email,please login using password provided in email." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Face error while sending email,please try again later." });
                        }
                    }
                    return Json(new { success = false, message = "Email not found." });
                }
            }
            catch (Exception ex)
            {
                Http.Current.Session["EX"] = ex;
                return RedirectToAction("", "Exception");
            }
        }
    }
}