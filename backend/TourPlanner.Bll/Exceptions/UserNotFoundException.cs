namespace TourPlanner.Bll.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() { }
        public UserNotFoundException(string message) { }
        public UserNotFoundException (string message, Exception innerException) { }
    }
}
