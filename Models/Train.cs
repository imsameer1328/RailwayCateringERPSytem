using System.ComponentModel.DataAnnotations;

namespace RailwayCateringERPSystem.Models
{
    public class Train
    {
        public Guid TrainId { get; set; } = Guid.NewGuid();
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public int TotalCoaches { get; set; }

        public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
    }
}