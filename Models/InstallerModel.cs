
namespace Models
{
    public class InstallerModel
    {
        public uint Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }

        public uint? PZipCode { get; set; }

        public uint PCity { get; set; }

        public string Picture { get; set; }

        public float TravelInMiles { get; set; }

        public bool IsPreferred { get; set; }

        public string CompanyName { get; set; }

        public string BillToAddress { get; set; }

        public uint? CZipCode { get; set; }

        public uint CCity { get; set; }

        public string FaxNo { get; set; }

        public string CContactNo { get; set; }

        public string CompanyEmail { get; set; }

        public string FEINumber { get; set; }

        public uint NoOfEmployees { get; set; }

        public string CertificationTraining { get; set; }

        public bool IsInstallersEmployee { get; set; }

        public bool IsEmployeesBackgroundCheck { get; set; }

        public bool IsDrugTest { get; set; }

        public string EquipmentOwned { get; set; }

        public string InstallLocations { get; set; }

        public string InstallProjectCompleted { get; set; }

        public uint YearInBusiness { get; set; }

        public bool IsEmailVerified { get; set; }

        public InstallerStatus Status { get; set; }

        public uint AwardedSites { get; set; }

        public uint AssignedSites { get; set; }

        public uint TotalCrew { get; set; }
    }
}
