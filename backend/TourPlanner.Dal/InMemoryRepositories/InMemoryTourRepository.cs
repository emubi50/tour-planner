using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.InMemoryRepositories
{
    public class InMemoryTourRepository : ITourRepository
    {
        private readonly List<Tour> _tours = [];

        public IEnumerable<Tour> GetAllTours()
        {
            return _tours?.ToArray() ?? Enumerable.Empty<Tour>();
        }

        public Tour? GetTourById(int tourId)
        {
            return GetAllTours().FirstOrDefault(tour => tour.Id == tourId);
        }

        public void InsertTour(Tour tour)
        {
            _tours.Add(tour);
        }

        public void UpdateTour(Tour tour)
        {
            Tour updateTour = GetTourById(tour.Id) ?? throw new KeyNotFoundException($"Tou with ID {tour.Id} not found");

            updateTour.Name = tour.Name;
            updateTour.Description = tour.Description;
            updateTour.From = tour.From;
            updateTour.To = tour.To;
            updateTour.TransportType = tour.TransportType;
            updateTour.Distance = tour.Distance;
            updateTour.EstimatedTime = tour.EstimatedTime;
        }

        public bool DeleteTour(int tourId)
        {
            bool found = false;

            Tour? tour = _tours.FirstOrDefault(t => t.Id == tourId);
            if (tour != null)
            {
                found = true;
                _tours.Remove(tour);
            }

            return found;
        }
    }
}
