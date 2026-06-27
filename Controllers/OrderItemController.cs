using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;
using System.Text.Json;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all order items
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.MenuItem)
                .ToListAsync();
            return Ok(orderItems);
        }

        // GET one order item by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(Guid id)
        {
            var orderItem = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.MenuItem)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == id);
            if (orderItem == null)
                return NotFound("Order item not found");
            return Ok(orderItem);
        }

        // GET all order items by order id
        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrderId(Guid orderId)
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.MenuItem)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
            return Ok(orderItems);
        }

        // POST — create a new order item
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return Ok(orderItem);
        }

        // PUT — update kitchen status only (preserves Quantity/UnitPrice)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(Guid id, [FromBody] JsonElement body)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
                return NotFound("Order item not found");

            if (body.TryGetProperty("kitchenStatus", out var ks))
                orderItem.KitchenStatus = ks.GetString() ?? orderItem.KitchenStatus;

            await _context.SaveChangesAsync();
            return Ok(orderItem);
        }

        // DELETE — delete an order item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
                return NotFound("Order item not found");

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return Ok("Order item deleted successfully");
        }
    }
}