using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.ORS.Direction
{
    public class DirectionResult
    {
        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; } = new List<Feature>();
    }
}
