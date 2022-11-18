using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("fleet_owner")]
    public class FleetOwner : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint UserCorporateId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string Address { get; set; }

        public uint? PZipCode { get; set; }

        [Required]
        public uint PCityId { get; set; }

        public string Picture { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string BillToAddress { get; set; }

        public uint? CZipCode { get; set; }

        [Required]
        public uint CCityId { get; set; }

        public string FaxNo { get; set; }

        [Required]
        public string CContactNo { get; set; }

        public string BIEmbededCode { get; set; }

        [Required]
        public FleetOwnerStatus Status { get; set; }


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
