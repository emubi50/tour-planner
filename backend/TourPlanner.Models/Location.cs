using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class Location
    {
        public required double[] Coordinates { get; set; }
        public required string Label { get; set; }

        // Confidence value for sorting
        public required float Confidence { get; set; }
    }
}
