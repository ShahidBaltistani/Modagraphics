using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("job_images")]
    public class JobImages : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint JobId { get; set; }

        [Required]
        public uint CrewId { get; set; }

        [Required]
        public string Picture { get; set; }

        public string Remarks { get; set; }

        public JobImagesStatus Status { get; set; }

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
