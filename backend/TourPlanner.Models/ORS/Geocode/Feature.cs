using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = new Geometry();

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; } = new Properties();
    }
}
