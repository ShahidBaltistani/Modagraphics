namespace Models
{
    public class UserModel
    {
        public uint Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public uint CityId { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public uint? ZipCode { get; set; }
        public UserStatus Status { get; set; }
        public UserType UserType { get; set; }

        public bool IsEmailVerified { get; set; }

        public string Name { get; set; }
        public UserRoles Role { get; set; }
    }

}
