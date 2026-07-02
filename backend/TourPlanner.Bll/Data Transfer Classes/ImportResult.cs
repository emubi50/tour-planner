namespace TourPlanner.Bll.Strategies
{
    public class ImportResult
    {
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public List<ImportError> Errors { get; set; } = new();

        public ImportResult(int successCount, int failCount, List<ImportError> errors)
        {
            SuccessCount = successCount;
            FailCount = failCount;
            Errors = errors;
        }

        public ImportResult()
        {
        }
    }
}
