using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    //Bridge Table Between Order and MenuItem
    public class OrderItem  
    {
        public Guid OrderItemId { get; set; } = Guid.NewGuid();

        [Required]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [Required]
        [MaxLength(20)]
        public string KitchenStatus { get; set; } = "Pending";


        // Foreign Key — which Order this item belongs to
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        // Foreign Key — which MenuItem was ordered
        public Guid MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
    }
}