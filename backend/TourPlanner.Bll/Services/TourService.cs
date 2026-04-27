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

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _tourRepository.GetByIdAsync(id);
        }

        public async Task CreateTourAsync(Tour tour)
        {
            await _tourRepository.AddAsync(tour);
        }
    }
}
