using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.DatabaseRepositories
{
    public class TourLogRepository : ITourLogRepository
    {
        private readonly TourPlannerDbContext _context;

        public TourLogRepository(TourPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<TourLog>> GetAllByTourIdAsync(int id)
        {
            return await _context.TourLogs.Where(l => l.TourId == id).ToListAsync();
        }

        public async Task<TourLog?> GetByIdAsync(int id)
        {
            return await _context.TourLogs.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(TourLog log)
        {
            _context.TourLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            TourLog? log = await GetByIdAsync(id);
            if (log != null)
            {
                _context.TourLogs.Remove(log);
                await _context.SaveChangesAsync();
                return;
            }
            throw new ArgumentException($"TourLog with id {id} not found.");
        }

        public async Task UpdateAsync(TourLog log)
        {
            if (await _context.TourLogs.AnyAsync(l => l.Id == log.Id))
            {
                _context.TourLogs.Update(log);
                await _context.SaveChangesAsync();
                return;
            }
            throw new ArgumentException($"TourLog with id {log.Id} not found.");
        }
    }
}
