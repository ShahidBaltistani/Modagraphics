namespace Models
{
    public class CorporateUserModel
    {
        public uint Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }

        public uint? ZipCode { get; set; }

        public uint CityId { get; set; }

        public string Picture { get; set; }

        public CorporateUserStatus Status { get; set; }
    }
}
