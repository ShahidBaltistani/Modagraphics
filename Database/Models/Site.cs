using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("site")]
    public class Site : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint FleetOwnerId { get; set; }

        [Required]
        public uint ProjectId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CSR { get; set; }

        [Required]
        public string SalesMan { get; set; }

        public DateTime? BidDueDate { get; set; }

        [Required]
        public DateTime SiteStartDate { get; set; }

        [Required]
        public DateTime SiteEndDate { get; set; }

        [Required]
        public float MaximumBidAmount { get; set; }

        [Required]
        public float SalePrice { get; set; }

        public string ScopeOfWork { get; set; }

        public string SpecialInstruction { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public uint ZipCode { get; set; }

        [Required]
        public uint CityId { get; set; }

        public string PONo { get; set; }

        public string EPMSJobNo { get; set; }

        [Required]
        public TakedownStatus TakeDownStatus { get; set; }

        [Required]
        public SiteStatus SiteStatus { get; set; }

        [Required]
        public bool IndoorFacility { get; set; }

        [Required]
        public bool BusinessHours { get; set; }

        [Required]
        public bool WeekendHours { get; set; }

        [Required]
        public bool IsSpottingTractor { get; set; }

        [Required]
        public bool StoreEquipmentOnSite { get; set; }

        [Required]
        public bool SpotVehicle { get; set; }

        [Required]
        public bool RemoveExistingGraphics { get; set; }

        public int? YearsOnVehicle { get; set; }

        [Required]
        public bool CleaningAndPrepVehicle { get; set; }

        [Required]
        public bool PowerWash { get; set; }

        [Required]
        public bool IsVinylGraphics { get; set; }

        [Required]
        public bool SealOfDecals { get; set; }

        [Required]
        public bool ArlonVinylFilmRemoval { get; set; }

        [Required]
        public bool ArlonVinylFilmApply { get; set; }

        [Required]
        public bool AveryVinylFilmRemoval { get; set; }

        [Required]
        public bool AveryVinylFilmApply { get; set; }

        [Required]
        public bool M3VinylFilmRemoval { get; set; }

        [Required]
        public bool M3VinylFilmApply { get; set; }

        [Required]
        public bool ArlonReflectiveFilmRemoval { get; set; }

        [Required]
        public bool ArlonReflectiveFilmApply { get; set; }

        [Required]
        public bool AveryReflectiveFilmRemoval { get; set; }

        [Required]
        public bool AveryReflectiveFilmApply { get; set; }

        [Required]
        public bool M3ReflectiveFilmRemoval { get; set; }

        [Required]
        public bool M3ReflectiveFilmApply { get; set; }

        public float? TotalSqFtRemoval { get; set; }

        public float? TotalSqFtApply { get; set; }

        public string OtherQuestionsComments { get; set; }

        [Required]
        public bool CustomerAvailabilityOnInstallationDay { get; set; }

        [Required]
        public bool InsideFacilities { get; set; }

        [Required]
        public bool IsDecalsReceived { get; set; }

        [Required]
        public bool VehicleCleanessBeforeArrival { get; set; }

        [Required]
        public bool IsInformed6FeetArea { get; set; }

        [Required]
        public DateTime VehicleAvailabilityDate { get; set; }

        [Required]
        public string WorkHoursAtFacility { get; set; }

        public string AdditionalContacts { get; set; }


        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public uint CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public uint? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Required]
        public int TZOSCreatedBy { get; set; }
        public int? TZOSModifiedBy { get; set; }

    }

    [Table("site_status_log")]
    public class SiteStatusLog : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint SiteId { get; set; }

        [Required]
        public SiteStatus SiteStatus { get; set; }

        [Required]
        public uint CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
      
        [Required]
        public int TZOSCreatedBy { get; set; }

    }

}
