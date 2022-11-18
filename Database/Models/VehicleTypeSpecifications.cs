using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("vehicle_type_specs")]
    public class VehicleTypeSpecifications : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Value { get; set; }

        [Required]
        public uint VehicleTypeId { get; set; }
    }
}
