using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.ORS.Direction
{
    public class Feature
    {
        [JsonPropertyName("properties")]
        public Properties Properties { get; set; } = new Properties();

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = new Geometry();
    }
}
