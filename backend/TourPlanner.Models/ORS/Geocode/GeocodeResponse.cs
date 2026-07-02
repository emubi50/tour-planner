using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Geocode
{
    /// <summary>
    /// Represents base response structure for requests to OpenRouteService's geocoding endpoint.
    /// <para>
    /// <strong>Note:</strong> Intentionally strips "useless" information from the OpenRouteService response to reduce unnecessary memory usage and bloat.
    /// </para>
    /// </summary>
    public class GeocodeResponse
    {
        public required GeoCoding Geocoding { get; set; }
        public required IReadOnlyList<Feature> Features { get; set; }
    }
}
