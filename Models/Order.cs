using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public string? CoachNumber { get; set; }
        public string? SeatNumber { get; set; }
        public string? Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Foreign Key — which User placed this order
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Foreign Key — which Journey this order belongs to
        public Guid JourneyId { get; set; }
        public Journey? Journey { get; set; }

        // Navigation Property — one Order has many OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}