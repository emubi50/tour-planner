using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Geocode
{
    /// <summary>
    /// Represents an OpenRouteService "Geometry" object
    /// <para>
    /// <strong>Note:</strong> Intentionally strips "useless" information from the OpenRouteService response to reduce unnecessary memory usage and bloat.
    /// </para>
    /// </summary>
    public class Geometry
    {
        public double[] Coordinates { get; set; } = [];
    }
}
