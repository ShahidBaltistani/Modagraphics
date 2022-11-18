using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
namespace WebApp.Controllers
{
    [Role(UserRoles.ApprovalManager)]
    [RoutePrefix("UserLogin")]
    public class UserLoginController : BaseController
    {
        public UserLoginController() : base("UserLogin"){}

        [HttpGet]
        public ActionResult AddLogin() => View();

        [HttpGet]
        public ActionResult LoginList() => View();

        [HttpGet]
        public ActionResult ChangePassword(int id) => View(id);

        [HttpGet]
        public async Task<ActionResult> GetUserLogin(int id)
        {
            try
            {
                var result = await GetAsync<LoginModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var userLogin = result as LoginModel;
                    return Json(userLogin);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetUserLogins(LoginPaginationModel model)
        {
            try
            {
                model.Email = model.Email ?? "";
                model.LoginName = model.LoginName ?? "";
                model.RoleString = model.RoleString ?? "";
                model.IsActiveString = model.IsActiveString ?? "";
                model.UserName = model.UserName ?? "";
                var result = await GetAsync<LoginModel, int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                var userLogins = result as SuccessModel<List<LoginModel>, int>;
                return Json(new { userLogins = userLogins.MainData, TotalRecords = userLogins.OtherData });
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveUserLogin(LoginModel model)
        {
            try
            {
                return await PostAsync(model);
            }
            catch (Exception ex)
            {
               return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserLogin(LoginModel model)
        {
            try
            {
                return await PutAsync(model.Id, model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ChangeUserStatus(int id)
        {
            try
            {
                var result =  await GetAsync<LoginModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                var userLogin = result as LoginModel;
                userLogin.IsActive = !userLogin.IsActive;

                return await PutAsync(userLogin.Id,userLogin);

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("CheckUserName/{id:int}/{name}")]
        public async Task<ActionResult> CheckUserName (int id, string name)
        {
            var result = await GetAsyncWithParams<bool>("UserLogin", "IsUniqueName", id.ToString(), name);
            if (result is ActionResult actionResult)
            {
                return actionResult;
            }
            else
            {
                var isUnqiue = result as bool?;
                return Json(isUnqiue);
            }
        }

    }
}
