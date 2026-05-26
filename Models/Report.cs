using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class Report
    {
        public Guid ReportId { get; set; } = Guid.NewGuid();

        public int TotalOrders { get; set; } = 0;

        public decimal TotalRevenue { get; set; } = 0;

        [MaxLength(500)]
        public string StockConsumedSummary { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;


        // Foreign Key — one Journey has exactly one Report
        public Guid JourneyId { get; set; }
        public Journey Journey { get; set; }

        // Foreign Key — which Manager generated this report
        public Guid GeneratedById { get; set; }
        public User GeneratedBy { get; set; }
    }
}