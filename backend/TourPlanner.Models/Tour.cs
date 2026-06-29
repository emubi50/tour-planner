namespace TourPlanner.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public required int UserId { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string From { get; set; }
        public required string To { get; set; }
        public required TransportType TransportType { get; set; }
        public required int Distance { get; set; } // provided by openrouteservice.org
        public required int EstimatedTime { get; set; } // provided by openrouteservice.org

        public List<TourLog> Logs { get; set; } = new List<TourLog>();
    }
}
