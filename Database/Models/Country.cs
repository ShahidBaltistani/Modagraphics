using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("country")]
    public class Country : IIdentity
    {
        [Required]
        [Key]
        public uint Id { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public uint PhoneCode { get; set; }

    }
}
