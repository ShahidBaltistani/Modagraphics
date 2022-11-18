using System;

namespace Models
{
    public class SiteModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public uint FleetOwnerId { get; set; }
        public string FleetOwnerName { get; set; }

        public uint ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string CSR { get; set; }

        public string SalesMan { get; set; }

        public DateTime? BidDueDate { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime SiteStartDate { get; set; }

        public DateTime SiteEndDate { get; set; }

        public float MaximumBidAmount { get; set; }

        public float SalePrice { get; set; }

        public string ScopeOfWork { get; set; }

        public string SpecialInstruction { get; set; }

        public string Address { get; set; }

        public uint ZipCode { get; set; }

        public uint CityId { get; set; }

        public string PONo { get; set; }

        public string EPMSJobNo { get; set; }

        public TakedownStatus TakeDownStatus { get; set; }

        public SiteStatus SiteStatus { get; set; }

        public bool IndoorFacility { get; set; }

        public bool BusinessHours { get; set; }

        public bool WeekendHours { get; set; }

        public bool IsSpottingTractor { get; set; }

        public bool StoreEquipmentOnSite { get; set; }

        public bool SpotVehicle { get; set; }

        public bool RemoveExistingGraphics { get; set; }

        public int? YearsOnVehicle { get; set; }

        public bool CleaningAndPrepVehicle { get; set; }

        public bool PowerWash { get; set; }

        public bool IsVinylGraphics { get; set; }

        public bool SealOfDecals { get; set; }

        public bool ArlonVinylFilmRemoval { get; set; }

        public bool ArlonVinylFilmApply { get; set; }

        public bool AveryVinylFilmRemoval { get; set; }

        public bool AveryVinylFilmApply { get; set; }

        public bool M3VinylFilmRemoval { get; set; }

        public bool M3VinylFilmApply { get; set; }

        public bool ArlonReflectiveFilmRemoval { get; set; }

        public bool ArlonReflectiveFilmApply { get; set; }

        public bool AveryReflectiveFilmRemoval { get; set; }

        public bool AveryReflectiveFilmApply { get; set; }

        public bool M3ReflectiveFilmRemoval { get; set; }

        public bool M3ReflectiveFilmApply { get; set; }

        public float? TotalSqFtRemoval { get; set; }

        public float? TotalSqFtApply { get; set; }

        public string OtherQuestionsComments { get; set; }

        public bool CustomerAvailabilityOnInstallationDay { get; set; }

        public bool InsideFacilities { get; set; }

        public bool IsDecalsReceived { get; set; }

        public bool VehicleCleanessBeforeArrival { get; set; }

        public bool IsInformed6FeetArea { get; set; }

        public DateTime VehicleAvailabilityDate { get; set; }

        public string WorkHoursAtFacility { get; set; }

        public string AdditionalContacts { get; set; }

        public uint TotalVehicleTypes { get; set; }

        public uint TotalVehicles { get; set; }

        public uint TotalBids { get; set; }

        public uint JobsInQueue { get; set; }

        public uint JobsInProgress { get; set; }

        public uint JobsUnderReview { get; set; }

        public uint JobsCompleted { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public bool AllowBiding { get; set; }

        //
        public string VehicleAvailabileDate { get; set; }


    }
}
