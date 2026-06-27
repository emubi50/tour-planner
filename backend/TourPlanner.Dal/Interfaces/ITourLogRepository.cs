using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface ITourLogRepository
    {
        Task<List<TourLog>> GetAllByTourIdAsync(int tourId);
        Task<TourLog?> GetByIdAsync(int tourId, int tourLogId);
        Task AddAsync(int tourId, TourLog tourLog);
        Task UpdateAsync(int tourId, TourLog tourLog);
        Task DeleteAsync(int tourId, int tourLogId);
    }
}
