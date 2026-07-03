using System.Diagnostics.CodeAnalysis;
using System.Net;
using TourPlanner.Models;
using TourPlanner.Models.Enums;
using TransportType = TourPlanner.Models.Enums.TransportType;

namespace TourPlanner.Api.Dtos
{
    public class TourSearchResultDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Location From { get; set; }
        public required Location To { get; set; }
        public required TransportType TransportType { get; set; }
        public required double Distance { get; set; }
        public required double EstimatedTime { get; set; }
        public required PopularityLevel Popularity { get; set; }
        public required ChildFriendlinessLevel ChildFriendliness { get; set; }
        public required List<TourLogSummaryDto> Logs { get; set; }

        [SetsRequiredMembers]
        public TourSearchResultDto(Tour tour)
        {
            Id = tour.Id;
            Name = tour.Name;
            Description = tour.Description;
            From = tour.From;
            To = tour.To;
            TransportType = tour.TransportType;
            Distance = tour.Distance;
            EstimatedTime = tour.EstimatedTime;
            Popularity = tour.Popularity;
            ChildFriendliness = tour.ChildFriendliness;
            Logs = (tour.Logs ?? new List<TourLog>())
                .Select(l => new TourLogSummaryDto
                {
                    Id = l.Id,
                    Date = l.Date,
                    Comment = l.Comment,
                    Difficulty = l.Difficulty,
                    Rating = l.Rating,
                })
                .ToList();
        }
    }

    public class TourLogSummaryDto
    {
        public required int Id { get; set; }
        public required DateOnly Date { get; set; }
        public required string Comment { get; set; }
        public required int Difficulty { get; set; }
        public required int Rating { get; set; }
    }
}
