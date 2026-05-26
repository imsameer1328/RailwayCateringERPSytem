using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";


        // Foreign Key — One User has only one Role
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }

        // One User places many Orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        // One User manages many Journeys
        public ICollection<Journey> Journeys { get; set; } = new List<Journey>();

        // One User logs many InventoryLogs
        public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

        // One User generates many Reports
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}