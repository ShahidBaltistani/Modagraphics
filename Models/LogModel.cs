using System;

namespace Models
{
    public class LogModel
    {
        public uint Id { get; set; }

        public string IP { get; set; }

        public DateTime EnteredOn { get; set; }

        public string UserAgent { get; set; }
    }
}
