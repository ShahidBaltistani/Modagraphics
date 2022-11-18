using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Database.Models
{
    [Table("site_vehicle_type")]
    public class SiteVehicleType : IIdentity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public uint VehicleTypeId { get; set; }

        [Required]
        public uint SiteId { get; set; }

        [Required]
        public uint VehicleSpecificationId { get; set; }

        [Required]
        public float VehiclePrice { get; set; }

        [Required]
        public float SideHeight { get; set; }

        [Required]
        public float SideWidth { get; set; }

        [Required]
        public float FrontHeight { get; set; }

        [Required]
        public float FrontWidth { get; set; }

        [Required]
        public float RearHeight { get; set; }

        [Required]
        public float RearWidth { get; set; }


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
