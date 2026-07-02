using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.ORS.Direction
{
    public class Summary
    {
        [JsonPropertyName("distance")]
        public float Distance { get; set; }

        [JsonPropertyName("duration")]
        public float Duration { get; set; }
    }
}
