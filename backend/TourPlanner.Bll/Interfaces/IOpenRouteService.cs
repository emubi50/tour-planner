using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;
using TourPlanner.Models.Enums;

namespace TourPlanner.Bll.Interfaces
{
    public interface IOpenRouteService
    {
        Task<LocationSearchResult> SearchDestinations(string query);
        Task<LocationSearchResult> AutocompleteDestinations(string query);
        Task<Route> GetRoute(
            double[] startLocCords,
            double[] destLocCords,
            TransportType transportType
        );
    }
}
