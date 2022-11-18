using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("job")]
    public class Job : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint SiteId { get; set; }

        [Required]
        public string VIN { get; set; }

        [Required]
        public string LPN { get; set; }

        [Required]
        public uint UnitNo { get; set; }

        [Required]
        public JobStatus Status { get; set; }

        [Required]
        public uint SiteVehicleTypeId { get; set; }


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

    [Table("jobStatusLogs")]
    public class JobStatusLogs : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint JobId { get; set; }

        [Required]
        public JobStatus Status { get; set; }

        [Required]
        public uint CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int TZOSCreatedBy { get; set; }
    }
}
