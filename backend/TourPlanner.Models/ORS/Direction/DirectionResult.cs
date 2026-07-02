using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Direction
{
    public class DirectionResult
    {
        public IList<Feature> Features { get; set; } = new List<Feature>();
    }
}
