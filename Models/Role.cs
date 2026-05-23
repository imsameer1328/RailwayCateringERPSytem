namespace RailwayCateringERPSystem.Models
{
    public class Role
    {
        public Guid RoleId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Permissions { get; set; }

        // Navigation property: One Role → Many Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}