namespace TourPlanner.Bll.Exceptions
{
    public class TourLogNotFoundException : Exception
    {
        public TourLogNotFoundException() { }

        public TourLogNotFoundException(string? message)
            : base(message) { }

        public TourLogNotFoundException(string? message, Exception? inner)
            : base(message, inner) { }
    }
}
