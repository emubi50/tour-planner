using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    internal interface IOpenRouteService
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
