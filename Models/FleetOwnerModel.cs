namespace Models
{
    public class FleetOwnerModel
    {
        public uint Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }

        public uint? PZipCode { get; set; }

        public uint PCityId { get; set; }

        public string Picture { get; set; }

        public string CompanyName { get; set; }

        public string BillToAddress { get; set; }

        public uint? CZipCode { get; set; }

        public uint CCityId { get; set; }

        public string FaxNo { get; set; }

        public string CContactNo { get; set; }

        public string BIEmbededCode { get; set; }

        public uint NumberOfSite { get; set; }
        public uint NumberOfSupervisor { get; set; }
        public uint NumberOfProject { get; set; }

        public uint CorporateUserId { get; set; }

        public string UserCorporateName { get; set; }

        public FleetOwnerStatus Status { get; set; }
        public uint TotalProjects { get; set; }
        public uint TotalSites { get; set; }
        public uint TotalSupervisors { get; set; }

    }
}
