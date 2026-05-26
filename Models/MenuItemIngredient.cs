using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class MenuItemIngredient
    {
        public Guid MenuItemIngredientId { get; set; } = Guid.NewGuid();

        [Required]
        public decimal QuantityNeeded { get; set; }

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }

        // Foreign Key — which MenuItem
        public Guid MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        // Foreign Key — which Ingredient
        public Guid IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}