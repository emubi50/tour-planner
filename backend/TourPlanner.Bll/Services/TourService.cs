using TourPlanner.Bll.Interfaces;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task<List<Tour>> GetAllAsync()
        {
            return await _tourRepository.GetAllAsync();
        }

        public async Task<Tour?> GetByIdAsync(int userId, int tourId)
        {
            return await _tourRepository.GetByIdAsync(userId, tourId);
        }

        public async Task CreateTourAsync(int userId, Tour tour)
        {
            await _tourRepository.AddAsync(userId, tour);
        }

        public async Task DeleteTourAsync(int userId, int tourId)
        {
            await _tourRepository.DeleteAsync(userId, tourId);
        }
    }
}
