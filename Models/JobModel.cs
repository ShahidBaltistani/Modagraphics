using System;
namespace Models
{
    public class JobModel
    {
        public uint Id { get; set; }

        public uint SiteId { get; set; }

        public string SiteName { get; set; }

        public string ProjectName { get; set; }

        public string InstallerName { get; set; }

        public string VIN { get; set; }

        public string LPN { get; set; }

        public uint UnitNo { get; set; }

        public JobStatus Status { get; set; }

        public uint SiteVehicleTypeId { get; set; }
        public string VehicleName { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime CompletedDate { get; set; }
    }
}
