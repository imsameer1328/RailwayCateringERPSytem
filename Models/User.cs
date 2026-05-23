namespace RailwayCateringERPSystem.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; } = "Active";


        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}