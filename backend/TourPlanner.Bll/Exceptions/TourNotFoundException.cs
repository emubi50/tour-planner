namespace TourPlanner.Bll.Exceptions
{
    public class TourNotFoundException : Exception
    {
        public TourNotFoundException() { }
        public TourNotFoundException(string? message) : base(message) { }
        public TourNotFoundException(string? message, Exception? inner) : base(message, inner) { }
    }
}
