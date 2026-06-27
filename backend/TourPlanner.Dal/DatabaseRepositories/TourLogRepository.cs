using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TourPlanner.Dal.Exceptions;
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

        public async Task<List<TourLog>> GetAllByTourIdAsync(int tourId)
        {
            return await _context.TourLogs.Where(l => l.TourId == tourId).ToListAsync();
        }

        public async Task<TourLog?> GetByIdAsync(int tourId, int tourLogId)
        {
            return await _context.TourLogs.FirstOrDefaultAsync(l => l.Id == tourLogId);
        }

        public async Task AddAsync(int tourId, TourLog log)
        {
            try
            {
                _context.TourLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is ArgumentException)
            {
                throw new DuplicateKeyException($"TourLog with id {log.Id} already exists", ex);
            }
        }

        public async Task DeleteAsync(int tourId, int tourLogId)
        {
            TourLog? log = await GetByIdAsync(tourId, tourLogId);
            if (log != null)
            {
                _context.TourLogs.Remove(log);
                await _context.SaveChangesAsync();
                return;
            }
            throw new ArgumentException($"TourLog with id {tourLogId} not found.");
        }

        public async Task UpdateAsync(int tourId, TourLog log)
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
