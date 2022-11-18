using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("city")]
    public class City : IIdentity
    {
        [Required]
        [Key]
        public uint Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public uint StateId { get; set; }
    }
}
