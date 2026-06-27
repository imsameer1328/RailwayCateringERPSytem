namespace FrontRailwayCateringERP.Models
{
    public class JourneyViewModel
    {
        public Guid JourneyId { get; set; }
        public DateOnly JourneyDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }

        public Guid TrainId { get; set; }
        public TrainViewModel? Train { get; set; }

        public Guid ManagerId { get; set; }
        public UserViewModel? Manager { get; set; }

        public List<TrainViewModel> TrainList { get; set; } = new();
        public List<UserViewModel> UserList { get; set; } = new();
    }
}
