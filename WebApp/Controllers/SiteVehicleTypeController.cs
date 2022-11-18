using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models;

namespace WebApp.Controllers
{
    [WebApp.Role(UserRoles.User)]
    public class SiteVehicleTypeController : BaseController
    {
        public SiteVehicleTypeController(): base("SiteVehicleType") { }

        [HttpGet]
        public ActionResult AddType(string id)
        {
            try
            {
                var arr = id.Split('$');
                ViewBag.SiteName = arr[1];
                return View(int.Parse(arr[0]));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ActionResult Types(string id)
        {
            try
            {
                var arr = id.Split('$');
                ViewBag.SiteName = arr[1];
                ViewBag.IsModificationDisabled = arr[2];
                return View(int.Parse(arr[0]));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ActionResult EditType(int id) => View(id);

        public async Task<ActionResult> GetSiteVehicles(VehicleTypeSiteAssociationPaginationModel model)
        {
            try
            {

                // Code By : Kashif Shahzad
                model.VehicleType = model.VehicleType ?? "";
                model.Specification = model.Specification ?? "";
                // Code End



                var result = await GetAsync<SiteVehicleTypeModel, int>(model);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var VehicleTypeSite = result as SuccessModel<List<SiteVehicleTypeModel>, int>;
                    return Json(new { siteVehicles = VehicleTypeSite.MainData, TotalRecords = VehicleTypeSite.OtherData });

                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> SaveSiteVehicleType(SiteVehicleTypeModel model)
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

        public async Task<ActionResult> UpdateSiteVehicleType(SiteVehicleTypeModel model)
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

        public async Task<ActionResult> GetBasePrice(int id)
        {
            try
            {
                var result = await GetAsync<string>(id, "Vehicle", "BasePrice");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var vechicleBasePrice = result as string;
                    return Json(vechicleBasePrice);

                }
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> GetSiteVehicle(int id)
        {
            try
            {
                var result = await GetAsync<SiteVehicleTypeModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var VehicleTypeSite = result as SiteVehicleTypeModel;
                    return Json(VehicleTypeSite);

                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> DeleteSiteVehicleType(int id)
        {
            try
            {
                var result = await DeleteAsync<SiteVehicleTypeModel>((uint)id);
                if (result is ErrorResult actionResult)
                {
                    
                    return actionResult;
                }
                 if (result is ActionResult ac)
                {
                    return ac;
                   

                }
                return result;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


    }
}