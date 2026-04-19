using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface ITourLogRepository
    {
        IEnumerable<TourLog> GetAllTourLogs();
        TourLog? GetTourLogById(int tourLogId);
        void InsertTourLog(TourLog tourLog);
        void UpdateTourLog(TourLog tourLog);
        bool DeleteTourLog(int tourLogId);
    }
}
