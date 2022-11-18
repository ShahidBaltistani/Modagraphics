namespace Models
{
    public class LoginModel
    {
        public uint Id { get; set; }
        public uint AssociatedId { get; set; }
        public UserRoles UserRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int TimeZoneOffset { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string Name { get; set; } //From user table
        public string Email { get; set; } 
    }
}
