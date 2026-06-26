using System;
using System.Collections.Generic;

namespace FrontRailwayCateringERP.Models
{
    public class TrainViewModel
    {
        public Guid TrainId { get; set; }
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public int TotalCoaches { get; set; }

        // Collection of journeys (will map to JourneyViewModel later)
        //public List<JourneyViewModel> Journeys { get; set; } = new();
    }
}
