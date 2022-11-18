using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [WebApp.Role(UserRoles.User)]
    public class VehicleController : BaseController
    {
        public VehicleController() : base("Vehicle") { }
        public ActionResult CreateType() => View();
        public ActionResult Types() => View();
        public ActionResult EditType(int id) => View(id);

        [HttpPost]
        public async Task<ActionResult> SaveVehicleType(VehicleTypeModel model)
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
        public async Task<ActionResult> UpdateVehicleType(VehicleTypeModel model)
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
        public async Task<ActionResult> GetVehicle(int id)
        {
            try
            {
                var result = await GetAsync<VehicleTypeModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var vehicle = result as VehicleTypeModel;
                    vehicle.ImageString = vehicle.ImageString is null ? "null" : $"data:image/jpg;base64,{vehicle.ImageString}";
                    return Json(vehicle);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetVehicles(VehicleTypePaginationModel model)
        {
            try
            {
                model.VType = model.VType ?? "";
                var result = await GetAsync<VehicleTypeModel,int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var vehicles = result as SuccessModel<List<VehicleTypeModel>, int>;
                    return Json(new { TotalRecords = vehicles.OtherData, Vehicles = vehicles.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(int id)
        {
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
                    return imageString is null ? Json("null") : Json($"data:image/jpg;base64,{imageString}");
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}