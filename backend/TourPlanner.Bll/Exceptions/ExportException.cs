namespace TourPlanner.Bll.Exceptions
{
    public class ExportException : Exception
    {
        public ExportException() { }
        public ExportException(string message) : base(message) { }
        public ExportException(string message, Exception inner) : base(message, inner) { }
    }
}
