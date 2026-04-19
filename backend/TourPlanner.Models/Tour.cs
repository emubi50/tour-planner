namespace TourPlanner.Models
{
    public class Tour
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string From { get; set; }
        public required string To { get; set; }
        public required string TransportType { get; set; }
        public required string Distance { get; set; }
        public required string EstimatedTime { get; set; }

        public TourLog[] Logs { get; set; } = Array.Empty<TourLog>();
    }
}
