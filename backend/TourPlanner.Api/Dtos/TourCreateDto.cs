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

        [Required]
        public TransportType TransportType { get; set; }

        [Required]
        public Location From { get; set; } = null!;

        [Required]
        public Location To { get; set; } = null!;

        public Tour toTour()
        {
            var tour = new Tour()
            {
                UserId = 0, // Placeholder value to be set in TourService
                Name = this.Name,
                Description = this.Description,
                From = this.From,
                To = this.To,
                TransportType = this.TransportType,
                Distance = 0, // Placeholder value to be set in TourService
                EstimatedTime = 0, // Placeholder value to be set in TourService
                RouteInformation = string.Empty, // Placeholder value to be set in TourService
            };
            return tour;
        }
    }
}
