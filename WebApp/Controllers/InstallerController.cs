using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;

namespace WebApp.Controllers
{
    [Role(UserRoles.User, UserRoles.Installer)]
    public class InstallerController : BaseController
    {
        public InstallerController() : base("Installer") { }

        [HttpGet]
        public ActionResult AddInstaller() => View();

        [HttpGet]
        public ActionResult InstallerList() => View();

        [HttpGet]
        public ActionResult EditInstaller(int id) => View(id);

        [HttpGet]
        public async Task<ActionResult> GetInstaller(int id)
        {
            try
            {
                var result = await GetAsync<InstallerModel>(id);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var installer = result as InstallerModel;
                    installer.Picture = installer.Picture is null ? "null" : $"data:image/jpg;base64,{installer.Picture}";
                    return Json(installer);
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateInstaller(InstallerModel model)
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

        [HttpPost]
        public async Task<ActionResult> SaveInstaller(InstallerModel model)
        {
            try
            {
                return await PostAsync(model);
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> GetInstallers(InstallerPaginationModel model)
        {
            try
            {
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Company = model.Company ?? "";
                model.Email = model.Email ?? "";
                var result = await GetAsync<InstallerModel,int>(model);
                if(result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var installerModel = result as SuccessModel<List<InstallerModel>, int>;
                    return Json(new { Installers = installerModel.MainData, TotalRecords = installerModel.OtherData}); 
                }
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }

        [OverrideRole(UserRoles.Installer)]
        [HttpPost]
        public async Task<ActionResult> GetBidHistory(InstallerPaginationModel model)
        {
            try {

                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.Price = model.Price;
                model.AwardedDate = model.AwardedDate ?? null;
                // Code End

                var result = await PostAsync<InstallerPaginationModel, List<BidModel>, int>("BidHistory", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var BidHistories = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { BidHistories = BidHistories.MainData, TotalRecords = BidHistories.OtherData });
                }


            } catch(Exception ex) { return Error(ex); }

        }

        [OverrideRole(UserRoles.Installer)]
        [HttpPost]
        public async Task<ActionResult> GetAwardedSites(InstallerPaginationModel model)
        {
            try
            {
                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.AwardedDate = model.AwardedDate ?? null;
                // Code End



                var result = await PostAsync<InstallerPaginationModel, List<BidModel>, int>("AwardedSites", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var AwardedSite = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { AwardedSites = AwardedSite.MainData, TotalRecords = AwardedSite.OtherData });
                }
            }
            catch (Exception ex) { return Error(ex); }
        }
        
        [OverrideRole(UserRoles.Installer)]
        [HttpPost]
        public async Task<ActionResult> AssignSite(ChildEntries obj)
        {
            try
            {
                return await PostAsync<ChildEntries>("AssignSite", obj);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [OverrideRole(UserRoles.Installer)]
        [HttpPost]
        public async Task<ActionResult> GetAssignedSites(InstallerPaginationModel model)
        {
            try
            {
                // Code By : Kashif Shahzad
                model.FirstName = model.FirstName ?? "";
                model.LastName = model.LastName ?? "";
                model.AwardedDate = model.AwardedDate ?? null;
                // Code End


                var result = await PostAsync<InstallerPaginationModel, List<BidModel>, int>("AssignedSitesList", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var AssignedSite = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { AssignedSite = AssignedSite.MainData, TotalRecords = AssignedSite.OtherData });
                }
            }
            catch (Exception ex) { return Error(ex); }
        }
        [HttpGet]
        [OverrideRole(UserRoles.Installer)]
        public async Task<ActionResult> GetCrewsAgainstSites(int id)
        {
            try
            {
                var result = await GetAsync<List<CrewModel>>(id, "GetCrewsAgainstSites");
                return Json(new { CrewsAgainstSites = result as List<CrewModel> });
            }
            catch (Exception ex)
            {

                throw ex;
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
        [OverrideRole(UserRoles.Installer)]
        public ActionResult AssignedSites()
        {
            return View();
        }

        [HttpGet]
        [OverrideRole(UserRoles.Installer, UserRoles.FleetOwner, UserRoles.Supervisor)]
        public async Task<ActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var result = await GetAsync<List<SiteVehicleTypeModel>>(Id, "GetVehiclesAgainstAssignedSites");
                return Json(new { VehiclesAgainstAssignedSites = result as List<SiteVehicleTypeModel> });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.Installer)]
        public async Task<ActionResult> GetVehiclesAgainstAwardedSites(int Id)
        {
            try
            {
                var result = await GetAsync<List<SiteVehicleTypeModel>>(Id, "GetVehiclesAgainstAwardedSites");
                return Json(new { VehiclesAgainstAssignedSites = result as List<SiteVehicleTypeModel> });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [OverrideRole(UserRoles.Installer)]
        public ActionResult AwardedSites()
        {
            return View();
        }

        public ActionResult InvoicedSites()
        {
            return View();
        }


        [OverrideRole(UserRoles.Installer)]
        public ActionResult BidHistory()
        {
            return View();
        }

        public ActionResult SitesForBid()
        {
            return View();
        }

        //Job Log
        public ActionResult JobList() => View();

        [HttpPost]
        public async Task<ActionResult> GetJobDetail(CrewJobPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.VehicleTypeName = model.VehicleTypeName ?? "";
                model.LPN = model.LPN ?? "";
                model.VIN = model.VIN ?? "";
                model.JobStatus = model.JobStatus;

                var url = "Installer/JobList";

                var result = await PostAsync<CrewSiteModel, int>(model, url);
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var crew = result as SuccessModel<List<CrewSiteModel>, int>;
                    return Json(new { TotalRecords = crew.OtherData, crews = crew.MainData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

    }
}