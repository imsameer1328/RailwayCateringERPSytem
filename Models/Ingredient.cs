using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class Ingredient
    {
        public Guid IngredientId { get; set; } = Guid.NewGuid();
      
        public string Name { get; set; }
        public decimal CurrentStock { get; set; } = 0;   
        public decimal MinimumThreshold { get; set; }      
        public string Unit { get; set; }

        // One Ingredient is used in many MenuItems
        public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; } = new List<MenuItemIngredient>();

        // One Ingredient has many InventoryLogs
        public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    }
}