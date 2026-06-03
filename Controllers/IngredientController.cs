using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all ingredients
        [HttpGet("all")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _context.Ingredients
                .Include(i => i.MenuItemIngredients)
                .ToListAsync();
            return Ok(ingredients);
        }

        // GET one ingredient by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredientById(Guid id)
        {
            var ingredient = await _context.Ingredients
                .Include(i => i.MenuItemIngredients)
                .FirstOrDefaultAsync(i => i.IngredientId == id);

            if (ingredient == null)
                return NotFound("Ingredient not found");

            return Ok(ingredient);
        }

        // POST — create a new ingredient
        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromBody] Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return Ok(ingredient);
        }

        // PUT — update existing ingredient
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(Guid id, [FromBody] Ingredient updatedIngredient)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
                return NotFound("Ingredient not found");

            ingredient.Name = updatedIngredient.Name;
            ingredient.CurrentStock = updatedIngredient.CurrentStock;
            ingredient.MinimumThreshold = updatedIngredient.MinimumThreshold;
            ingredient.Unit = updatedIngredient.Unit;

            await _context.SaveChangesAsync();
            return Ok(ingredient);
        }

        // DELETE — delete an ingredient
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
                return NotFound("Ingredient not found");

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return Ok("Ingredient deleted successfully");
        }
    }
}