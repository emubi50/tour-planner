namespace TourPlanner.Bll.Exceptions
{
    public class TourLogAlreadyExistsException : Exception
    {
        public TourLogAlreadyExistsException() { }

        public TourLogAlreadyExistsException(string? message)
            : base(message) { }

        public TourLogAlreadyExistsException(string? message, Exception? inner)
            : base(message, inner) { }
    }
}
