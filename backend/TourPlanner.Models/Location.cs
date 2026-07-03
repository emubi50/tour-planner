using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class Location
    {
        public required double Longitude { get; set; }
        public required double Latitude { get; set; }

        public required string Label { get; set; }

        public double[] Coordinates => [Longitude, Latitude];

        public Location(double longitude, double latitude, string label)
        {
            Longitude = longitude;
            Latitude = latitude;
            Label = label;
        }

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
