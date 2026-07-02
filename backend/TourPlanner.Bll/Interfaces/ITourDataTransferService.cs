using TourPlanner.Bll.Strategies;

namespace TourPlanner.Bll.Interfaces
{
    public interface ITourDataTransferService
    {
        Task<byte[]> ExportToursAsync(string username, string format);
        Task<ImportResult> ImportToursAsync(string username, byte[] fileContent);
    }
}
