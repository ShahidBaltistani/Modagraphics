using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    public class ListCounterController : BaseController  
    {
        public ListCounterController(IHttpContextAccessor httpContext) : base(httpContext) { }
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var project = await CountAsync<Project>(x =>x.IsDeleted);
                //var VehicelTypes = await CountAsync<Project>(x =>x.IsDeleted);
                //var project = await CountAsync<Project>(x =>x.IsDeleted);
                

                return Ok(
                    new DashboardModel
                    {
                        //TotalProjects = project                       
                    });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpGet("GetAdminDashboardData")]
        public async Task<IActionResult> GetAdminDashboardData()
        {
            try
            {
                var TotalProject = await CountAsync<Project>(x=> !x.IsDeleted);
                var TotalSites = await CountAsync<Site>(x => !x.IsDeleted);
                var Activeprojects = await Entities.Project.Where(x => !x.IsDeleted).Select(x => x.Id).ToListAsync();
                var active_project = await Entities.Site.Where(x => Activeprojects.Contains(x.ProjectId)).ToListAsync();
                var noOfActiveSite = await CountAsync<Site>(x => !x.IsDeleted && x.SiteStatus == SiteStatus.OpenForBid);

                DashboardModel model = new DashboardModel {
                    ActiveProjects = (uint)Activeprojects.Count(),
                    ActiveSites = noOfActiveSite,
                    TotalSites = TotalSites,
                    TotalProjects = TotalProject
                };
                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };
                return Ok(list);

            }catch(Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        [HttpGet("GetInstallerDashboardData")]
        public async Task<IActionResult> GetInstallerDashboardData()
       {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var TotalCrews = await CountAsync<Crew>(x => !x.IsDeleted && x.InstallerId == AssociatedId);
                var noOfOpenSites = await CountAsync<Site>(x => !x.IsDeleted && x.SiteStatus == SiteStatus.OpenForBid);
                var TotalBids = await CountAsync<Bid>(x => !x.IsDeleted && x.InstallerId == AssociatedId && x.Status == BidStatus.Pending);
                var AssignedSites = await CountAsync<Bid>(x => !x.IsDeleted && x.InstallerId == AssociatedId && x.Status == BidStatus.Assigned);
                var TotalSites = await CountAsync<Site>(x => !x.IsDeleted);

                DashboardModel model = new DashboardModel
                {
                    TotalCrews = TotalCrews,
                    TotalBids = TotalBids,
                    OpenSites = noOfOpenSites,
                    AssignedSites = AssignedSites,
                    TotalSites = TotalSites
                };

                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };

                return Ok(list);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("GetFleetownerDashboardData")]
        public async Task<IActionResult> GetFleetownerDashboardData()
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var TotalSupervisors = await CountAsync<Supervisor>(x => !x.IsDeleted && x.FleetOwnerId == AssociatedId);
                var SiteIds = await Entities.Site.Where(x => !x.IsDeleted && x.FleetOwnerId == AssociatedId).Select(x => x.Id).ToListAsync();
                var TotalVehicals = Entities.VehicleTypeSiteAssociation.Where(x => SiteIds.Contains(x.SiteId) && !x.IsDeleted).Count();
                var CompletedJobs = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.Completed).Count();
                var Approvals = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.UnderReviewSupervisor).Count();

                DashboardModel model = new DashboardModel
                {
                    TotalSupervisors = TotalSupervisors,
                    TotalVehicals = (uint)TotalVehicals,
                    CompletedJobs = (uint)CompletedJobs,
                    Approvals = (uint)Approvals
                };
                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };

                return Ok(list);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        [HttpGet("GetSupervisorDashboardData")]
        public async Task<IActionResult> GetSupervisorDashboardData()
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var TotalSites = await CountAsync<SupervisorSiteAssociation>(x => x.SupervisorId == AssociatedId && !x.IsDeleted);
                var SiteIds = await Entities.SupervisorSiteAssociation.Where(x => x.SupervisorId == AssociatedId).Select(x => x.SiteId).ToListAsync();
                var JobWaitingForApprovals = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.UnderReviewSupervisor).Count();
                var IncidentReports = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.IncidentReported).Count();

                DashboardModel model = new DashboardModel
                {
                    TotalSites = TotalSites,
                    Approvals = (uint)JobWaitingForApprovals,
                    TotalIncidentReports = (uint)IncidentReports
                };
                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };

                return Ok(list);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        [HttpGet("GetApprovalManagerDashboardData")]
        public async Task<IActionResult> GetApprovalManagerDashboardData()
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var UserApprovals = await CountAsync<User>(x => !x.IsDeleted && x.Status == UserStatus.WaitingForApproval);
                var JobApprovals = await CountAsync<Job>(x => !x.IsDeleted && x.Status == JobStatus.UnderReviewApprovalManager);


                DashboardModel model = new DashboardModel
                {
                    UserApprovals = UserApprovals,
                    JobApprovals = JobApprovals
                };
                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };

                return Ok(list);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        [HttpGet("GetCrewDashboardData")]
        public async Task<IActionResult> GetCrewDashboardDataI()
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var AssignedSites = await CountAsync<CrewSite>(x=>x.SiteId == AssociatedId);
                var SiteIds = await Entities.CrewSites.Where(x => x.CrewId == AssociatedId).Select(x => x.SiteId).ToListAsync();
                var AssignedJobs = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.InQueue).Count();
                

                DashboardModel model = new DashboardModel
                {
                    AssignedJobs = (uint)AssignedJobs,
                    AssignedSites = AssignedSites
                };
                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };

                return Ok(list);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("GetCorporateUserDashboardData")]
        public async Task<IActionResult> GetCorporateUserDashboardData()
        {
            try
            {
                var AssociatedId = await GetAssociatedId();
                var FleetOwnerId = await Entities.FleetOwner.Where(x => !x.IsDeleted && x.UserCorporateId == AssociatedId).Select(x=> x.Id).ToListAsync();
              
                 var SiteIds = await Entities.Site.Where(x => !x.IsDeleted && FleetOwnerId.Contains(x.FleetOwnerId) ).Select(x => x.Id).ToListAsync();
                var TotalVehicals = Entities.VehicleTypeSiteAssociation.Where(x => SiteIds.Contains(x.SiteId) && !x.IsDeleted).Count();
                var CompletedJobs = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.Completed).Count();
                var CompletedSites = Entities.Site.Where(x => SiteIds.Contains(x.Id) && x.SiteStatus == SiteStatus.ReadyForInvoice).Count();
                var TotalSites = await Entities.Site.Where(x => !x.IsDeleted && FleetOwnerId.Contains(x.FleetOwnerId) ).Select(x => x.Id).CountAsync();
                var TotalInProgressSites = await Entities.Site.Where(x => !x.IsDeleted && FleetOwnerId.Contains(x.FleetOwnerId) && (x.SiteStatus == SiteStatus.Awarded || x.SiteStatus == SiteStatus.Assigned)).Select(x => x.Id).CountAsync();
                var JobWaitingForApprovals = Entities.Job.Where(x => SiteIds.Contains(x.SiteId) && x.Status == JobStatus.UnderReviewSupervisor).Count();
                DashboardModel model = new DashboardModel
                {
                    TotalFleetOwner = (uint) FleetOwnerId.Count(),
                    TotalVehicals = (uint)TotalVehicals,
                    CompletedJobs = (uint)CompletedJobs,
                    CompletedSites = (uint)CompletedSites,
                    TotalSites = (uint)TotalSites,
                    TotalInProgressSites = (uint)TotalInProgressSites,
                    JobApprovals = (uint)JobWaitingForApprovals
                };
                List<DashboardModel> list = new List<DashboardModel>
                {
                    model
                };

                return Ok(list);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }
    }
}