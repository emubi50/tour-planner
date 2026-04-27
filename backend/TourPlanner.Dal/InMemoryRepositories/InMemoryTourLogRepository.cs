using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.InMemoryRepositories
{
    public class InMemoryTourLogRepository : ITourLogRepository
    {
        private readonly List<TourLog> _tourLogs = [];

        public async Task<List<TourLog>> GetAllByTourIdAsync(int tourId)
        {
            return _tourLogs.Where(t => t.TourId == tourId).ToList();
        }

        public async Task<TourLog?> GetByIdAsync(int tourLogId)
        {
            return _tourLogs.FirstOrDefault(t => t.Id == tourLogId);
        }

        public async Task AddAsync(TourLog tourLog)
        {
            _tourLogs.Add(tourLog);
        }

        public async Task UpdateAsync(TourLog tourLog)
        {
            TourLog updateTourLog =
                await GetByIdAsync(tourLog.Id) ?? throw new KeyNotFoundException();

            updateTourLog.DateAndTime = tourLog.DateAndTime;
            updateTourLog.Comment = tourLog.Comment;
            updateTourLog.Difficulty = tourLog.Difficulty;
            updateTourLog.TotalDistance = tourLog.TotalDistance;
            updateTourLog.TotalTime = tourLog.TotalTime;
            updateTourLog.Rating = tourLog.Rating;
        }

        public async Task DeleteAsync(int tourLogId)
        {
            TourLog? tourLog = await GetByIdAsync(tourLogId);
            if (tourLog != null)
            {
                _tourLogs.Remove(tourLog);
                return;
            }
            throw new KeyNotFoundException($"TourLog with id {tourLogId} not found.");
        }
    }
}
