using System;

namespace Models

{
    public class CrewSiteModel
    {
        public uint Id { get; set; }

        public uint CrewId { get; set; }
        public uint FleetOwnerId { get; set; }

        public uint SiteId { get; set; }

        public uint JobId { get; set; }

        public string VIN { get; set; }

        public string LPN { get; set; }

        public uint UnitNo { get; set; }

        public JobStatus Status { get; set; }
        public uint StartImgCount { get; set; }
        public uint EndImgCount { get; set; }
        public string VehicleTypeName { get; set; }
        public string ProjectName { get; set; }
        public string SiteName { get; set; }

        public DateTime? StartDate { get; set; }
        
    }
}
