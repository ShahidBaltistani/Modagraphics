using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;

namespace WebApp.Controllers
{
    [Role(UserRoles.User)]
    [RoutePrefix("Site")]
    public class SiteController : BaseController
    {
        public SiteController() : base("Site") { }

        [HttpGet]
        public ActionResult AddSite() => View();

        [HttpGet]
        public ActionResult SitesForBid() => View();

        [HttpGet]
        public ActionResult SiteList() => View();

        [HttpGet]
        public ActionResult EditSite(int id) => View(id);

        [HttpPost]
        public async Task<ActionResult> SaveProject(SiteModel model)
        {
            try
            {
                model.OtherQuestionsComments = model.OtherQuestionsComments ?? "";
                model.SpecialInstruction = model.SpecialInstruction ?? "";
                model.ScopeOfWork = model.ScopeOfWork ?? "";
                model.PONo = model.PONo ?? "";
                model.EPMSJobNo = model.EPMSJobNo ?? "";
                model.AdditionalContacts = model.AdditionalContacts ?? "";

                return await PostAsync(model);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetSites(SitePaginationModel model)
        {
            try
            {
                model.SiteName = model.SiteName ?? "";
                model.PO = model.PO ?? "";
                model.ProjectName = model.ProjectName ?? "";
                model.FleetOwnerName = model.FleetOwnerName ?? "";

                var result = await GetAsync<SiteModel, int>(model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sites = site.MainData, TotalRecords = site.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //Get Sites that are open for bid
        [OverrideRole(UserRoles.Installer)]
        [HttpPost]
        public async Task<ActionResult> GetSitesOpenForBid(SitePaginationModel model)
        {
            try {
                // Code By : Kashif Shahzad
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";
                model.OpeningDate = model.OpeningDate ?? null;
                // Code End
                //var result = await GetOpenSites<SiteModel, int>(model);
                var result = await PostAsync<SitePaginationModel, List<SiteModel>, int>("OpenSites", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sites = site.MainData, TotalRecords = site.OtherData });
                }

            } catch(Exception ex) {
                return Error(ex);
            }

        }

        [HttpPost]
        public async Task<ActionResult> AwardSite(BidModel model)
        {

            try
            {
                await PostAsync<BidModel>("AwardSite", model);

                return Json(new { AwardSite = true });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> RejectSite(BidModel model)
        {
            try
            {
                await PostAsync<BidModel>("RejectSite", model);
                return Json(new { RejectSite = true });
            }
            catch (Exception ex)
            {

                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetBidList(SiteBidPaginationModel model) {
            try
            {

                model.Installer = model.Installer ?? "";
                model.BidPrice = model.BidPrice;
                model.BidDate = model.BidDate ?? null;



                //var result = await GetOpenSites<SiteModel, int>(model);
                var result = await PostAsync<SiteBidPaginationModel, List<BidModel>, int>("SiteBidList", model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<BidModel>, int>;
                    return Json(new { sites = site.MainData, TotalRecords = site.OtherData });
                }

            }
            catch (Exception ex)
            {
                return Error(ex);

            }


        }

        //[HttpGet]
        public async Task<ActionResult> GetSite(int id)
        {
            var result = await GetAsync<SiteModel>(id);
            if (result is ActionResult actionResult)
            {
                return actionResult;
            }
            else
            {
                var project = result as SiteModel;
                return Json(project);
            }
        }

        [HttpGet]
        public ActionResult SiteBidList(string id) {
            try
            {
                var arr = id.Split('$');
                ViewBag.SiteName = arr[1];
                return View(int.Parse(arr[0]));

            }catch(Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetSiteBidList(SiteBidPaginationModel model)
        {
            try
            {
                model.SiteName = model.SiteName ?? "";
                model.ProjectName = model.ProjectName ?? "";

                var result = await GetAsync<SiteModel, int>(model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sites = site.MainData, TotalRecords = site.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSite(SiteModel model)
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
        public async Task<ActionResult> StuatsOpenForBid(uint id)
        {
            try
            {
                return await GetAsync("UpdateStatus", id, SiteStatus.OpenForBid);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> StuatsClosedForBid(int id)
        {
            try
            {
                return await GetAsync("UpdateStatus", id, SiteStatus.ClosedForBid);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> SaveSiteBidStatusLog(SiteBidStatusLogModel model)
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

        [HttpGet]
        [Route("CheckUserName/{projectId:int}/{id:int}/{name}")]
        public async Task<ActionResult> CheckUserName(int projectId,int id,string name)
        {
            var result = await GetAsyncWithParams<bool>("Site", "IsUniqueName", projectId.ToString(), id.ToString(), name);
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

        [OverrideRole(UserRoles.Installer)]
        public ActionResult OpenSites()
        {
            return View();
        }

        [OverrideRole(UserRoles.Installer)]
        public async Task<ActionResult> GetOpenSites(SiteBidPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
                model.SiteName = model.SiteName ?? "";

                var result = await PostAsync<SiteBidPaginationModel, List<SiteModel>, int>("OpenSites", model);
                if(result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var responseModel = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sites = responseModel.MainData, TotalRecords = responseModel.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public async Task<ActionResult> AssociationVerification(BidModel model)
        {
            try
            {
                var result = await GetAsyncWithParams<bool>("Site", "IsUniqueName", model.SiteId.ToString());
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
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [OverrideRole(UserRoles.User,UserRoles.Installer,UserRoles.Crew,UserRoles.Supervisor,UserRoles.FleetOwner)]
        public async Task<ActionResult> ProjectAgainstSite(int Id)
        {
            try
            {
                var result = await GetAsync<List<ProjectModel>>(Id, "ProjectAgainstSite");
                return Json(new { ProjectDetail=result });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        //Site List
        [HttpGet]
        public async Task<ActionResult> JobInQueueDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "JobInQueueDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { InQueue = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> JobInProgressDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "JobInProgressDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { InProgress = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompletedJobDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "CompletedJobDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { Completed = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [HttpGet]
        public async Task<ActionResult> JobUnderReviewDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "JobUnderReviewDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { UnderReview = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //Type

        [HttpGet]
        public async Task<ActionResult> JobInQueueDetailByJobId(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "JobInQueueDetailByJobId");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { InQueueType = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> JobInProgressDetailByJobId(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "JobInProgressDetailByJobId");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { InProgressType = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompletedDetailByJobId(int id)
        {
            try
            {
                var result = await GetAsync<List<JobModel>>(id, "CompletedDetailByJobId");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { CompletedType = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //for Site Log

        [HttpGet]
        public ActionResult SiteStatusList() => View();

        [HttpPost]
        public async Task<ActionResult> GetSiteStatusLog(SitePaginationModel model)
        {
            try
            {
                model.SiteName = model.SiteName ?? "";
                model.PO = model.PO ?? "";
                model.ProjectName = model.ProjectName ?? "";
                model.FleetOwnerName = model.FleetOwnerName ?? "";

                var result = await GetAsync<SiteModel, int>(model);

                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var site = result as SuccessModel<List<SiteModel>, int>;
                    return Json(new { sitesStatus = site.MainData, TotalRecords = site.OtherData });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        [HttpGet]
        public async Task<ActionResult> ViewSiteLogDetail(int id)
        {
            try
            {
                var result = await GetAsync<List<SiteStatusLogModel>>(id, "JobInQueueDetail");
                if (result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    return Json(new { logs = result });
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.User)]
        public async Task<ActionResult> SiteAgainstProject(int Id)
        {
            try
            {
                var result = await GetAsync<SiteModel>(Id, "SiteAgainstProject");
                return Json(new { SiteInfo = result });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [OverrideRole(UserRoles.User)]
        public async Task<ActionResult> SiteVehicleTypeDetail(int Id)
        {
            try
            {
                var result = await GetAsync<SiteVehicleTypeModel>(Id, "SiteVehicleTypeDetail");
                return Json(new { Sitevehicletypedetail = result });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


    }
}
