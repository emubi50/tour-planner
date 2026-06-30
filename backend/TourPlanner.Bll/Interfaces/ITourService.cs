using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface ITourService
    {
        Task<List<Tour>> GetAllAsync(string username);

        Task<Tour?> GetByIdAsync(string username, int tourId);

        Task CreateTourAsync(string username, Tour tour);

        Task<bool> UpdateTourAsync(string username, Tour tour);

        Task<bool> DeleteTourAsync(string username, int tourId);
    }
}
