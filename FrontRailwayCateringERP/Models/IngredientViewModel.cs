namespace FrontRailwayCateringERP.Models
{
    public class IngredientViewModel
    {
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinimumThreshold { get; set; }
        public string Unit { get; set; }
    }
}
