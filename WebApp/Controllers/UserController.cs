using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [WebApp.Role(UserRoles.User)]
    public class UserController : BaseController
    {
        public UserController() : base("User") { }
        public ActionResult Create() => View();
        public ActionResult EditUser(int id) => View(id);
        public ActionResult UserList() => View();

        [HttpPost]
        public async Task<ActionResult> SaveUser(UserModel model)
        {
            try
            {
                model.Status = UserStatus.WaitingForApproval;
                return await PostAsync(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserModel model)
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
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var result = await GetAsync<UserModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var user = result as UserModel;
                    user.Image = user.Image is null ? "null" : $"data:image/jpg;base64,{user.Image}";
                    return Json(user);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetUsers(UserPaginationModel model)
        {
            model.FirstName = model.FirstName ?? "";
            model.LastName = model.LastName ?? "";
            model.Email = model.Email ?? "";
            model.Phone = model.Phone ?? "";
            var result = await GetAsync<UserModel,int>(model);
            if(result is ActionResult actionResult)
            {
                return actionResult;
            }
            else
            {
                var users = result as SuccessModel<List<UserModel>,int>;
                return Json(new { TotalRecords = users.OtherData, Users = users.MainData });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(int id)
        {
            var result = await GetAsync<string>(id, "Image");
            if(result is ActionResult actionResult)
            {
                return actionResult;
            }
            var imageString = result as string;
            if (imageString is null)
            {
                return Json("null");
            }
            else
            {
                var imgUrlData = $"data:image/jpg;base64,{imageString}";
                return Json(imgUrlData);
            }
        }
    }
}