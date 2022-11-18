using System;
using System.Collections.Generic;

namespace Models
{
    public class BidModel
    {
        public uint Id { get; set; }

        public uint InstallerId { get; set; }

        public uint SiteId { get; set; }

        public uint SiteVehicals { get; set; } 

        public uint CrewCount { get; set; }

        public string SiteName { get; set; }

        public string ProjectName { get; set; }

        public string InstallerName { get; set; } 

        public float Price { get; set; }

        public float Progress { get; set; }

        public string Comments { get; set; }

        public BidStatus Status { get; set; }

        public SiteStatus SiteStatus { get; set; }

        public DateTime date { get; set; }

        public DateTime AwardedDate { get; set; }

        public DateTime AssignDate { get; set; }

        public uint SiteVehicalsInQue { get; set; }

        public IEnumerable<uint> SiteSupervisors { get; set; }
    }
}
