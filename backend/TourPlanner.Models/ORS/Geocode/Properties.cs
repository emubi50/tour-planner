using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Geocode
{
    /// <summary>
    /// Represents an OpenRouteService "Properties" object
    /// <para>
    /// <strong>Note:</strong> Intentionally strips "useless" information from the OpenRouteService response to reduce unnecessary memory usage and bloat.
    /// </para>
    /// </summary>
    public class Properties
    {
        public string Layer { get; set; } = string.Empty;
        public required string Name { get; set; }
        public int Confidence { get; set; }
        public string MatchType { get; set; } = string.Empty;
        public required string Label { get; set; }
    }
}
