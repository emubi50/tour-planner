namespace TourPlanner.Bll.Exceptions
{
    public class TourValidationException : Exception
    {
        public TourValidationException() : base() { }
        public TourValidationException(string message) : base(message) { }
        public TourValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
