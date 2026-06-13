using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all reports
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _context.Reports
                .Include(r => r.Journey)
                .Include(r => r.GeneratedBy)
                .ToListAsync();
            return Ok(reports);
        }

        // GET one report by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var report = await _context.Reports
                .Include(r => r.Journey)
                .Include(r => r.GeneratedBy)
                .FirstOrDefaultAsync(r => r.ReportId == id);
            if (report == null)
                return NotFound("Report not found");
            return Ok(report);
        }

        // GET report by journey id
        [HttpGet("by-journey/{journeyId}")]
        public async Task<IActionResult> GetReportByJourneyId(Guid journeyId)
        {
            var report = await _context.Reports
                .Include(r => r.Journey)
                .Include(r => r.GeneratedBy)
                .FirstOrDefaultAsync(r => r.JourneyId == journeyId);
            if (report == null)
                return NotFound("Report not found for this journey");
            return Ok(report);
        }

        // POST — generate a new report
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] Report report)
        {
            // check if report already exists for this journey
            var existingReport = await _context.Reports
                .FirstOrDefaultAsync(r => r.JourneyId == report.JourneyId);
            if (existingReport != null)
                return BadRequest("A report already exists for this journey");

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return Ok(report);
        }

        // PUT — update report
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(Guid id, [FromBody] Report updatedReport)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
                return NotFound("Report not found");

            report.TotalOrders = updatedReport.TotalOrders;
            report.TotalRevenue = updatedReport.TotalRevenue;
            report.StockConsumedSummary = updatedReport.StockConsumedSummary;

            await _context.SaveChangesAsync();
            return Ok(report);
        }

        // DELETE — delete report
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
                return NotFound("Report not found");

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return Ok("Report deleted successfully");
        }
    }
}