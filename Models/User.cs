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

        // Foreign Key
        public Guid RoleId { get; set; }

        // Navigation property: Each User belongs to one Role
        public Role Role { get; set; }
    }
}