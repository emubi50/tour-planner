using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface ITourLogService
    {
        Task<List<TourLog>> GetAllAsync(string username, int tourId);
        Task<TourLog?> GetByIdAsync(string username, int tourId, int tourLogId);
        Task CreateTourLogAsync(string username, TourLog tourLog);
        Task<bool> UpdateTourLogAsync(string username, TourLog newtourLog);
        Task<bool> DeleteTourLogAsync(string username, int tourId, int tourLogId);
    }
}
