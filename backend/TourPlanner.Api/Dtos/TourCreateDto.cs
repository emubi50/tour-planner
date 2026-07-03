using System.ComponentModel.DataAnnotations;
using TourPlanner.Models;
using TourPlanner.Models.Enums;

namespace TourPlanner.Api.Dtos
{
    public class TourCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();

        [Required]
        public TransportType TransportType { get; set; }

        [Required]
        public LocationDto From { get; set; } = default!;

        [Required]
        public LocationDto To { get; set; } = default!;

        public Tour toTour()
        {
            var tour = new Tour()
            {
                UserId = 0, // Placeholder value to be set in TourService
                Name = this.Name,
                Description = this.Description,
                From = new Location(
                    this.From.Coordinates[0],
                    this.From.Coordinates[1],
                    this.From.Label
                ),
                Tags = this.Tags,
                To = new Location(this.To.Coordinates[0], this.To.Coordinates[1], this.To.Label),
                TransportType = this.TransportType,
                Distance = 0, // Placeholder value to be set in TourService
                EstimatedTime = 0, // Placeholder value to be set in TourService
                RouteInformation = string.Empty, // Placeholder value to be set in TourService
            };
            return tour;
        }
    }
}
