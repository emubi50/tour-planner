using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Bll.Exceptions
{
    public class TourAlreadyExistsException : Exception
    {
        public TourAlreadyExistsException() { }
        public TourAlreadyExistsException(string? message) : base(message) { }
        public TourAlreadyExistsException(string? message, Exception? inner) : base(message, inner) { }
    }
}
