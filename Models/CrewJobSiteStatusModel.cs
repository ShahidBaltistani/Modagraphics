
using System;

namespace Models
{
    public class CrewJobSiteStatusModel: CrewSiteModel
    {
        public string Project { get; set; }
        public string Site { get; set; }
        //public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CrewJobCompletedStatusModel : CrewJobSiteStatusModel
    {
        public DateTime? ApproveDate { get; set; }
    }
}
