using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TourPlanner.Models
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public string Label { get; set; }

        public double[] Coordinates => [Longitude, Latitude];

        public Location(double longitude, double latitude, string label)
        {
            Longitude = longitude;
            Latitude = latitude;
            Label = label;
        }

        [JsonConstructor]
        public Location(double[] coordinates, string label)
        {
            if (coordinates.Length < 2)
            {
                throw new ArgumentException("Coordinates array must have at least two elements: [longitude, latitude]");
            }

            Longitude = coordinates[0];
            Latitude = coordinates[1];
            Label = label;
        }
    }
}
