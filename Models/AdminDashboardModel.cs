using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DashboardModel
    {
        public uint ActiveProjects { get; set; }
        public uint ActiveSites { get; set; }
        public uint Bids { get; set; }
        public uint TotalProjects { get; set; }
        public uint TotalSites { get; set; }

        public uint TotalCrews { get; set; }
        public uint TotalBids { get; set; }
        public uint OpenSites { get; set; }
        public uint AssignedSites { get; set; }

        public uint TotalSupervisors { get; set; }
        public uint TotalVehicals { get; set; }
        public uint CompletedJobs { get; set; }
        public uint Approvals { get; set; }

        public uint TotalIncidentReports { get; set; }

        public uint UserApprovals { get; set; }
        public uint JobApprovals { get; set; }
        public uint AssignedJobs { get; set; }

        public uint TotalFleetOwner { get; set; }

        public uint CompletedSites { get; set; }

        public uint TotalInProgressSites { get; set; }
    }   
}
