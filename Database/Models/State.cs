using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("state")]
    public class State : IIdentity
    {
        [Key]
        [Required]
        public uint Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public uint CountryId { get; set; }
    }
}
