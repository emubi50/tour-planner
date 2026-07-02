using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Models;
using TourPlanner.Models.ORS.Direction;
using TourPlanner.Models.ORS.Geocode;

namespace TourPlanner.Bll.Services
{
    public class OpenRouteService : IOpenRouteService
    {
        private readonly HttpClient _httpClient;

        private const int searchSize = 20;
        private const int autocompleteSize = 10;

        public OpenRouteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LocationSearchResult> SearchDestinations(string query)
        {
            var uri = $"geocode/search?text={Uri.EscapeDataString(query)}&size={searchSize}";
            return await FetchSearchResults(uri);
        }

        public async Task<LocationSearchResult> AutocompleteDestinations(string query)
        {
            var uri =
                $"geocode/autocomplete?text={Uri.EscapeDataString(query)}&size={autocompleteSize}";
            return await FetchSearchResults(uri);
        }

        public async Task<Route> GetRoute(
            double[] startLocCords,
            double[] destLocCords,
            TransportType transportType
        )
        {
            var transportTypeString = transportType switch
            {
                TransportType.CAR => "driving-car",
                TransportType.BIKE => "cycling-regular",
                TransportType.WALK => "foot-walking",
                TransportType.PUBLIC => "driving-car", // OpenRouteService does not support public transport, so we use driving-car as a fallback
                _ => throw new ArgumentOutOfRangeException(
                    nameof(transportType),
                    transportType,
                    null
                ),
            };

            var uri =
                $"v2/directions/{transportTypeString}?start={startLocCords[0]},{startLocCords[1]}&end={destLocCords[0]},{destLocCords[1]}";

            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            DirectionResult? result =
                JsonSerializer.Deserialize<DirectionResult>(json)
                ?? throw new InvalidOperationException("Failed to deserialize route response.");

            return new Route
            {
                Distance = result.Features[0].Properties.Summary.Distance,
                Duration = result.Features[0].Properties.Summary.Duration,
                Path = result.Features[0].Geometry.Coordinates,
            };
        }

        private async Task<LocationSearchResult> FetchSearchResults(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            GeocodeResponse? result =
                JsonSerializer.Deserialize<GeocodeResponse>(json)
                ?? throw new InvalidOperationException("Failed to deserialize geocode response.");

            return MapToLocationSearchResult(result);
        }

        private LocationSearchResult MapToLocationSearchResult(GeocodeResponse result)
        {
            var locations = new List<Location>();
            foreach (var feature in result.Features)
            {
                locations.Add(
                    new Location
                    {
                        Coordinates = feature.Geometry.Coordinates,
                        Label = feature.Properties.Label,
                        Confidence = feature.Properties.Confidence,
                    }
                );
            }

            return new LocationSearchResult
            {
                Timestamp = result.Geocoding.Timestamp,
                Locations = locations,
            };
        }
    }
}
