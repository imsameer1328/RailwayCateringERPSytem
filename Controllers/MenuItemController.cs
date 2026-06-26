using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all menu items
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var menuItems = await _context.MenuItems.ToListAsync();
            return Ok(menuItems);
        }

        // GET one menu item by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(Guid id)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (menuItem == null)
                return NotFound("Menu item not found");

            return Ok(menuItem);
        }

        // POST — create a new menu item
        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return Ok(menuItem);
        }

        // PUT — update existing menu item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] MenuItem updatedMenuItem)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null)
                return NotFound("Menu item not found");

            menuItem.Name = updatedMenuItem.Name;
            menuItem.Category = updatedMenuItem.Category;
            menuItem.Price = updatedMenuItem.Price;
            menuItem.AvailabilityStatus = updatedMenuItem.AvailabilityStatus;

            await _context.SaveChangesAsync();
            return Ok(menuItem);
        }

        // DELETE — delete a menu item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(Guid id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null)
                return NotFound("Menu item not found");

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return Ok("Menu item deleted successfully");
        }
    }
}