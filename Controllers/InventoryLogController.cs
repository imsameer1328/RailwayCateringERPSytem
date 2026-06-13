using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all inventory logs
        [HttpGet("all")]
        public async Task<IActionResult> GetAllInventoryLogs()
        {
            var logs = await _context.InventoryLogs
                .Include(l => l.Ingredient)
                .Include(l => l.Journey)
                .Include(l => l.LoggedBy)
                .ToListAsync();
            return Ok(logs);
        }

        // GET one inventory log by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryLogById(Guid id)
        {
            var log = await _context.InventoryLogs
                .Include(l => l.Ingredient)
                .Include(l => l.Journey)
                .Include(l => l.LoggedBy)
                .FirstOrDefaultAsync(l => l.InventoryLogId == id);
            if (log == null)
                return NotFound("Inventory log not found");
            return Ok(log);
        }

        // GET all logs by journey id
        [HttpGet("by-journey/{journeyId}")]
        public async Task<IActionResult> GetLogsByJourneyId(Guid journeyId)
        {
            var logs = await _context.InventoryLogs
                .Include(l => l.Ingredient)
                .Include(l => l.LoggedBy)
                .Where(l => l.JourneyId == journeyId)
                .ToListAsync();
            return Ok(logs);
        }

        // POST — create a new inventory log
        [HttpPost]
        public async Task<IActionResult> CreateInventoryLog([FromBody] InventoryLog inventoryLog)
        {
            _context.InventoryLogs.Add(inventoryLog);
            await _context.SaveChangesAsync();
            return Ok(inventoryLog);
        }

        // PUT — update inventory log
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryLog(Guid id, [FromBody] InventoryLog updatedLog)
        {
            var log = await _context.InventoryLogs.FindAsync(id);
            if (log == null)
                return NotFound("Inventory log not found");

            log.ActionType = updatedLog.ActionType;
            log.QuantityChange = updatedLog.QuantityChange;
            log.StockAfter = updatedLog.StockAfter;

            await _context.SaveChangesAsync();
            return Ok(log);
        }

        // DELETE — delete inventory log
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryLog(Guid id)
        {
            var log = await _context.InventoryLogs.FindAsync(id);
            if (log == null)
                return NotFound("Inventory log not found");

            _context.InventoryLogs.Remove(log);
            await _context.SaveChangesAsync();
            return Ok("Inventory log deleted successfully");
        }
    }
}