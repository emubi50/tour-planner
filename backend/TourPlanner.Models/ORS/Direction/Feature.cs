using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models.ORS.Direction
{
    public class Feature
    {
        public Properties Properties { get; set; } = new Properties();
        public Geometry Geometry { get; set; } = new Geometry();
    }
}
