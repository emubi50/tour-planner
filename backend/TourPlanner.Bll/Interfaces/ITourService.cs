using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface ITourService
    {
        Task<List<Tour>> GetAllAsync();

        Task<Tour?> GetByIdAsync(int userId, int tourId);

        Task CreateTourAsync(int userId, Tour tour);

        Task DeleteTourAsync(int userId, int tourId);
    }
}
