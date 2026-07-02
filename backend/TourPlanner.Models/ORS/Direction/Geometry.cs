using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Direction
{
    public class Geometry
    {
        public IReadOnlyList<double[]> Coordinates { get; set; } = [];
    }
}
