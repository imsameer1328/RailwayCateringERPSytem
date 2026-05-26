using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class InventoryLog
    {
        public Guid InventoryLogId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string ActionType { get; set; }

        [Required]
        public decimal QuantityChange { get; set; }

        [Required]
        public decimal StockAfter { get; set; }

        public DateTime LoggedAt { get; set; } = DateTime.UtcNow;


        // Foreign Key — which Ingredient was affected
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        // Foreign Key — which Journey this log belongs to
        public Guid JourneyId { get; set; }
        public Journey Journey { get; set; }

        // Foreign Key — who made this stock change
        public Guid LoggedById { get; set; }
        public User LoggedBy { get; set; }
    }
}