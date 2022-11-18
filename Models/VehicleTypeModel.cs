using System.Collections.Generic;

namespace Models
{
    public class VehicleTypeModel
    {
        public VehicleTypeModel()
        {
            Specifications = new List<SpecificationModel>();
        }
        public class SpecificationModel
        {
            public uint Id { get; set; }
            public string Specification { get; set; }
            public float Value { get; set; }
        }

        public uint Id { get; set; }
        public string VehicleType { get; set; }
        public float BasePrice { get; set; }
        public bool IsActive { get; set; }
        public string ImageString { get; set; }
        public List<SpecificationModel> Specifications { get; set; }

        public string Status => IsActive ? "Active" : "In-Active";
        public int TotalSepcs => Specifications.Count;
    }
}
