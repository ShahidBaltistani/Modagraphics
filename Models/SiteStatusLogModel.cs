using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class SiteStatusLogModel
    {
        public uint Id { get; set; }
        public uint SiteId { get; set; }
        public string SiteName { get; set; }
        public string CreatedBy { get; set; }
        public SiteStatus SiteStatus { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
