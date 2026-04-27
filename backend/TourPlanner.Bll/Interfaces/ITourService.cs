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

        Task<Tour?> GetByIdAsync(int id);

        Task CreateTourAsync(Tour tour);
    }
}
