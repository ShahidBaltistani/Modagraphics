using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    [Table("rejectedJob")]
    public class RejectedJob : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint JobId { get; set; }

        [Required]
        public string Comments { get; set; }

        [Required]
        public JobStatus Status { get; set; }

        [Required]
        public bool isActive { get; set; }

        [Required]
        public uint CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int TZOSCreatedBy { get; set; }
    }
}
