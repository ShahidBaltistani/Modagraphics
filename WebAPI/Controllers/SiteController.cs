using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using WebAPI.Extension;

namespace WebAPI.Controllers
{
    public class SiteController : BaseController
    {
        public SiteController(IHttpContextAccessor httpContext) : base(httpContext) { }

        // GET: <controller>

        [HttpGet("IsUniqueName/{id}")]
        public async Task<IActionResult> AssociationVerification(int id)
        {
            try
            {

                return Ok(await AnyAsync<SupervisorSiteAssociation>(x => x.SiteId == id));
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var site = await ToListAsync<Site>(x => !x.IsDeleted);
                return Ok(site.Select(x =>
                    new SiteModel
                    {
                        Id = x.Id,
                        FleetOwnerId = x.FleetOwnerId,
                        ProjectId = x.ProjectId,
                        Name = x.Name,
                        CSR = x.CSR,
                        SalesMan = x.SalesMan,
                        BidDueDate = x.BidDueDate,
                        SiteStartDate = x.SiteStartDate,
                        SiteEndDate = x.SiteEndDate,
                        MaximumBidAmount = x.MaximumBidAmount,
                        SalePrice = x.SalePrice,
                        ScopeOfWork = x.ScopeOfWork,
                        SpecialInstruction = x.SpecialInstruction,
                        Address = x.Address,
                        ZipCode = x.ZipCode,
                        CityId = x.CityId,
                        PONo = x.PONo,
                        EPMSJobNo = x.EPMSJobNo,
                        TakeDownStatus = x.TakeDownStatus,
                        SiteStatus = x.SiteStatus,
                        IndoorFacility = x.IndoorFacility,
                        BusinessHours = x.BusinessHours,
                        WeekendHours = x.WeekendHours,
                        IsSpottingTractor = x.IsSpottingTractor,
                        StoreEquipmentOnSite = x.StoreEquipmentOnSite,
                        SpotVehicle = x.SpotVehicle,
                        RemoveExistingGraphics = x.RemoveExistingGraphics,
                        YearsOnVehicle = x.YearsOnVehicle,
                        CleaningAndPrepVehicle = x.CleaningAndPrepVehicle,
                        PowerWash = x.PowerWash,
                        IsVinylGraphics = x.IsVinylGraphics,
                        SealOfDecals = x.SealOfDecals,
                        ArlonVinylFilmRemoval = x.ArlonVinylFilmRemoval,
                        ArlonReflectiveFilmApply = x.ArlonReflectiveFilmApply,
                        AveryVinylFilmRemoval = x.AveryVinylFilmRemoval,
                        AveryVinylFilmApply = x.AveryVinylFilmApply,
                        M3VinylFilmRemoval = x.M3VinylFilmRemoval,
                        M3VinylFilmApply = x.M3VinylFilmApply,
                        ArlonReflectiveFilmRemoval = x.ArlonReflectiveFilmRemoval,
                        ArlonVinylFilmApply = x.ArlonVinylFilmApply,
                        AveryReflectiveFilmRemoval = x.AveryReflectiveFilmRemoval,
                        AveryReflectiveFilmApply = x.AveryReflectiveFilmApply,
                        M3ReflectiveFilmRemoval = x.M3ReflectiveFilmRemoval,
                        M3ReflectiveFilmApply = x.M3ReflectiveFilmApply,
                        TotalSqFtRemoval = x.TotalSqFtRemoval,
                        TotalSqFtApply = x.TotalSqFtApply,
                        OtherQuestionsComments = x.OtherQuestionsComments,
                        CustomerAvailabilityOnInstallationDay = x.CustomerAvailabilityOnInstallationDay,
                        InsideFacilities = x.InsideFacilities,
                        IsDecalsReceived = x.IsDecalsReceived,
                        VehicleCleanessBeforeArrival = x.VehicleCleanessBeforeArrival,
                        IsInformed6FeetArea = x.IsInformed6FeetArea,
                        VehicleAvailabilityDate = x.VehicleAvailabilityDate,
                        WorkHoursAtFacility = x.WorkHoursAtFacility,
                        AdditionalContacts = x.AdditionalContacts,
                    }).ToList());
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        // GET <controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var site = await FirstOrDefaultAsync<Site>(model => model.Id == id);
                return site == null ? (IActionResult)NotFound() : Ok(new SiteModel
                {
                    Id = site.Id,
                    FleetOwnerId = site.FleetOwnerId,
                    ProjectId = site.ProjectId,
                    Name = site.Name,
                    CSR = site.CSR,
                    SalesMan = site.SalesMan,
                    BidDueDate = site.BidDueDate,
                    SiteStartDate = site.SiteStartDate,
                    SiteEndDate = site.SiteEndDate,
                    MaximumBidAmount = site.MaximumBidAmount,
                    SalePrice = site.SalePrice,
                    ScopeOfWork = site.ScopeOfWork,
                    SpecialInstruction = site.SpecialInstruction,
                    Address = site.Address,
                    ZipCode = site.ZipCode,
                    CityId = site.CityId,
                    PONo = site.PONo,
                    EPMSJobNo = site.EPMSJobNo,
                    TakeDownStatus = site.TakeDownStatus,
                    SiteStatus = site.SiteStatus,
                    IndoorFacility = site.IndoorFacility,
                    BusinessHours = site.BusinessHours,
                    WeekendHours = site.WeekendHours,
                    IsSpottingTractor = site.IsSpottingTractor,
                    StoreEquipmentOnSite = site.StoreEquipmentOnSite,
                    SpotVehicle = site.SpotVehicle,
                    RemoveExistingGraphics = site.RemoveExistingGraphics,
                    YearsOnVehicle = site.YearsOnVehicle,
                    CleaningAndPrepVehicle = site.CleaningAndPrepVehicle,
                    PowerWash = site.PowerWash,
                    IsVinylGraphics = site.IsVinylGraphics,
                    SealOfDecals = site.SealOfDecals,
                    ArlonVinylFilmRemoval = site.ArlonVinylFilmRemoval,
                    ArlonReflectiveFilmApply = site.ArlonReflectiveFilmApply,
                    AveryVinylFilmRemoval = site.AveryVinylFilmRemoval,
                    AveryVinylFilmApply = site.AveryVinylFilmApply,
                    M3VinylFilmRemoval = site.M3VinylFilmRemoval,
                    M3VinylFilmApply = site.M3VinylFilmApply,
                    ArlonReflectiveFilmRemoval = site.ArlonReflectiveFilmRemoval,
                    ArlonVinylFilmApply = site.ArlonVinylFilmApply,
                    AveryReflectiveFilmRemoval = site.AveryReflectiveFilmRemoval,
                    AveryReflectiveFilmApply = site.AveryReflectiveFilmApply,
                    M3ReflectiveFilmRemoval = site.M3ReflectiveFilmRemoval,
                    M3ReflectiveFilmApply = site.M3ReflectiveFilmApply,
                    TotalSqFtRemoval = site.TotalSqFtRemoval,
                    TotalSqFtApply = site.TotalSqFtApply,
                    OtherQuestionsComments = site.OtherQuestionsComments,
                    CustomerAvailabilityOnInstallationDay = site.CustomerAvailabilityOnInstallationDay,
                    InsideFacilities = site.InsideFacilities,
                    IsDecalsReceived = site.IsDecalsReceived,
                    VehicleCleanessBeforeArrival = site.VehicleCleanessBeforeArrival,
                    IsInformed6FeetArea = site.IsInformed6FeetArea,
                    VehicleAvailabilityDate = site.VehicleAvailabilityDate,
                    WorkHoursAtFacility = site.WorkHoursAtFacility,
                    AdditionalContacts = site.AdditionalContacts,
                    
                });
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("Page")]
        public async Task<IActionResult> PostPage(SitePaginationModel paginationModel)
        {
            try
            {
                Expression<Func<Site, bool>> filter = x =>
                (paginationModel.SiteName == "" || x.Name.ToLower().Contains(paginationModel.SiteName.ToLower()))
                && (paginationModel.PO == "" || x.PONo.ToLower().Contains(paginationModel.PO.ToLower()))
                && !x.IsDeleted;

                var joinResult = Entities.Site.Where(filter).OrderByDescending(x => x.Id).Join(Entities.Project, s => s.ProjectId, p => p.Id
                , (a, b) => new
                {
                    a.Id,
                    a.Name,
                    ProjectName = b.Name,
                    a.PONo,
                    b.FleetOwnerId,
                    a.SiteStatus
                }).Where(p => paginationModel.ProjectName == "" || p.ProjectName == paginationModel.ProjectName).Join(Entities.FleetOwner, s => s.FleetOwnerId, f => f.Id
                , (s, f) => new SiteModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ProjectName = s.ProjectName,
                    FleetOwnerId = f.Id,
                    FleetOwnerName = f.FirstName + " " + f.LastName,
                    PONo = s.PONo,
                    SiteStatus = s.SiteStatus
                    //TotalVehicleTypes = 5,
                    //TotalVehicles = 30,
                    //TotalBids = 10
                }).Where(f => paginationModel.FleetOwnerName == "" || f.FleetOwnerName == paginationModel.FleetOwnerName);

                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result)
                {
                    var vehicleType = await ToListAsync<SiteVehicleType>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.TotalVehicleTypes = (uint) vehicleType.Count();//await CountAsync<SiteVehicleType>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.TotalBids = await CountAsync<Bid>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.TotalVehicles = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted);
                    x.JobsCompleted = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.Status == JobStatus.Completed);
                    x.JobsInQueue = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.Status == JobStatus.InQueue);
                    x.JobsInProgress = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.Status == JobStatus.InProcess);
                    x.JobsUnderReview = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted &&( y.Status == JobStatus.UnderReviewSupervisor || y.Status == JobStatus.UnderReviewApprovalManager));
                    
                    if (x.TotalVehicleTypes > 0)
                    {
                        x.AllowBiding = true;
                        foreach (var type in vehicleType)
                        {
                            var vehicleCount = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted && y.SiteVehicleTypeId == type.Id);
                            if (vehicleCount == 0)
                            {
                                x.AllowBiding = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        x.AllowBiding = false;
                    }

                }
               

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("OpenSites")]
        public async Task<IActionResult> GetOpenSites(SitePaginationModel paginationModel)
        {
            try
            {
                var installerId = await GetAssociatedId();
                var temp_result = await Entities.Bid.Where(x => x.InstallerId == installerId).Select(x => x.SiteId).ToListAsync();



                Expression<Func<Site, bool>> filter = x => !x.IsDeleted

                // Start Code
                && (paginationModel.SiteName == "" || x.Name.ToLower().Contains(paginationModel.SiteName.ToLower()))
                // End Code


                && x.SiteStatus == SiteStatus.OpenForBid
                && !temp_result.Contains(x.Id);
                var joinResult = Entities.Site.Where(filter).OrderByDescending(x => x.Id).Join(Entities.Project, s => s.ProjectId, p => p.Id
               , (a, b) => new SiteModel
               {
                   Id = a.Id,
                   Name = a.Name,
                   ProjectName = b.Name,
                   PONo = a.PONo,
                   SiteStatus = SiteStatus.OpenForBid
               }).Where(p => paginationModel.ProjectName == "" || p.ProjectName.ToLower().Contains(paginationModel.ProjectName.ToLower()));


                var count = await joinResult.CountAsync();
                var result = await joinResult.GetPageAsync(paginationModel);

                foreach (var x in result.ToList())
                {
                    x.TotalVehicles = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted);
                    SiteStatusLog temp = await LastOrDefaultAsync<SiteStatusLog>(y => y.SiteId == x.Id && (paginationModel.OpeningDate==null || y.CreatedDate.Date== paginationModel.OpeningDate.GetValueOrDefault().Date));
                    if (temp!=null)
                    {
                        x.OpenDate = temp?.CreatedDate;
                    }
                    else if (!(paginationModel.OpeningDate==null))
                    {
                        result.Remove(x);
                    }
                }
                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
        //Post pagination model to get open Sites For Bids
        //[HttpPost("OpenSites")]
        //public async Task<IActionResult> GetOpenSites(SitePaginationModel paginationModel)
        //{
        //    try
        //    {
        //        var installerId = await GetAssociatedId();
        //        //var temp_result = Entities.Bid.Where(x => x.InstallerId == installerId);
        //        var temp_result = await Entities.Bid.Where(x => x.InstallerId == installerId).Select(x => x.SiteId).ToListAsync();

        //        Expression<Func<Site, bool>> filter = x => !x.IsDeleted && x.SiteStatus == SiteStatus.OpenForBid
        //        && !temp_result.Contains(x.Id);
        //        var joinResult = Entities.Site.Where(filter).OrderByDescending(x => x.Id).Join(Entities.Project, s => s.ProjectId, p => p.Id
        //       , (a, b) => new SiteModel
        //       {
        //           Id = a.Id,
        //           Name = a.Name,
        //           ProjectName = b.Name,
        //           PONo = a.PONo,
        //           //b.FleetOwnerId,
        //           SiteStatus = SiteStatus.OpenForBid
        //       });
        //        var count = await joinResult.CountAsync();
        //        var result = await joinResult.GetPageAsync(paginationModel);

        //        foreach (var x in result)
        //        {
        //            x.TotalVehicles = await CountAsync<Job>(y => y.SiteId == x.Id && !y.IsDeleted);
        //            SiteStatusLog temp = await LastOrDefaultAsync<SiteStatusLog>(y => y.SiteId == x.Id);
        //            x.OpenDate = temp?.CreatedDate;
        //        }


        //        //var site = await ToListAsync<Site>(x => !x.IsDeleted && x.IsForBid == true);

        //        return Ok(result, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        return MethodFailure(ex);
        //    }
        //}

        //Site Bid List



        [HttpPost("SiteBidList")]
        public async Task<IActionResult> SiteBidList(SiteBidPaginationModel model)
        {
            try
            {
                Expression<Func<Bid, bool>> filter = x => !x.IsDeleted && x.SiteId == model.SiteId;

                var temp_result = Entities.Bid.Where(filter).Join(Entities.Installer, b => b.InstallerId, i => i.Id
                , (s, f) => new BidModel
                {
                    Id = s.Id,
                    SiteId = s.SiteId,
                    date = s.CreatedDate,
                    Status = s.Status,
                    Price = s.Price,
                    Comments = s.Comments,
                    InstallerName = f.FirstName + " " + f.LastName

                }).Where(z =>

                    (model.Installer == "" || z.InstallerName.ToLower().Contains(model.Installer.ToLower()))
                    && (model.BidPrice == 0 || z.Price == model.BidPrice)
                    && (model.BidDate == null || z.date.Date==model.BidDate.Value.Date)
                 );

                var count = await temp_result.CountAsync();
                var result = await temp_result.GetPageAsync(model);

                return Ok(result, count);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }

        }

        //Award Sites
        [HttpPost("AwardSite")]
        public async Task<IActionResult> AwardSite(BidModel model)
        {
            try {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        //Bid bidObj = Entities.Bid.Where(x => x.Id == bid.Id).SingleOrDefault();
                        var bid = await FirstOrDefaultAsync<Bid>(x => x.Id == model.Id);
                        if(bid is null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            bid.Status = BidStatus.Awarded;
                            await UpdateAsync(bid);
                            var otherBids = await ToListAsync<Bid>(x => x.SiteId == model.SiteId && x.Id != model.Id && x.Status == BidStatus.Pending);
                            otherBids.ForEach(async x => {
                                x.Status = BidStatus.Rejected;
                                await UpdateAsync(x);
                            });

                            var site = await FirstOrDefaultAsync<Site>(x => x.Id == model.SiteId);
                            if(site is null)
                            {
                                return NotFound();
                            }
                            else
                            {
                                site.SiteStatus = Models.SiteStatus.Awarded;
                                await UpdateAsync<Site>(site);

                                var log = new SiteStatusLog
                                {
                                    SiteId = site.Id,
                                    SiteStatus = site.SiteStatus,
                                    CreatedBy = UserClaims.UserId,
                                    CreatedDate = UserClaims.DateTime,
                                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                                };

                                await InsertAsync<SiteStatusLog>(log);

                            }
                            tran.Commit();
                            return Ok();
                        }
                        //bidObj.Status = Models.BidStatus.Awarded;
                        //await UpdateAsync<Bid>(bidObj);
                        //var bids = Entities.Bid.Where(x => x.SiteId == bid.SiteId && x.Status == Models.BidStatus.Pending);

                        //foreach (var x in bids)
                        //{
                        //    x.Status = Models.BidStatus.Rejected;

                        //    //await UpdateAsync<Bid>(x);
                        //    var res = Entities.Bid.Update(x);
                        //}
                        //return Ok();
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
        
        //Reject Site
        [HttpPost("RejectSite")] 
        public async Task<IActionResult> RejectSite(BidModel model)
        {
            try
            {
                var bid = await FirstOrDefaultAsync<Bid>(i=> i.Id == model.Id && i.SiteId == model.SiteId);
                if(bid is null)
                {
                    return NotFound();
                }
                else
                {
                    bid.Status = BidStatus.Rejected;
                    await UpdateAsync<Bid>(bid);

                    return Ok();
                }
            }
            catch (Exception ex)
            {

                return MethodFailure(ex);
            }
        }

        // POST <controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SiteModel site)
        {
            try
            {
                var vSite = new Site
                {
                    Id = site.Id,
                    FleetOwnerId = site.FleetOwnerId,
                    ProjectId = site.ProjectId,
                    Name = site.Name,
                    CSR = site.CSR,
                    SalesMan = site.SalesMan,
                    BidDueDate = site.BidDueDate,
                    SiteStartDate = site.SiteStartDate,
                    SiteEndDate = site.SiteEndDate,
                    MaximumBidAmount = site.MaximumBidAmount,
                    SalePrice = site.SalePrice,
                    ScopeOfWork = site.ScopeOfWork,
                    SpecialInstruction = site.SpecialInstruction,
                    Address = site.Address,
                    ZipCode = site.ZipCode,
                    CityId = site.CityId,
                    PONo = site.PONo,
                    EPMSJobNo = site.EPMSJobNo,
                    TakeDownStatus = site.TakeDownStatus,
                    SiteStatus = site.SiteStatus,
                    IndoorFacility = site.IndoorFacility,
                    BusinessHours = site.BusinessHours,
                    WeekendHours = site.WeekendHours,
                    IsSpottingTractor = site.IsSpottingTractor,
                    StoreEquipmentOnSite = site.StoreEquipmentOnSite,
                    SpotVehicle = site.SpotVehicle,
                    RemoveExistingGraphics = site.RemoveExistingGraphics,
                    YearsOnVehicle = site.YearsOnVehicle,
                    CleaningAndPrepVehicle = site.CleaningAndPrepVehicle,
                    PowerWash = site.PowerWash,
                    IsVinylGraphics = site.IsVinylGraphics,
                    SealOfDecals = site.SealOfDecals,
                    ArlonVinylFilmRemoval = site.ArlonVinylFilmRemoval,
                    ArlonReflectiveFilmApply = site.ArlonReflectiveFilmApply,
                    AveryVinylFilmRemoval = site.AveryVinylFilmRemoval,
                    AveryVinylFilmApply = site.AveryVinylFilmApply,
                    M3VinylFilmRemoval = site.M3VinylFilmRemoval,
                    M3VinylFilmApply = site.M3VinylFilmApply,
                    ArlonReflectiveFilmRemoval = site.ArlonReflectiveFilmRemoval,
                    ArlonVinylFilmApply = site.ArlonVinylFilmApply,
                    AveryReflectiveFilmRemoval = site.AveryReflectiveFilmRemoval,
                    AveryReflectiveFilmApply = site.AveryReflectiveFilmApply,
                    M3ReflectiveFilmRemoval = site.M3ReflectiveFilmRemoval,
                    M3ReflectiveFilmApply = site.M3ReflectiveFilmApply,
                    TotalSqFtRemoval = site.TotalSqFtRemoval,
                    TotalSqFtApply = site.TotalSqFtApply,
                    OtherQuestionsComments = site.OtherQuestionsComments,
                    CustomerAvailabilityOnInstallationDay = site.CustomerAvailabilityOnInstallationDay,
                    InsideFacilities = site.InsideFacilities,
                    IsDecalsReceived = site.IsDecalsReceived,
                    VehicleCleanessBeforeArrival = site.VehicleCleanessBeforeArrival,
                    IsInformed6FeetArea = site.IsInformed6FeetArea,
                    VehicleAvailabilityDate = site.VehicleAvailabilityDate,
                    WorkHoursAtFacility = site.WorkHoursAtFacility,
                    AdditionalContacts = site.AdditionalContacts,
                    IsDeleted = false,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset,
                };
                await InsertAsync(vSite);
                SiteStatusLog log = new SiteStatusLog
                {
                    Id = 0,
                    SiteId = vSite.Id,
                    SiteStatus = SiteStatus.Pendning,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };
                await InsertAsync(log);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        // PUT <controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]SiteModel site)
        {
            try
            {
                if (!await AnyAsync<Site>(model => model.Id == id))
                {
                    return NotFound();
                }

                var vSite = await FirstOrDefaultAsync<Site>(model => model.Id == id);
                vSite.FleetOwnerId = site.FleetOwnerId;
                vSite.ProjectId = site.ProjectId;
                vSite.Name = site.Name;
                vSite.CSR = site.CSR;
                vSite.SalesMan = site.SalesMan;
                vSite.BidDueDate = site.BidDueDate;
                vSite.SiteStartDate = site.SiteStartDate;
                vSite.SiteEndDate = site.SiteEndDate;
                vSite.MaximumBidAmount = site.MaximumBidAmount;
                vSite.SalePrice = site.SalePrice;
                vSite.ScopeOfWork = site.ScopeOfWork;
                vSite.SpecialInstruction = site.SpecialInstruction;
                vSite.Address = site.Address;
                vSite.ZipCode = site.ZipCode;
                vSite.CityId = site.CityId;
                vSite.PONo = site.PONo;
                vSite.EPMSJobNo = site.EPMSJobNo;
                vSite.TakeDownStatus = site.TakeDownStatus;
                vSite.SiteStatus = site.SiteStatus;
                vSite.IndoorFacility = site.IndoorFacility;
                vSite.BusinessHours = site.BusinessHours;
                vSite.WeekendHours = site.WeekendHours;
                vSite.IsSpottingTractor = site.IsSpottingTractor;
                vSite.StoreEquipmentOnSite = site.StoreEquipmentOnSite;
                vSite.SpotVehicle = site.SpotVehicle;
                vSite.RemoveExistingGraphics = site.RemoveExistingGraphics;
                vSite.YearsOnVehicle = site.YearsOnVehicle;
                vSite.CleaningAndPrepVehicle = site.CleaningAndPrepVehicle;
                vSite.PowerWash = site.PowerWash;
                vSite.IsVinylGraphics = site.IsVinylGraphics;
                vSite.SealOfDecals = site.SealOfDecals;
                vSite.ArlonVinylFilmRemoval = site.ArlonVinylFilmRemoval;
                vSite.ArlonReflectiveFilmApply = site.ArlonReflectiveFilmApply;
                vSite.AveryVinylFilmRemoval = site.AveryVinylFilmRemoval;
                vSite.AveryVinylFilmApply = site.AveryVinylFilmApply;
                vSite.M3VinylFilmRemoval = site.M3VinylFilmRemoval;
                vSite.M3VinylFilmApply = site.M3VinylFilmApply;
                vSite.ArlonReflectiveFilmRemoval = site.ArlonReflectiveFilmRemoval;
                vSite.ArlonVinylFilmApply = site.ArlonVinylFilmApply;
                vSite.AveryReflectiveFilmRemoval = site.AveryReflectiveFilmRemoval;
                vSite.AveryReflectiveFilmApply = site.AveryReflectiveFilmApply;
                vSite.M3ReflectiveFilmRemoval = site.M3ReflectiveFilmRemoval;
                vSite.M3ReflectiveFilmApply = site.M3ReflectiveFilmApply;
                vSite.TotalSqFtRemoval = site.TotalSqFtRemoval;
                vSite.TotalSqFtApply = site.TotalSqFtApply;
                vSite.OtherQuestionsComments = site.OtherQuestionsComments;
                vSite.CustomerAvailabilityOnInstallationDay = site.CustomerAvailabilityOnInstallationDay;
                vSite.InsideFacilities = site.InsideFacilities;
                vSite.IsDecalsReceived = site.IsDecalsReceived;
                vSite.VehicleCleanessBeforeArrival = site.VehicleCleanessBeforeArrival;
                vSite.IsInformed6FeetArea = site.IsInformed6FeetArea;
                vSite.VehicleAvailabilityDate = site.VehicleAvailabilityDate;
                vSite.WorkHoursAtFacility = site.WorkHoursAtFacility;
                vSite.AdditionalContacts = site.AdditionalContacts;
                vSite.IsDeleted = false;
                vSite.ModifiedBy = UserClaims.UserId;
                vSite.ModifiedDate = UserClaims.DateTime;
                vSite.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                await UpdateAsync(vSite);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        // DELETE <controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var site = await FirstOrDefaultAsync<Site>(x => x.Id == id);
                if (site is null) return NotFound();
                site.IsDeleted = true;
                site.ModifiedBy = UserClaims.UserId;
                site.ModifiedDate = UserClaims.DateTime;
                site.TZOSModifiedBy = UserClaims.TimeZoneOffset;
                await DeleteAsync(site);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("IsUniqueName/{projectId}/{siteId}/{siteName}")]
        public async Task<IActionResult> IsRecordExists(uint projectId, uint siteId, string siteName)
        {
            return Ok(!await AnyAsync<Site>(x => x.ProjectId == projectId && x.Id != siteId && x.Name.ToLower().Trim() == siteName.ToLower().Trim()));
        }

        [HttpGet("OpenForBid/{id}")]
        public async Task<IActionResult> OpenForBid(uint id)
        {
            try
            {
                var site = await FirstOrDefaultAsync<Site>(x => x.Id == id);
                if(site is null)
                {
                    return NotFound();
                }
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        site.SiteStatus = SiteStatus.OpenForBid;
                        await UpdateAsync(site);
                        SiteStatusLog log = new SiteStatusLog
                        {
                            Id = 0,
                            SiteId = site.Id,
                            SiteStatus = SiteStatus.OpenForBid,
                            CreatedBy = UserClaims.UserId,
                            CreatedDate = UserClaims.DateTime,
                            TZOSCreatedBy = UserClaims.TimeZoneOffset
                        };
                        await InsertAsync(log);
                        tran.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }


        [HttpGet("UpdateStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeSiteStatus(uint id, SiteStatus status)
        {
            try
            {
                var site = await FirstOrDefaultAsync<Site>(x => x.Id == id);
                if (site is null)
                {
                    return NotFound();
                }
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        site.SiteStatus = status;
                        await UpdateAsync(site);
                        SiteStatusLog log = new SiteStatusLog
                        {
                            Id = 0,
                            SiteId = site.Id,
                            SiteStatus = status,
                            CreatedBy = UserClaims.UserId,
                            CreatedDate = UserClaims.DateTime,
                            TZOSCreatedBy = UserClaims.TimeZoneOffset
                        };
                        await InsertAsync(log);
                        tran.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return MethodFailure(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("ProjectAgainstSite/{SiteId}")]
        public async Task<IActionResult> ProjectAgainstSite(uint SiteId)
        {
            try
            {
                Expression<Func<Site, bool>> filter = x => !x.IsDeleted && x.Id == SiteId;

                var JoinResult = Entities.Site.Where(filter).Join(Entities.Project, s => s.ProjectId, p => p.Id
                , (a, b) => new
                {
                    b.Id,
                    b.FleetOwnerId,
                    b.Name,
                    b.IsPoolVehicle,
                    b.Status,
                    b.StatusByFleetOwner,
                    b.ContactName,
                    b.ContactPhone,
                    b.Address
                }).Join(Entities.FleetOwner, x => x.FleetOwnerId, y => y.Id, (c, d) => new ProjectModel
                {
                    Name=c.Name,
                    FleetOwnerName=d.FirstName+" "+d.LastName,
                    IsPoolVehicle=c.IsPoolVehicle,
                    Status=c.Status,
                    StatusByFleetOwner=c.StatusByFleetOwner,
                    ContactName=c.ContactName,
                    ContactPhone=c.ContactPhone,
                    Address=c.Address
                });
                var result =await JoinResult.ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }


        }

        // For Site List //

        [HttpGet("JobInQueueDetail/{SiteId}")]
        public async Task<IActionResult> JobInQueueDetail(uint SiteId)
        {
            try
            {
                var result =await Entities.Job.Where(x=>x.SiteId == SiteId && x.IsDeleted == false && x.Status == JobStatus.InQueue).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }


        }

        [HttpGet("JobInProgressDetail/{SiteId}")]
        public async Task<IActionResult> JobInProgressDetail(uint SiteId)
        {
            try
            {
                var result = await Entities.Job.Where(x => x.SiteId == SiteId && x.IsDeleted == false && x.Status == JobStatus.InProcess).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }


        }

        [HttpGet("CompletedJobDetail/{SiteId}")]
        public async Task<IActionResult> CompletedJobDetail(uint SiteId)
        {
            try
            {
                var result = await Entities.Job.Where(x => x.SiteId == SiteId && x.IsDeleted == false && x.Status == JobStatus.Completed).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }


        }

        [HttpGet("JobUnderReviewDetail/{SiteId}")]
        public async Task<IActionResult> JobUnderReviewDetail(uint SiteId)
        {
            try
            {
                var result = await Entities.Job.Where(x => x.SiteId == SiteId 
                                                     && x.IsDeleted == false 
                                                     && (x.Status == JobStatus.UnderReviewSupervisor || x.Status == JobStatus.UnderReviewApprovalManager)).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }


        }

        //Types
        [HttpGet("JobInQueueDetailByJobId/{JobId}")]
        public async Task<IActionResult> JobInQueueDetailByJobId(uint JobId)
        {
            try
            {
                var result = await Entities.Job.Where(x => x.SiteVehicleTypeId == JobId && x.IsDeleted == false && x.Status == JobStatus.InQueue).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("JobInProgressDetailByJobId/{JobId}")]
        public async Task<IActionResult> JobInProgressDetailByJobId(uint JobId)
        {
            try
            {
                var result = await Entities.Job.Where(x => x.SiteVehicleTypeId == JobId && x.IsDeleted == false && x.Status == JobStatus.InProcess).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("CompletedDetailByJobId/{JobId}")]
        public async Task<IActionResult> CompletedDetailByJobId(uint JobId)
        {
            try
            {
                var result = await Entities.Job.Where(x => x.SiteVehicleTypeId == JobId && x.IsDeleted == false && x.Status == JobStatus.Completed).ToListAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("SiteAgainstProject/{ID}")]
        public async Task<IActionResult> SiteAgainstProject(int ID)
        {
            try
            {
                Expression<Func<Site, bool>> filter = x => !x.IsDeleted && x.Id == ID;

                var joinResult = Entities.Site.Where(filter).Join(Entities.Project, s => s.ProjectId, p => p.Id
                , (a, b) => new
                {
                    a.Id,
                    a.Name,
                    ProjectName = b.Name,
                    b.FleetOwnerId,
                    a.CSR,
                    a.SalesMan,
                    a.BidDueDate,
                    a.SiteStartDate,
                    a.SiteEndDate,
                    a.MaximumBidAmount,
                    a.SalePrice,
                    a.ScopeOfWork,
                    a.SpecialInstruction,
                    a.Address,
                    a.CityId,
                    a.ZipCode,
                    a.PONo,
                    a.EPMSJobNo,
                    a.TakeDownStatus,
                    a.IndoorFacility,
                    a.BusinessHours,
                    a.WeekendHours,
                    a.IsSpottingTractor,
                    a.StoreEquipmentOnSite,
                    a.SpotVehicle,
                    a.RemoveExistingGraphics,
                    a.YearsOnVehicle,
                    a.CleaningAndPrepVehicle,
                    a.PowerWash,
                    a.IsVinylGraphics,
                    a.SealOfDecals,
                    a.OtherQuestionsComments,
                    a.CustomerAvailabilityOnInstallationDay,
                    a.InsideFacilities,
                    a.IsDecalsReceived,
                    a.VehicleCleanessBeforeArrival,
                    a.IsInformed6FeetArea,
                    a.VehicleAvailabilityDate,
                    a.AdditionalContacts,
                    a.WorkHoursAtFacility,
                    a.ArlonVinylFilmRemoval,
                    a.ArlonVinylFilmApply,
                    a.AveryVinylFilmRemoval,
                    a.AveryVinylFilmApply,
                    a.M3VinylFilmRemoval,
                    a.M3VinylFilmApply,
                    a.ArlonReflectiveFilmRemoval,
                    a.ArlonReflectiveFilmApply,
                    a.AveryReflectiveFilmRemoval,
                    a.AveryReflectiveFilmApply,
                    a.M3ReflectiveFilmRemoval,
                    a.M3ReflectiveFilmApply,
                    a.TotalSqFtRemoval,
                    a.TotalSqFtApply,
                    a.SiteStatus
                }).Join(Entities.FleetOwner, s => s.FleetOwnerId, f => f.Id
                , (s, f) => new SiteModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ProjectName = s.ProjectName,
                    FleetOwnerId = f.Id,
                    FleetOwnerName = f.FirstName + " " + f.LastName,
                    CSR = s.CSR,
                    SalesMan = s.SalesMan,
                    BidDueDate = s.BidDueDate,
                    SiteStartDate = s.SiteStartDate,
                    SiteEndDate = s.SiteEndDate,
                    MaximumBidAmount = s.MaximumBidAmount,
                    SalePrice = s.SalePrice,
                    ScopeOfWork = s.ScopeOfWork,
                    SpecialInstruction = s.SpecialInstruction,
                    Address = s.Address,
                    CityId = s.CityId,
                    ZipCode = s.ZipCode,
                    PONo = s.PONo,
                    EPMSJobNo = s.EPMSJobNo,
                    TakeDownStatus =s.TakeDownStatus,
                    IndoorFacility = s.IndoorFacility,
                    BusinessHours = s.BusinessHours,
                    WeekendHours = s.WeekendHours,
                    IsSpottingTractor = s.IsSpottingTractor,
                    StoreEquipmentOnSite = s.StoreEquipmentOnSite,
                    SpotVehicle = s.SpotVehicle,
                    RemoveExistingGraphics = s.RemoveExistingGraphics,
                    YearsOnVehicle = s.YearsOnVehicle,
                    CleaningAndPrepVehicle = s.CleaningAndPrepVehicle,
                    PowerWash = s.PowerWash,
                    IsVinylGraphics =s.IsVinylGraphics,
                    SealOfDecals = s.SealOfDecals,
                    OtherQuestionsComments = s.OtherQuestionsComments,
                    CustomerAvailabilityOnInstallationDay = s.CustomerAvailabilityOnInstallationDay,
                    InsideFacilities = s.InsideFacilities,
                    IsDecalsReceived = s.IsDecalsReceived,
                    VehicleCleanessBeforeArrival = s.VehicleCleanessBeforeArrival,
                    IsInformed6FeetArea = s.IsInformed6FeetArea,
                    VehicleAvailabilityDate = s.VehicleAvailabilityDate,
                    AdditionalContacts = s.AdditionalContacts,
                    WorkHoursAtFacility = s.WorkHoursAtFacility,
                    ArlonVinylFilmRemoval = s.ArlonVinylFilmRemoval,
                    ArlonVinylFilmApply = s.ArlonVinylFilmApply,
                    AveryVinylFilmRemoval = s.AveryVinylFilmRemoval,
                    AveryVinylFilmApply = s.AveryVinylFilmApply,
                    M3VinylFilmRemoval = s.M3VinylFilmRemoval,
                    M3VinylFilmApply = s.M3VinylFilmApply,
                    ArlonReflectiveFilmRemoval = s.ArlonReflectiveFilmRemoval,
                    ArlonReflectiveFilmApply = s.ArlonReflectiveFilmApply,
                    AveryReflectiveFilmRemoval = s.AveryReflectiveFilmRemoval,
                    AveryReflectiveFilmApply = s.AveryReflectiveFilmApply,
                    M3ReflectiveFilmRemoval = s.M3ReflectiveFilmRemoval,
                    M3ReflectiveFilmApply = s.M3ReflectiveFilmApply,
                    TotalSqFtRemoval = s.TotalSqFtRemoval,
                    TotalSqFtApply = s.TotalSqFtApply,
                    SiteStatus = s.SiteStatus
                    //TotalVehicleTypes = 5,
                    //TotalVehicles = 30,
                    //TotalBids = 10
                });

                var result = await joinResult.FirstOrDefaultAsync();
                if (result != null)
                {

                    result.VehicleAvailabileDate = result.VehicleAvailabilityDate.ToShortDateString();
                    if (result.VehicleAvailabileDate.Contains("0001"))
                    {
                        result.VehicleAvailabileDate = "";
                    }
                    var city = await FirstOrDefaultAsync<City>(x => x.Id == result.CityId);
                    result.CityName = city.Name;
                    var state = await FirstOrDefaultAsync<State>(x => x.Id == city.StateId);
                    result.StateName = state.Name;
                    var country = await FirstOrDefaultAsync<Country>(x => x.Id == state.CountryId);
                    result.CountryName = country.Name;
                }
               
                return Ok(result);
                
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("SiteVehicleTypeDetail/{ID}")]
        public async Task<IActionResult> SiteVehicleTypeDetail(int ID)
        {
            try
            {
                //Expression<Func<VehicleTypeSpecifications, bool>> filter = x => !x.IsDeleted && x.Id == ID;

                var joinResult = Entities.VehicleTypeSiteAssociation.Where(x => !x.IsDeleted && x.Id == ID).Join(Entities.VehicleTypes, s => s.VehicleTypeId, p => p.Id
                , (a, b) => new
                {
                    a.Id,
                    VehicleTypeName = b.Name,
                    a.VehiclePrice,
                    a.VehicleSpecificationId,
                    a.SideHeight,
                    a.SideWidth,
                    a.FrontHeight,
                    a.FrontWidth,
                    a.RearHeight,
                    a.RearWidth
                }).Select(x => new SiteVehicleTypeModel {
                    Id = x.Id,
                    VehicleTypeName = x.VehicleTypeName,
                    VehiclePrice = x.VehiclePrice,
                    VehicleSpecificationId = x.VehicleSpecificationId,
                    VehicleSpecificationName = Entities.VehicleTypeSpecifications.Where(s=>s.Id == x.VehicleSpecificationId).Select(s=> s.Name).FirstOrDefault(),
                    SideHeight = x.SideHeight,
                    SideWidth = x.SideWidth,
                    FrontHeight = x.FrontHeight,
                    FrontWidth = x.FrontWidth,
                    RearHeight = x.RearHeight,
                    RearWidth = x.RearWidth
                });
                //Join(Entities.VehicleTypeSpecifications, s => s.VehicleSpecificationId, f => f.Id
                //, (s, f) => new SiteVehicleTypeModel
                //{
                //    Id = s.Id,
                //    VehicleTypeName = s.VehicleTypeName,
                //    VehiclePrice = s.VehiclePrice,
                //    VehicleSpecificationId = f.Id,
                //    VehicleSpecificationName = f.Name,
                //    SideHeight = s.SideHeight,
                //    SideWidth = s.SideWidth,
                //    FrontHeight = s.FrontHeight,
                //    FrontWidth = s.FrontWidth,
                //    RearHeight = s.RearHeight,
                //    RearWidth = s.RearWidth
                //});

                var result = await joinResult.FirstOrDefaultAsync();
               
                return Ok(result);

            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

    }
}
