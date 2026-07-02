using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("geocoding")]
        public GeoCoding Geocoding { get; set; } = new GeoCoding();

        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; } = [];
    }
}
