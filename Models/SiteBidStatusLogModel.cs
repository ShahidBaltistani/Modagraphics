using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SiteBidStatusLogModel
    {
        public uint Id { get; set; }
        public uint SiteId { get; set; }
        public SiteStatus SiteStatus { get; set; }
    }
}
