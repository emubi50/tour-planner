using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface ITourLogRepository
    {
        Task<List<TourLog>> GetAllByTourIdAsync(int tourId);
        Task<TourLog?> GetByIdAsync(int tourLogId);
        Task AddAsync(TourLog tourLog);
        Task UpdateAsync(TourLog tourLog);
        Task DeleteAsync(int tourLogId);
    }
}
