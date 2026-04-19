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

        public IEnumerable<TourLog> GetAllTourLogs()
        {
            return _tourLogs.ToArray() ?? Enumerable.Empty<TourLog>();
        }

        public TourLog? GetTourLogById(int tourLogId)
        {
            return GetAllTourLogs().FirstOrDefault(t => t.Id == tourLogId);
        }

        public void InsertTourLog(TourLog tourLog)
        {
            _tourLogs.Add(tourLog);
        }

        public void UpdateTourLog(TourLog tourLog)
        {
            TourLog updateTourLog = GetTourLogById(tourLog.Id) ?? throw new KeyNotFoundException();

            updateTourLog.DateAndTime = tourLog.DateAndTime;
            updateTourLog.Comment = tourLog.Comment;
            updateTourLog.Difficulty = tourLog.Difficulty;
            updateTourLog.TotalDistance = tourLog.TotalDistance;
            updateTourLog.TotalTime = tourLog.TotalTime;
            updateTourLog.Rating = tourLog.Rating;
        }

        public bool DeleteTourLog(int tourLogId)
        {
            bool found = false;

            TourLog? tourLog = GetAllTourLogs().FirstOrDefault(t => t.Id == tourLogId);

            if (tourLog != null)
            {
                found = true;
                _tourLogs.Remove(tourLog);
            }

            return found;
        }
    }
}
