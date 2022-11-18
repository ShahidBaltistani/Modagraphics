using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Role(UserRoles.User)]
    public class CorporateUserController : BaseController
    {
        public CorporateUserController() : base("CorporateUser") { }

        [HttpGet]
        public ActionResult CorporateUserList() => View();
        [HttpGet]
        public ActionResult AddCorporateUser() => View();
        [HttpGet]
        public ActionResult EdituserCorporate(int id) => View(id);

        [HttpPost]
        public async Task<ActionResult> SaveUserCorporate(CorporateUserModel userCorporateModel)
        {
            try
            {
                return await PostAsync(userCorporateModel);
                
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetUserCorporates(CorporateUserPaginationModel model)
        {
            try
            {
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Email = model.Email ?? "";
                model.PhoneNumber = model.PhoneNumber ?? "";
                var result = await GetAsync<CorporateUserModel,int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var userCorporate = result as SuccessModel<List<CorporateUserModel>,int>;
                    return Json(new { corporateUsers = userCorporate.MainData, TotalRecords = userCorporate.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserCorporate(CorporateUserModel userCorporateModel)
        {
            try
            {
                return await PutAsync(userCorporateModel.Id, userCorporateModel);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(int id)
        {
            //var result = await GetAsync<string>(id, "UserCorporate", "Image");
            try
            {
                var result = await GetAsync<string>(id, "Image");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var imageString = result as string;
                    if (imageString == "")
                    {
                        return Json("null");
                    }
                    var imgUrlData = imageString is null ? "null" : $"data:image/jpg;base64,{imageString}";
                    return Json(imgUrlData);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetUserCorporate(int id)
        {
            try
            {
                var result = await GetAsync<CorporateUserModel>(id);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var userCorporateModel = result as CorporateUserModel;
                    return Json(userCorporateModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Throw(ex);
            }
        }

        [OverrideRole(UserRoles.CorporateUser)]

        public ActionResult GetCorporateUserFleetOwnerList() => View();

        [OverrideRole(UserRoles.CorporateUser)]
        [HttpPost]
        public async Task<ActionResult> GetCorporateUserFleetOwnerList(FleetOwnerPaginationModel model)
        {
            try
            {
               
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Company = model.Company ?? "";
                model.Email = model.Email ?? "";
                model.PhoneNo = model.PhoneNo ?? "";
                var result = await GetAsync<FleetOwnerModel, int>(model, "GetCorporateUserFleetOwner");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var fleetOwner = result as SuccessModel<List<FleetOwnerModel>, int>;
                    return Json(new { TotalRecords = fleetOwner.OtherData, fleetOwners = fleetOwner.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
