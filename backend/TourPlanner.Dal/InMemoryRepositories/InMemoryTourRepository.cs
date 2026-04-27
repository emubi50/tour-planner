using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.InMemoryRepositories
{
    public class InMemoryTourRepository : ITourRepository
    {
        private readonly List<Tour> _tours = [];

        public async Task<List<Tour>> GetAllAsync()
        {
            return _tours;
        }

        public async Task<Tour?> GetByIdAsync(int tourId)
        {
            return _tours.FirstOrDefault(t => t.Id == tourId);
        }

        public async Task AddAsync(Tour tour)
        {
            _tours.Add(tour);
        }

        public async Task UpdateAsync(Tour tour)
        {
            Tour updateTour =
                await GetByIdAsync(tour.Id)
                ?? throw new KeyNotFoundException($"Tou with ID {tour.Id} not found");

            updateTour.Name = tour.Name;
            updateTour.Description = tour.Description;
            updateTour.From = tour.From;
            updateTour.To = tour.To;
            updateTour.TransportType = tour.TransportType;
            updateTour.Distance = tour.Distance;
            updateTour.EstimatedTime = tour.EstimatedTime;
        }

        public async Task DeleteAsync(int tourId)
        {
            Tour? tour = await GetByIdAsync(tourId);
            if (tour != null)
            {
                _tours.Remove(tour);
                return;
            }
            throw new KeyNotFoundException($"Tour with id {tourId} not found.");
        }
    }
}
