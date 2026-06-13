using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all trains
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTrains()
        {
            var trains = await _context.Trains.ToListAsync();
            return Ok(trains);
        }

        // GET one train by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainById(Guid id)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
                return NotFound("Train not found");

            return Ok(train);
        }

        // POST — create a new train
        [HttpPost]
        public async Task<IActionResult> CreateTrain([FromBody] Train train)
        {
            train.TrainId = Guid.NewGuid();
            _context.Trains.Add(train);
            await _context.SaveChangesAsync();
            return Ok(train);
        }

        // PUT — update existing train
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrain(Guid id, [FromBody] Train updatedTrain)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
                return NotFound("Train not found");

            train.TrainName = updatedTrain.TrainName;
            train.TrainNumber = updatedTrain.TrainNumber;
            train.TotalCoaches = updatedTrain.TotalCoaches;

            await _context.SaveChangesAsync();
            return Ok(train);
        }

        // DELETE — delete a train
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrain(Guid id)
        {
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
                return NotFound("Train not found");

            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
            return Ok("Train deleted successfully");
        }
    }
}