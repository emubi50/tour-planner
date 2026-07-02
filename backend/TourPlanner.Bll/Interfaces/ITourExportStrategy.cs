using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface ITourExportStrategy
    {
        byte[] Export(IEnumerable<Tour> tours);
        string FileExtension { get; }
    }
}
