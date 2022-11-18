using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("corporate_user")]
    public class CorporateUser : IIdentity
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

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string Address { get; set; }

        public uint? ZipCode { get; set; }

        [Required]
        public uint CityId { get; set; }

        public string Picture { get; set; }

        [Required]
        public CorporateUserStatus Status { get; set; }


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
