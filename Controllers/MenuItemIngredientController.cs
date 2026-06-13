using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemIngredientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuItemIngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all menu item ingredients
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMenuItemIngredients()
        {
            var menuItemIngredients = await _context.MenuItemIngredients
                .Include(mi => mi.MenuItem)
                .Include(mi => mi.Ingredient)
                .ToListAsync();
            return Ok(menuItemIngredients);
        }

        // GET one menu item ingredient by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemIngredientById(Guid id)
        {
            var menuItemIngredient = await _context.MenuItemIngredients
                .Include(mi => mi.MenuItem)
                .Include(mi => mi.Ingredient)
                .FirstOrDefaultAsync(mi => mi.MenuItemIngredientId == id);
            if (menuItemIngredient == null)
                return NotFound("Menu item ingredient not found");
            return Ok(menuItemIngredient);
        }

        // GET all ingredients by menu item id
        [HttpGet("by-menuitem/{menuItemId}")]
        public async Task<IActionResult> GetIngredientsByMenuItemId(Guid menuItemId)
        {
            var ingredients = await _context.MenuItemIngredients
                .Include(mi => mi.Ingredient)
                .Where(mi => mi.MenuItemId == menuItemId)
                .ToListAsync();
            return Ok(ingredients);
        }

        // POST — link ingredient to menu item
        [HttpPost]
        public async Task<IActionResult> CreateMenuItemIngredient([FromBody] MenuItemIngredient menuItemIngredient)
        {
            _context.MenuItemIngredients.Add(menuItemIngredient);
            await _context.SaveChangesAsync();
            return Ok(menuItemIngredient);
        }

        // PUT — update quantity needed
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItemIngredient(Guid id, [FromBody] MenuItemIngredient updatedItem)
        {
            var menuItemIngredient = await _context.MenuItemIngredients.FindAsync(id);
            if (menuItemIngredient == null)
                return NotFound("Menu item ingredient not found");

            menuItemIngredient.QuantityNeeded = updatedItem.QuantityNeeded;
            menuItemIngredient.Unit = updatedItem.Unit;

            await _context.SaveChangesAsync();
            return Ok(menuItemIngredient);
        }

        // DELETE — remove ingredient from menu item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItemIngredient(Guid id)
        {
            var menuItemIngredient = await _context.MenuItemIngredients.FindAsync(id);
            if (menuItemIngredient == null)
                return NotFound("Menu item ingredient not found");

            _context.MenuItemIngredients.Remove(menuItemIngredient);
            await _context.SaveChangesAsync();
            return Ok("Menu item ingredient deleted successfully");
        }
    }
}