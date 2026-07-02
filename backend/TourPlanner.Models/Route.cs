using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class Route
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public IReadOnlyList<double[]> Path { get; set; } = [];
    }
}
