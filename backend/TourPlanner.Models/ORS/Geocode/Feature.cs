using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Geocode
{
    /// <summary>
    /// Represents an OpenRouteService "Feature" object
    /// <para>
    /// <strong>Note:</strong> Intentionally strips "useless" information from the OpenRouteService response to reduce unnecessary memory usage and bloat.
    /// </para>
    /// </summary>
    public class Feature
    {
        public Geometry Geometry { get; set; } = new Geometry();
        public required Properties Properties { get; set; }
    }
}
