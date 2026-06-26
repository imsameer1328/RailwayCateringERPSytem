namespace FrontRailwayCateringERP.Models
{
    public class MenuItemViewModel
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string AvailabilityStatus { get; set; }
    }
}
