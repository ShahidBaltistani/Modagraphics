using System;

namespace Models
{
    public class IncidentReportModel
    {
        public uint JobId { get; set; }

        public uint SupervisorId { get; set; }

        public uint SiteId { get; set; }

        public uint ProjectId { get; set; }

        public uint SiteVehicleTypeId { get; set; }

        public uint JobImageID { get; set; }

        public uint vechicleCount { get; set; }

        public string SiteName { get; set; }

        public string ProjectName { get; set; }

        public string Picture { get; set; }

        public string LPN { get; set; }

        public string VIN { get; set; }

        public uint UnitNo { get; set; }
        
        public JobStatus JobStatus { get; set; }

        public JobImagesStatus JobImageStatus { get; set; }

        public DateTime AssignedDate { get; set; }
    } 
}
