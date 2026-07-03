using System.ComponentModel.DataAnnotations;
using TourPlanner.Models;

namespace TourPlanner.Api.Dtos
{
    public class TourLogCreateDto
    {
        [Required]
        public int TourId { get; set; } = 0;
        [Required]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        [Required]
        public string comment { get; set; } = string.Empty;
        [Required]
        public int Difficulty { get; set; } = 1;
        [Required]
        public int TotalDistance { get; set; } = 1; // in meters
        [Required]
        public int TotalTime { get; set; } = 1; // in seconds
        [Required]
        public int Rating { get; set; } = 1;

        public TourLog ToTourLog()
        {
            var tourLog = new TourLog
            {
                TourId = this.TourId,
                Date = this.Date,
                Comment = this.comment,
                Difficulty = this.Difficulty,
                TotalDistance = this.TotalDistance,
                TotalTime = this.TotalTime,
                Rating = this.Rating,
                Tour = null // Placeholder value to be set in TourLogService
            };
            return tourLog;
        }
    }
}
