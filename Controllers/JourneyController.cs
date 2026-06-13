using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JourneyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all journeys
        [HttpGet("all")]
        public async Task<IActionResult> GetAllJourneys()
        {
            var journeys = await _context.Journeys
                .Include(j => j.Train)
                .Include(j => j.Manager)
                .ToListAsync();
            return Ok(journeys);
        }

        // GET one journey by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJourneyById(Guid id)
        {
            var journey = await _context.Journeys
                .Include(j => j.Train)
                .Include(j => j.Manager)
                .FirstOrDefaultAsync(j => j.JourneyId == id);
            if (journey == null)
                return NotFound("Journey not found");
            return Ok(journey);
        }

        // POST — create a new journey
        [HttpPost]
        public async Task<IActionResult> CreateJourney([FromBody] Journey journey)
        {
            _context.Journeys.Add(journey);
            await _context.SaveChangesAsync();
            return Ok(journey);
        }

        // PUT — update existing journey
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJourney(Guid id, [FromBody] Journey updatedJourney)
        {
            var journey = await _context.Journeys.FindAsync(id);
            if (journey == null)
                return NotFound("Journey not found");

            journey.JourneyDate = updatedJourney.JourneyDate;
            journey.DepartureTime = updatedJourney.DepartureTime;
            journey.Origin = updatedJourney.Origin;
            journey.Destination = updatedJourney.Destination;
            journey.Status = updatedJourney.Status;
            journey.TrainId = updatedJourney.TrainId;
            journey.ManagerId = updatedJourney.ManagerId;

            await _context.SaveChangesAsync();
            return Ok(journey);
        }

        // DELETE — delete a journey
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJourney(Guid id)
        {
            var journey = await _context.Journeys.FindAsync(id);
            if (journey == null)
                return NotFound("Journey not found");

            _context.Journeys.Remove(journey);
            await _context.SaveChangesAsync();
            return Ok("Journey deleted successfully");
        }
    }
}