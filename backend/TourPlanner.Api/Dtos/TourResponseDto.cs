using NpgsqlTypes;
using TourPlanner.Models;
using TourPlanner.Models.Enums;

namespace TourPlanner.Api.Dtos
{
    public class TourResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TransportType TransportType { get; set; }
        public double Distance { get; set; } // provided by openrouteservice.org
        public double EstimatedTime { get; set; } // provided by openrouteservice.org
        public string RouteInformation { get; set; } // provided by openrouteservice.org
        public List<TourLogResponseDto> Logs { get; set; } = new List<TourLogResponseDto>();
    
        public TourResponseDto(Tour tour)
        {
            Id = tour.Id;
            UserId = tour.UserId;
            Description = tour.Description;
            From = tour.From;
            To = tour.To;
            TransportType = tour.TransportType;
            Distance = tour.Distance;
            EstimatedTime = tour.EstimatedTime;
            RouteInformation = tour.RouteInformation;
            Logs = (tour.Logs ?? new List<TourLog>())
                .Select(l => new TourLogResponseDto(l))
                .ToList();
        }
    }
}
