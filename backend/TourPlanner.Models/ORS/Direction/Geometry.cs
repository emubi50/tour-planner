using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.ORS.Direction
{
    public class Geometry
    {
        [JsonPropertyName("coordinates")]
        public List<double[]> Coordinates { get; set; } = [];
    }
}
