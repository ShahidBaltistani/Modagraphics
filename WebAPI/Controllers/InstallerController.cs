using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using Database.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;
using Microsoft.EntityFrameworkCore;
using Database;
using System.Globalization;

namespace WebAPI.Controllers
{
    public class InstallerController : BaseController
    {
        public InstallerController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet("GetVehiclesAgainstAssignedSites/{Id}")]
        public async Task<IActionResult> GetVehiclesAgainstAssignedSites(int Id)
        {
            try
            {
                var joinresult = Entities.VehicleTypeSiteAssociation.Where(x => x.SiteId == Id && !x.IsDeleted).Join(Entities.VehicleTypes, x => x.VehicleTypeId, v => v.Id,
                (a, b) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    VehicleTypeName = b.Name,
                    VehiclePrice=b.BasePrice,
                    SideHeight=a.SideHeight,
                    SideWidth=a.SideWidth,
                    FrontHeight=a.FrontHeight,
                    FrontWidth=a.FrontWidth,
                    RearHeight=a.RearHeight,
                    RearWidth=a.RearWidth
                });
                var newlist = joinresult.ToList();

                foreach (var item in newlist)
                {
                    item.Vehicles = await CountAsync<Job>(x => x.SiteVehicleTypeId == item.Id);
                }
                return Ok(newlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetVehiclesAgainstAwardedSites/{Id}")]
        public async Task<IActionResult> GetVehiclesAgainstAwardedSites(int Id)
        {
            try
            {
                var joinresult = Entities.VehicleTypeSiteAssociation.Where(x => x.SiteId == Id && !x.IsDeleted).Join(Entities.VehicleTypes, x => x.VehicleTypeId, v => v.Id,
                (a, b) => new SiteVehicleTypeModel
                {
                    Id = a.Id,
                    VehicleTypeName = b.Name,
                    VehiclePrice = b.BasePrice,
                    SideHeight = a.SideHeight,
                    SideWidth = a.SideWidth,
                    FrontHeight = a.FrontHeight,
                    FrontWidth = a.FrontWidth,
                    RearHeight = a.RearHeight,
                    RearWidth = a.RearWidth
                });
                var newlist = joinresult.ToList();

                foreach (var item in newlist)
                {
                    item.Vehicles = await CountAsync<Job>(x => x.SiteVehicleTypeId == item.Id);
                }
                return Ok(newlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var installer = await ToListAsync<Installer>(x => !x.IsDeleted);
                return Ok(installer.Select(x =>
                    new InstallerModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        EmailAddress = x.EmailAddress,
                        PhoneNo = x.PhoneNo,
                        Address = x.Address,
                        PZipCode = x.PZipCode,
                        Picture = x.Picture,
                        TravelInMiles = x.TravelInMiles,
                        IsPreferred = x.IsPreferred,
                        CompanyName = x.CompanyName,
                        BillToAddress = x.BillToAddress,
                        CZipCode = x.CZipCode,
                        FaxNo = x.FaxNo,
                        CContactNo = x.CContactNo,
                        CompanyEmail = x.CompanyEmail,
                        FEINumber = x.FEINumber,
                        NoOfEmployees = x.NoOfEmployees,
                        CertificationTraining = x.CertificationTraining,
                        IsInstallersEmployee = x.IsInstallersEmployee,
                        IsEmployeesBackgroundCheck = x.IsEmployeesBackgroundCheck,
                        IsDrugTest = x.IsDrugTest,
                        EquipmentOwned = x.EquipmentOwned,
                        InstallLocations = x.InstallLocations,
                        InstallProjectCompleted = x.InstallProjectCompleted,
                        YearInBusiness = x.YearInBusiness,
                        IsEmailVerified = x.IsEmailVerified,
                        Status = x.Status,
                        CCity = x.CCityId,
                        PCity = x.PCityId,
                    }).ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet]
        [Route("Image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var installer = await FirstOrDefaultAsync<Installer>(model => model.Id == id);
                return installer == null ? (IActionResult)NotFound() : Ok(installer.Picture);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var installer = await FirstOrDefaultAsync<Installer>(model => model.Id == id);
                return installer == null ? (IActionResult)NotFound() : Ok(new InstallerModel
                {
                    Id = installer.Id,
                    FirstName = installer.FirstName,
                    LastName = installer.LastName,
                    EmailAddress = installer.EmailAddress,
                    PhoneNo = installer.PhoneNo,
                    Address = installer.Address,
                    PZipCode = installer.PZipCode,
                    Picture = installer.Picture,
                    TravelInMiles = installer.TravelInMiles,
                    IsPreferred = installer.IsPreferred,
                    CompanyName = installer.CompanyName,
                    BillToAddress = installer.BillToAddress,
                    CZipCode = installer.CZipCode,
                    FaxNo = installer.FaxNo,
                    CContactNo = installer.CContactNo,
                    CompanyEmail = installer.CompanyEmail,
                    FEINumber = installer.FEINumber,
                    NoOfEmployees = installer.NoOfEmployees,
                    CertificationTraining = installer.CertificationTraining,
                    IsInstallersEmployee = installer.IsInstallersEmployee,
                    IsEmployeesBackgroundCheck = installer.IsEmployeesBackgroundCheck,
                    IsDrugTest = installer.IsDrugTest,
                    EquipmentOwned = installer.EquipmentOwned,
                    InstallLocations = installer.InstallLocations,
                    InstallProjectCompleted = installer.InstallProjectCompleted,
                    YearInBusiness = installer.YearInBusiness,
                    IsEmailVerified = installer.IsEmailVerified,
                    Status = installer.Status,
                    CCity = installer.CCityId,
                    PCity = installer.PCityId,

                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("Page")]
        public async Task<IActionResult> PostPage([FromBody] InstallerPaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Installer, bool>> filter = x =>
                    (paginationModel.FirstName == "" || x.FirstName.ToLower().Contains(paginationModel.FirstName.ToLower()))
                    && (paginationModel.LastName == "" || x.LastName.ToLower().Contains(paginationModel.LastName.ToLower()))
                    && (paginationModel.Email == "" || x.EmailAddress.ToLower().Contains(paginationModel.Email.ToLower()))
                    && (paginationModel.Company == "" || x.CompanyName.ToLower().Contains(paginationModel.Company.ToLower()))
                    && !x.IsDeleted;

                var joinResult = Entities.Installer.Where(filter).OrderByDescending(x => x.Id).Select(x =>
                new InstallerModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmailAddress = x.EmailAddress,
                    CompanyName = x.CompanyName,
                    PhoneNo = x.PhoneNo,
                    Status = x.Status
                });

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result)
                {
                    x.AwardedSites = await CountAsync<Bid>(y => y.InstallerId == x.Id && y.Status == BidStatus.Awarded && !y.IsDeleted);
                    x.AssignedSites = await CountAsync<Bid>(y => y.InstallerId == x.Id && y.Status == BidStatus.Assigned && !y.IsDeleted);
                    x.TotalCrew = await CountAsync<Crew>(y => y.InstallerId == x.Id && !y.IsDeleted);
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("BidHistory")]
        public async Task<IActionResult> PostBidHistory([FromBody] InstallerPaginationModel model) {
            try {

                var InstallerId = await GetAssociatedId();
                Expression<Func<Bid, bool>> filter = x => x.InstallerId == InstallerId && !x.IsDeleted;

                var result = Entities.Bid.Where(filter).Join(Entities.Site, b => b.SiteId, s => s.Id,
                    (a,b) => new
                    {
                        a.Id,
                        a.Price,
                        a.Status,
                        a.SiteId,
                        a.InstallerId,
                        a.CreatedDate,
                        b.Name,
                        b.ProjectId
                    }).Join(Entities.Project, m => m.ProjectId, p=>p.Id, 
                    (s, f) => new BidModel
                    {
                        Id = s.Id,
                        SiteId = s.SiteId,
                        SiteName = s.Name,
                        Price = s.Price,
                        InstallerId = s.InstallerId,
                        ProjectName = f.Name,
                        Status = s.Status,
                        date = s.CreatedDate
                    }).Where(z=>
                    
                    (model.FirstName=="" || z.ProjectName.ToLower().Contains(model.FirstName.ToLower()) )
                    && (model.LastName=="" || z.SiteName.ToLower().Contains(model.LastName.ToLower()))
                    &&(model.Price==0 || z.Price==model.Price)
                    &&(model.AwardedDate==null || z.date.Date==model.AwardedDate.GetValueOrDefault().Date)
                    );

               

                var count = await result.CountAsync();
                var final_result = await result.GetPageAsync(model);
                

                return Ok(final_result, count);
            }
            catch(Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        [HttpPost("AwardedSites")]
        public async Task<IActionResult> PostAwardedSites([FromBody] InstallerPaginationModel model)
        {
            try
            {
                var InstallerId = await GetAssociatedId();
                Expression<Func<Bid, bool>> filter = x => x.InstallerId == InstallerId && !x.IsDeleted && x.Status == BidStatus.Awarded;
                var joinResult = Entities.Bid.Where(filter).Join(Entities.Site, b => b.SiteId, s => s.Id,
                    (a, b) => new {
                        a.Id,
                        a.SiteId,
                        a.Status,
                        a.CreatedDate,
                        b.ProjectId,
                        SiteName = b.Name
                    }).Join(Entities.Project, x => x.ProjectId, p => p.Id, (x, y) => new BidModel
                    {
                        Id = x.Id,
                        SiteId = x.SiteId,
                        Status = x.Status,
                        SiteName = x.SiteName,
                        ProjectName = y.Name,
                    }).Where(z=> 
                    
                    (model.FirstName=="" || z.ProjectName.ToLower().Contains(model.FirstName.ToLower())) 
                    && (model.LastName=="" || z.SiteName.ToLower().Contains(model.LastName.ToLower()))

                    );

                var count = await joinResult.CountAsync();
                var page = await joinResult.GetPageAsync(model);

                foreach (var x in page.ToList())
                {
                    x.SiteVehicals = await CountAsync<Job>(y => y.SiteId == x.SiteId && !y.IsDeleted);
                    //x.AwardedDate = (await LastOrDefaultAsync<SiteStatusLog>(y => y.SiteId == x.SiteId && y.SiteStatus == SiteStatus.Awarded)).CreatedDate;
                    
                    var sitestatuslog=await LastOrDefaultAsync<SiteStatusLog>(y => y.SiteId == x.SiteId && y.SiteStatus == SiteStatus.Awarded
                    
                    && (model.AwardedDate==null || y.CreatedDate.Date==model.AwardedDate.GetValueOrDefault().Date)
                    );
                    if (sitestatuslog!=null)
                    {
                        x.AwardedDate = sitestatuslog.CreatedDate;
                    }
                    else if (!(model.AwardedDate==null))
                    {
                        page.Remove(x);

                    }
                }

                return Ok(page, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        //[HttpPost("AwardedSites")]
        //public async Task<IActionResult> PostAwardedSites([FromBody] InstallerPaginationModel model)
        //{
        //    try{
        //        var InstallerId = await GetAssociatedId();
        //        Expression<Func<Bid, bool>> filter = x => x.InstallerId == InstallerId && !x.IsDeleted && x.Status == BidStatus.Awarded;
        //        var joinResult = Entities.Bid.Where(filter).Join(Entities.Site, b => b.SiteId, s => s.Id,
        //            (a, b) => new {
        //                a.Id,
        //                a.SiteId,
        //                a.Status,
        //                a.CreatedDate,
        //                b.ProjectId,
        //                SiteName = b.Name
        //            }).Join(Entities.Project, x=> x.ProjectId, p => p.Id, (x,y) => new BidModel{
        //                Id = x.Id,
        //                SiteId = x.SiteId,
        //                Status = x.Status,
        //                SiteName = x.SiteName,
        //                ProjectName = y.Name,
        //            });

        //        var count = await joinResult.CountAsync();
        //        var page = await joinResult.GetPageAsync(model);

        //        foreach (var x in page)
        //        {
        //            x.SiteVehicals = await CountAsync<Job>(y => y.SiteId == x.SiteId && !y.IsDeleted);
        //            x.AwardedDate = (await LastOrDefaultAsync<SiteStatusLog>(y => y.SiteId == x.SiteId && y.SiteStatus == SiteStatus.Awarded)).CreatedDate;
        //        }

        //        return Ok(page, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        return MethodFailure(ex);
        //    }
        //}

        [HttpPost("AssignSite")]
        public async Task<IActionResult> PostAssignSite([FromBody] ChildEntries model)
        {
            try
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        foreach (var x in model.ChildIds)
                        {
                            var crew = new CrewSite
                            {
                                CrewId = x,
                                SiteId = model.MainId,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync(crew);
                        }

                        var bid = await FirstOrDefaultAsync<Bid>(i => i.SiteId == model.MainId && i.Status == BidStatus.Awarded && !i.IsDeleted);
                        bid.Status = BidStatus.Assigned;
                        bid.ModifiedDate = UserClaims.DateTime;
                        bid.ModifiedBy = UserClaims.UserId;
                        await UpdateAsync(bid);

                        var site = await FirstOrDefaultAsync<Site>(i => i.Id == model.MainId && !i.IsDeleted);
                        site.SiteStatus = SiteStatus.Assigned;
                        site.ModifiedDate = UserClaims.DateTime;
                        site.ModifiedBy = UserClaims.UserId;
                        await UpdateAsync(site);

                        var jobs = await ToListAsync<Job>(i => i.SiteId == model.MainId);
                        foreach(var x in jobs)
                        {
                            x.Status = JobStatus.InQueue;
                            x.ModifiedDate = UserClaims.DateTime;
                            x.ModifiedBy = UserClaims.UserId;   
                            await UpdateAsync(x);



                            // Code for Job-Status-Log ---By :Kashif Shahzad
                            var log = new JobStatusLogs
                            {
                                Id = 0,
                                JobId = x.Id,
                                Status = JobStatus.InQueue,
                                CreatedBy = UserClaims.UserId,
                                CreatedDate = UserClaims.DateTime,
                                TZOSCreatedBy = UserClaims.TimeZoneOffset
                            };
                            await InsertAsync(log);
                            // Code for Job-Status-Log

                        }

                        var siteLog = new SiteStatusLog
                        {
                            SiteId = model.MainId,
                            SiteStatus = SiteStatus.Assigned,
                            CreatedBy = UserClaims.UserId,
                            CreatedDate = UserClaims.DateTime,
                            TZOSCreatedBy = UserClaims.TimeZoneOffset
                        };
                        await InsertAsync(siteLog);

                        tran.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("AssignedSitesList")]
        public async Task<IActionResult> PostAssignedSiteList([FromBody] InstallerPaginationModel model)
        {
            try
            {
                
                var InstallerID = await GetAssociatedId();
                Expression<Func<Bid, bool>> filter = x => x.InstallerId == InstallerID && !x.IsDeleted && x.Status == BidStatus.Assigned;
                var joinResult = Entities.Bid.Where(filter).Join(Entities.Site, b => b.SiteId, s => s.Id,
                    (a, b) => new
                    {
                        bidID = a.Id,
                        a.SiteId,
                        SiteName = b.Name,
                        ProjectId = b.ProjectId
                    }).Join(Entities.Project, s => s.ProjectId, p => p.Id, (x, y) => new BidModel
                    {
                        Id = x.bidID,
                        SiteId = x.SiteId,
                        SiteName = x.SiteName,
                        ProjectName = y.Name,
                       
                    }).Where(z=>
                    (model.FirstName=="" || z.ProjectName.ToLower().Contains(model.FirstName.ToLower()))
                    && (model.LastName=="" || z.SiteName.ToLower().Contains(model.LastName.ToLower()))

                    );
                //var joinResult = Entities.Bid.Where(filter).Join(Entities.Site, b => b.SiteId, s => s.Id,
                //    (a, b) => new
                //    {
                //        bidID = a.Id,
                //        a.SiteId,
                //        SiteName = b.Name,
                //        ProjectId = b.ProjectId
                //    }).Join(Entities.Project, s => s.ProjectId, p => p.Id, (x, y) => new
                //    {
                //        Id = x.bidID,
                //        SiteId = x.SiteId,
                //        SiteName = x.SiteName,
                //        ProjectName = y.Name,
                //    }).Join(Entities.CrewSites, x=>x.SiteId, p => p.SiteId, (c, d) => new BidModel
                //    {
                //        Id = c.Id,
                //        SiteId = c.SiteId,
                //        SiteName = c.SiteName,
                //        ProjectName = c.ProjectName,
                //        AssignDate=d.CreatedDate,
                //        //CrewCount=Count<CrewSite>(e=>e.SiteId == c.SiteId)
                //    });

                var count = await joinResult.CountAsync();
                var page = await joinResult.GetPageAsync(model);

                foreach (var x in page.ToList())
                {
                    x.SiteVehicals = await CountAsync<Job>(y => y.SiteId == x.SiteId && !y.IsDeleted);
                    x.CrewCount= await CountAsync<CrewSite>(y => y.SiteId == x.SiteId);

                    //x.AssignDate = (await FirstOrDefaultAsync<SiteStatusLog>(e => e.SiteId == x.SiteId && e.SiteStatus == SiteStatus.Assigned)).CreatedDate;
                    //x.AssignDate = Entities.SiteStatusLog.Where(e => e.SiteId == x.SiteId && e.SiteStatus == SiteStatus.Assigned).Select(
                    //    l=>DateTime.ParseExact(l.CreatedDate.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    //    ).FirstOrDefault();

                    var siteStatusLog = await FirstOrDefaultAsync<SiteStatusLog>(e => e.SiteId == x.SiteId && e.SiteStatus == SiteStatus.Assigned
                 //   &&(model.AwardedDate == null || e.CreatedDate.Date.ToString("MM/dd/yyyy") == model.AwardedDate.GetValueOrDefault().Date.ToString("MM/dd/yyyy"))
                    && (model.AwardedDate == null || e.CreatedDate.Date == model.AwardedDate.Value.Date)
                    );
                   
                    if (siteStatusLog != null)
                    {
                        x.AssignDate = siteStatusLog.CreatedDate;
                    }
                    else if (!(model.AwardedDate == null ))
                    {
                        page.Remove(x);
                    }
                }
                return Ok(page, count);
            }



            catch (Exception ex)
            {

                return MethodFailure(ex);
            }
        }


        [HttpGet("GetCrewsAgainstSites/{id}")]
        public IActionResult GetCrewsAgainstSites(int id)
        {
            try
            {
                Expression<Func<CrewSite, bool>> filter = x => x.SiteId == id;

                var joinresult =Entities.CrewSites.Where(filter).Join(Entities.Crew, x => x.CrewId, c => c.Id,
                (a, b) => new Crew
                {
                    Id = b.Id,
                    FirstName=b.FirstName,
                    LastName=b.LastName,
                    EmailAddress=b.EmailAddress,
                    PhoneNo=b.PhoneNo
                });
                
                return Ok(joinresult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InstallerModel installer)
        {
            try
            {
                var vInstaller = new Installer
                {
                    Id = installer.Id,
                    FirstName = installer.FirstName,
                    LastName = installer.LastName,
                    EmailAddress = installer.EmailAddress,
                    PhoneNo = installer.PhoneNo,
                    Address = installer.Address,
                    PZipCode = installer.PZipCode,
                    Picture = installer.Picture,
                    TravelInMiles = installer.TravelInMiles,
                    IsPreferred = installer.IsPreferred,
                    CompanyName = installer.CompanyName,
                    BillToAddress = installer.BillToAddress,
                    CZipCode = installer.CZipCode,
                    FaxNo = installer.FaxNo,
                    CContactNo = installer.CContactNo,
                    CompanyEmail = installer.CompanyEmail,
                    FEINumber = installer.FEINumber,
                    NoOfEmployees = installer.NoOfEmployees,
                    CertificationTraining = installer.CertificationTraining,
                    IsInstallersEmployee = installer.IsInstallersEmployee,
                    IsEmployeesBackgroundCheck = installer.IsEmployeesBackgroundCheck,
                    IsDrugTest = installer.IsDrugTest,
                    EquipmentOwned = installer.EquipmentOwned,
                    InstallLocations = installer.InstallLocations,
                    InstallProjectCompleted = installer.InstallProjectCompleted,
                    YearInBusiness = installer.YearInBusiness,
                    IsEmailVerified = installer.IsEmailVerified,
                    Status = installer.Status,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                    CCityId = installer.CCity,
                    PCityId = installer.PCity,
                    
                };
                await InsertAsync(vInstaller);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]InstallerModel installer)
        {
            try
            {
                if (!Entities.Installer.Any(model => model.Id == id))
                {
                    return NotFound();
                }

                var vInsatller = await FirstOrDefaultAsync<Installer>(model => model.Id == id);
                vInsatller.FirstName = installer.FirstName;
                vInsatller.LastName = installer.LastName;
                vInsatller.EmailAddress = installer.EmailAddress;
                vInsatller.PhoneNo = installer.PhoneNo;
                vInsatller.Address = installer.Address;
                vInsatller.PZipCode = installer.PZipCode;
                vInsatller.Picture = installer.Picture;
                vInsatller.TravelInMiles = installer.TravelInMiles;
                vInsatller.IsPreferred = installer.IsPreferred;
                vInsatller.CompanyName = installer.CompanyName;
                vInsatller.BillToAddress = installer.BillToAddress;
                vInsatller.CZipCode = installer.CZipCode;
                vInsatller.FaxNo = installer.FaxNo;
                vInsatller.CContactNo = installer.CContactNo;
                vInsatller.CompanyEmail = installer.CompanyEmail;
                vInsatller.FEINumber = installer.FEINumber;
                vInsatller.NoOfEmployees = installer.NoOfEmployees;
                vInsatller.CertificationTraining = installer.CertificationTraining;
                vInsatller.IsInstallersEmployee = installer.IsInstallersEmployee;
                vInsatller.IsEmployeesBackgroundCheck = installer.IsEmployeesBackgroundCheck;
                vInsatller.IsDrugTest = installer.IsDrugTest;
                vInsatller.EquipmentOwned = installer.EquipmentOwned;
                vInsatller.InstallLocations = installer.InstallLocations;
                vInsatller.InstallProjectCompleted = installer.InstallProjectCompleted;
                vInsatller.YearInBusiness = installer.YearInBusiness;
                vInsatller.IsEmailVerified = installer.IsEmailVerified;
                vInsatller.Status = installer.Status;
                vInsatller.IsDeleted = false;
                vInsatller.ModifiedBy = UserClaims.UserId;
                vInsatller.ModifiedDate = UserClaims.DateTime;
                vInsatller.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                vInsatller.CCityId = installer.CCity;
                vInsatller.PCityId = installer.PCity;

                await UpdateAsync(vInsatller);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var installer = await FirstOrDefaultAsync<Installer>(x => x.Id == id);
                if (installer is null) return NotFound();
                installer.IsDeleted = true;
                installer.ModifiedBy = UserClaims.UserId;
                installer.ModifiedDate = UserClaims.DateTime;
                installer.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(installer);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("Dropdown")]
        public async Task<IActionResult> DropDown()
        {
            try
            {
                var project = await SelectAsync<Installer, DropdownModel>(x => !x.IsDeleted && x.Status == InstallerStatus.Approved, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                return Ok(project.ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("DropdownForAddLogin")]
        public async Task<IActionResult> DropDownForAddLogin()
        {
            try
            {
                List<DropdownModel> dropdowns = new List<DropdownModel>();

                var project = await SelectAsync<Installer, DropdownModel>(x => !x.IsDeleted && x.Status == InstallerStatus.Approved, x => new DropdownModel
                {
                    Id = x.Id,
                    Value = x.FirstName + " " + x.LastName
                });
                foreach (var item in project.ToList())
                {
                    bool isExist = await AnyAsync<Login>(x => x.AssociatedId == item.Id && x.Role == UserRoles.Installer);
                    if (!isExist)
                    {
                        dropdowns.Add(item);
                    }
                }
                return Ok(dropdowns);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        //[HttpGet("IsUniqueEmail/{id}/{email}")]
        //public async Task<IActionResult> VerifyEmail(uint id, string email)
        //{
        //    try
        //    {
        //        return Ok(!await AnyAsync<Installer>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower()));
        //    }
        //    catch (Exception ex)
        //    {
        //        return MethodFailure(ex);
        //    }
        //}
        [HttpGet("IsUniqueEmail/{id}/{email}")]
        public async Task<IActionResult> VerifyEmail(uint id, string email)
        {
            try
            {
                return Ok(!await AnyAsync<Installer>(x => x.Id != id && x.EmailAddress.ToLower() == email.ToLower())

                    && !await AnyAsync<CorporateUser>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<FleetOwner>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<User>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Crew>(x => x.EmailAddress.ToLower() == email.ToLower())
                    && !await AnyAsync<Supervisor>(x => x.EmailAddress.ToLower() == email.ToLower())
                    );
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }


        // Job Log

        [HttpPost("JobList")]
        public async Task<IActionResult> GetJobLog(CrewJobPaginationModel paginationModel)
        {
            try
            {
                var installerId = await GetAssociatedId();
                Expression<Func<Bid, bool>> filter1 = x =>
                  x.InstallerId == installerId
                  && (x.Status == BidStatus.Assigned
                  || x.Status == BidStatus.Awarded)
                  && !x.IsDeleted;

                var joinResult = from b in Entities.Bid.Where(filter1)
                                 join j in Entities.Job
                                       on b.SiteId equals j.SiteId
                                 join s in Entities.Site
                                       on j.SiteId equals s.Id
                                 join p in Entities.Project
                                    on s.ProjectId equals p.Id
                                 where s.IsDeleted == false

                                 select new CrewSiteModel
                                 {
                                     ProjectName = p.Name,
                                     SiteName = s.Name,
                                     VehicleTypeName = string.Empty,
                                     VIN = j.VIN,
                                     LPN = j.LPN,
                                     UnitNo = j.UnitNo,
                                     Status = j.Status,
                                     SiteId = s.Id,
                                     JobId = j.Id,
                                     StartDate = null
                                 };


              

              var  result = (joinResult
                   .Where(c =>c.Status != 0
                       &&  (paginationModel.VehicleTypeName == "" || c.VehicleTypeName.ToLower().Contains(paginationModel.VehicleTypeName.ToLower()))
                       && (paginationModel.SiteName == "" || c.SiteName.ToLower().Contains(paginationModel.SiteName.ToLower()))
                       && (paginationModel.ProjectName == "" || c.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()))
                       && (paginationModel.VIN == "" || c.VIN.ToLower().Contains(paginationModel.VIN.ToLower()))
                       && (paginationModel.LPN == "" || c.LPN.ToLower().Contains(paginationModel.LPN.ToLower()))
                       && (paginationModel.JobStatus == null || (int)c.Status ==paginationModel.JobStatus))
                   
                   ).ToList();
                var count = result.Count();
                 result = result.GetPage(paginationModel);

                foreach (var item in result)
                {
                    var vtid = await FirstOrDefaultAsync<SiteVehicleType>(x => x.SiteId == item.SiteId);
                    item.VehicleTypeName = (await FirstOrDefaultAsync<VehicleType>(x => x.Id == vtid.VehicleTypeId)).Name;
                }

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

    }
}
