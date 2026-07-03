using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class LocationSearchResult
    {
        public long Timestamp { get; set; }
        public List<Location> Locations { get; set; } = [];
    }
}
