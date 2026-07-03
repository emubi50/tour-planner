using NpgsqlTypes;
using TourPlanner.Models.Enums;

namespace TourPlanner.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public required int UserId { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Location From { get; set; }
        public required Location To { get; set; }
        public required TransportType TransportType { get; set; }
        public required double Distance { get; set; } // provided by openrouteservice.org
        public required double EstimatedTime { get; set; } // provided by openrouteservice.org
        public required string RouteInformation { get; set; } // provided by openrouteservice.org
        public List<TourLog> Logs { get; set; } = new List<TourLog>();

        public PopularityLevel Popularity { get; set; }
        public ChildFriendlinessLevel ChildFriendliness { get; set; }
        public NpgsqlTsVector SearchVector { get; set; } = null!;
    }
}
