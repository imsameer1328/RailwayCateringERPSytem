namespace RailwayCateringERPSystem.Models
{
    public class Role
    {
        public Guid RoleId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Permissions { get; set; }

    }
}