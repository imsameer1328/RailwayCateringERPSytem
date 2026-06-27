using System.Text.Json.Serialization;

namespace FrontRailwayCateringERP.Models
{
    public class MenuItemViewModel
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string AvailabilityStatus { get; set; }

        [JsonPropertyName("menuItemIngredients")]
        public List<IngredientSelection> Ingredients { get; set; } = new();
    }

    public class IngredientSelection
    {
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal QuantityNeeded { get; set; }
        public string Unit { get; set; }
    }
}
