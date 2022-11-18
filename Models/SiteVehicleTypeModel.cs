
namespace Models
{
    public class SiteVehicleTypeModel
    {
        public uint Id { get; set; }

        public uint VehicleTypeId { get; set; }

        public string VehicleTypeName { get; set; }

        public uint SiteId { get; set; }

        public string SiteName { get; set; }

        public uint VehicleSpecificationId { get; set; }

        //public List int[]  VehicleSpecificationId { get; set; }

        public string VehicleSpecificationName { get; set; }

        public float VehiclePrice { get; set; }

        public float SideHeight { get; set; }

        public float SideWidth { get; set; }

        public float FrontHeight { get; set; }

        public float FrontWidth { get; set; }

        public float RearHeight { get; set; }

        public float RearWidth { get; set; }

        public uint TotalVehicles { get; set; }

        public uint JobsInQueue { get; set; }

        public uint JobsInProgress { get; set; }

        public uint JobsCompleted { get; set; }

        public uint Vehicles { get; set; }
    }
}
