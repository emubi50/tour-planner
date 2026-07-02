using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.ORS.Geocode
{
    /// <summary>
    /// Represents the geocoding information returned by the OpenRouteService API.
    /// <para>
    /// <strong>Note:</strong> Intentionally strips "useless" information from the OpenRouteService response to reduce unnecessary memory usage and bloat.
    /// </para>
    /// </summary>
    public class GeoCoding
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
