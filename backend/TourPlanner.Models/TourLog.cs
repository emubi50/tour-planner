using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class TourLog
    {
        public int Id { get; set; }
        public required int TourId { get; set; }

        public required DateTime DateAndTime { get; set; }
        public required string Comment { get; set; }
        public required int Difficulty { get; set; }
        public required int TotalDistance { get; set; }
        public required int TotalTime { get; set; }
        public required int Rating { get; set; }

        public Tour Tour { get; set; } = null!;
    }
}
