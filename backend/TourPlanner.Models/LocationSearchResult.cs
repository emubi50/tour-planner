using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class LocationSearchResult
    {
        public DateTime Timestamp { get; set; }
        public IEnumerable<Location> Locations { get; set; } = [];
    }
}
