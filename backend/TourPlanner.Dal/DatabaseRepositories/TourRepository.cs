using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Tour>> GetAllAsync()
        {
            return await _context.Tours.Include(t => t.Logs).ToListAsync();
        }

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _context.Tours.Include(t => t.Logs).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Tour? tour = await GetByIdAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                return;
            }
            throw new KeyNotFoundException($"Tour with id {id} not found.");
        }

        public async Task UpdateAsync(Tour tour)
        {
            if (await _context.Tours.AnyAsync(t => t.Id == tour.Id))
            {
                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();
                return;
            }
            throw new KeyNotFoundException($"Tour with id {tour.Id} not found.");
        }
    }
}
