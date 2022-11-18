using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class JobStatusLogModel
    {
        public uint Id { get; set; }
        public uint JobId { get; set; }
        public string LPN { get; set; }
        public string CreatedBy { get; set; }
        public JobStatus JobStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
