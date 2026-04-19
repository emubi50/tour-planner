using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface ITourRepository
    {
        IEnumerable<Tour> GetAllTours();
        Tour? GetTourById(int tourId);
        void InsertTour(Tour tour);
        void UpdateTour(Tour tour);
        bool DeleteTour(int tourId);
    }
}
