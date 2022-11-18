using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("installer")]
    public class Installer : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }

        public uint? PZipCode { get; set; }

        public uint PCityId { get; set; }

        public string Picture { get; set; }

        public float TravelInMiles { get; set; }

        [Required]
        public bool IsPreferred { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string BillToAddress { get; set; }

        public uint? CZipCode { get; set; }

        public uint CCityId { get; set; }

        public string FaxNo { get; set; }

        public string CContactNo { get; set; }

        public string CompanyEmail { get; set; }

        public string FEINumber { get; set; }

        public uint NoOfEmployees { get; set; }

        public string CertificationTraining { get; set; }

        public bool IsInstallersEmployee { get; set; }

        public bool IsEmployeesBackgroundCheck { get; set; }

        public bool IsDrugTest { get; set; }

        public string EquipmentOwned { get; set; }

        public string InstallLocations { get; set; }

        public string InstallProjectCompleted { get; set; }

        public uint YearInBusiness { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; }

        [Required]
        public InstallerStatus Status { get; set; }


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
}
