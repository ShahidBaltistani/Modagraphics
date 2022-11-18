
using System.Collections.Generic;

namespace Models
{
    public class ChildEntries
    {
        public uint MainId { get; set; }
        public List<uint> ChildIds { get; set; }
    }

    public class ChildEntries<T>
    {
        public int MainId { get; set; }
        public List<T> ChildIds { get; set; }
    }
}
