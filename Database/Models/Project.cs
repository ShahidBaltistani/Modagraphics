using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("project")]
    public class Project : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint FleetOwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsPoolVehicle { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        [Required]
        public StatusByFleetOwner StatusByFleetOwner { get; set; }

        
        public string Address { get; set; }

        public string ContactPhone { get; set; }

        public string ContactName { get; set; }


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
