using System;

namespace Models
{
    public class ProjectModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public uint FleetOwnerId { get; set; }

        public string FleetOwnerName { get; set; }

        public uint TotalSites { get; set; }

        public bool IsPoolVehicle { get; set; }

        public ProjectStatus Status { get; set; }

        public StatusByFleetOwner StatusByFleetOwner { get; set; }
        public string Address { get; set; }

        public string ContactPhone { get; set; }

        public string ContactName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
