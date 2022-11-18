using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
namespace WebApp.Controllers
{
    [Role(UserRoles.User)]
    public class ProjectController : BaseController
    {
        public ProjectController(): base("Project") { }

        [HttpGet]
        public ActionResult AddProject() => View();

        [HttpGet]
        public ActionResult ProjectList() => View();

        [HttpGet]
        public ActionResult EditProject(int id) => View(id);

        [HttpPost]
        public async Task<ActionResult> SaveProject(ProjectModel model)
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
        public async Task<ActionResult> GetProjects(ProjectPaginationModel model)
        {
            try
            {
                model.ProjectName = model.ProjectName ?? "";
               // model.ToDate = model.ToDate;
               // model.FromDate = model.FromDate;
                model.FleetOwnerName = model.FleetOwnerName ?? "";
                model.ProjectContactName = model.ProjectContactName ?? "";
                model.ProjectContactPhone = model.ProjectContactPhone ?? "";
                model.ProjectAddress = model.ProjectAddress ?? "";
                var result = await GetAsync<ProjectModel,int>(model);
                if(result is ActionResult actionResult)
                {
                    return actionResult;
                }
                else
                {
                    var project = result as SuccessModel<List<ProjectModel>, int>;
                    return Json(new { ProjectList = project.MainData, TotalRecords = project.OtherData });
                }
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetProject(int id)
        {
            var result = await GetAsync<ProjectModel>(id);
            if (result is ActionResult actionResult)
            {
                return actionResult;
            }
            else
            {
                var project = result as ProjectModel;
                return Json(project);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProject(ProjectModel model)
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

        public ActionResult AddSite()
        {
            return View();
        }

        public ActionResult SiteList()
        {
            return View();
        }

        //For User
        public ActionResult SiteBidList()
        {
            return View();
        }

    }
}