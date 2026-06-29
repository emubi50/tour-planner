using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface ITourRepository
    {
        Task<List<Tour>> GetAllByUserIdAsync(int userId);
        Task<Tour?> GetByIdAsync(int tourId);
        Task AddAsync(Tour tour);
        Task UpdateAsync(Tour tour);
        Task DeleteAsync(int tourId);
    }
}
