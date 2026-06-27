using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface ITourRepository
    {
        Task<List<Tour>> GetAllAsync();
        Task<Tour?> GetByIdAsync(int userId, int tourId);
        Task AddAsync(int userId, Tour tour);
        Task UpdateAsync(int userId, Tour tour);
        Task DeleteAsync(int userId, int tourId);
    }
}
