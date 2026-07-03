using Microsoft.EntityFrameworkCore;
using TourPlanner.Dal.Exceptions;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.DatabaseRepositories
{
    public class TourRepository : ITourRepository
    {
        private readonly TourPlannerDbContext _context;

        public TourRepository(TourPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tour>> GetAllByUserIdAsync(int userId)
        {
            return await _context
                .Tours.Where(t => t.UserId == userId)
                .Include(t => t.Logs)
                .ToListAsync();
        }

        public async Task<Tour?> GetByIdAsync(int tourId)
        {
            return await _context
                .Tours.Include(t => t.Logs)
                .FirstOrDefaultAsync(t => t.Id == tourId);
        }

        public async Task AddAsync(Tour tour)
        {
            try
            {
                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is ArgumentException)
            {
                throw new DuplicateKeyException($"Tour with id {tour.Id} already exists", ex);
            }
        }

        public async Task DeleteAsync(int tourId)
        {
            Tour? tour = await GetByIdAsync(tourId);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                return;
            }
            throw new KeyNotFoundException($"Tour with id {tourId} not found.");
        }

        public async Task UpdateAsync(Tour tour)
        {
            var trackedTour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tour.Id);

            if (trackedTour == null)
            {
                throw new KeyNotFoundException($"Tour with id {tour.Id} not found.");
            }

            // set the properties individually because it screams at me when i use update

            trackedTour.Name = tour.Name;
            trackedTour.Description = tour.Description;
            trackedTour.TransportType = tour.TransportType;
            trackedTour.From = tour.From;
            trackedTour.To = tour.To;
            trackedTour.Distance = tour.Distance;
            trackedTour.EstimatedTime = tour.EstimatedTime;
            trackedTour.RouteInformation = tour.RouteInformation;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Tour>> SearchAsync(int userId, string query)
        {
            return await _context.Tours
                .Where(t => t.UserId == userId)
                .Include(t => t.Logs)
                .Where(t => t.SearchVector.Matches(EF.Functions.WebSearchToTsQuery("english", query)))
                .OrderByDescending(t => t.SearchVector.Rank(EF.Functions.WebSearchToTsQuery("english", query)))
                .ToListAsync();
        }
    }
}
