using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("log")]
    public class Log : IIdentity
    {
        [Required]
        [Key]
        public uint Id { get; set; }

        [Required]
        public string IP { get; set; }

        [Required]
        public DateTime EnteredOn { get; set; }

        [Required]
        public string UserAgent { get; set; }
    }
}
