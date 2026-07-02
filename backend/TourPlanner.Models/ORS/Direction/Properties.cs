using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models.ORS.Direction
{
    public class Properties
    {
        [JsonPropertyName("summary")]
        public Summary Summary { get; set; } = new Summary();
    }
}
