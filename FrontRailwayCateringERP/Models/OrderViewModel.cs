namespace FrontRailwayCateringERP.Models
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public string? CoachNumber { get; set; }
        public string? SeatNumber { get; set; }
        public string? Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public UserViewModel? User { get; set; }
        public Guid JourneyId { get; set; }
        public JourneyViewModel? Journey { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string KitchenStatus { get; set; } = "Pending";
        public Guid OrderId { get; set; }
        public Guid MenuItemId { get; set; }
        public MenuItemViewModel? MenuItem { get; set; }
    }

    public class CreateOrderViewModel
    {
        public string? CoachNumber { get; set; }
        public string? SeatNumber { get; set; }
        public Guid JourneyId { get; set; }
        public List<JourneyViewModel> JourneyList { get; set; } = new();
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
        public List<Guid> SelectedItemIds { get; set; } = new();
        public List<int> Quantities { get; set; } = new();
    }
}
