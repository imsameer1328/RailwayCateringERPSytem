using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class MenuItem
    {
        public Guid MenuItemId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string AvailabilityStatus { get; set; } = "Available";

    }
}