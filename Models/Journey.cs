using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class Journey
    {
        public Guid JourneyId { get; set; } = Guid.NewGuid();

        [Required]
        public DateOnly JourneyDate { get; set; }

        [Required]
        public TimeOnly DepartureTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Origin { get; set; }

        [Required]
        [MaxLength(100)]
        public string Destination { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Scheduled";


        // Foreign Key — which Train is running
        public Guid TrainId { get; set; }
        public Train? Train { get; set; }

        // Foreign Key — which Manager is responsible
        public Guid ManagerId { get; set; }
        public User? Manager { get; set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
        public Report? Report { get; set; }
    }
}