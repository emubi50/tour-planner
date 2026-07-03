using TourPlanner.Models;

namespace TourPlanner.Api.Dtos
{
    public class TourLogResponseDto
    {

        public int Id { get; set; }
        public int TourId { get; set; }

        public DateOnly Date { get; set; }
        public string Comment { get; set; }
        public int Difficulty { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
        public int Rating { get; set; }

        public TourLogResponseDto(TourLog tourLog)
        {
            Id = tourLog.Id;
            TourId = tourLog.TourId;
            Date = tourLog.Date;
            Comment = tourLog.Comment;
            Difficulty = tourLog.Difficulty;
            Distance = tourLog.TotalDistance;
            Duration = tourLog.TotalTime;
            Rating = tourLog.Rating;
        }
    }
}
