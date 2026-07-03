using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Api.Dtos;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/tours")]
    [Authorize]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly IOpenRouteService _openRouteService;

        public TourController(ITourService tourService, IOpenRouteService openRouteService)
        {
            _tourService = tourService;
            _openRouteService = openRouteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTours()
        {
            var username = User.Identity!.Name!;
            var tours = await _tourService.GetAllAsync(username);
            var tourDtos = tours.Select(t => new TourResponseDto(t)).ToList();
            return Ok(tourDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTour(int id)
        {
            var username = User.Identity!.Name!;
            var tour = await _tourService.GetByIdAsync(username, id);
            return (tour == null) ? NotFound() : Ok(new TourResponseDto(tour));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] TourCreateDto tourCreateDto)
        {
            var username = User.Identity!.Name!;
            Tour tourData = tourCreateDto.toTour();

            // Get duration and distance from OpenRouteService
            Models.Route routeInfo = await _openRouteService.GetRoute(
                tourData.From.Coordinates,
                tourData.To.Coordinates,
                tourData.TransportType
            );

            tourData.Distance = routeInfo.Distance;
            tourData.EstimatedTime = routeInfo.Duration;

            tourData.RouteInformation = routeInfo.Path.Aggregate(
                "",
                (acc, point) => acc + $"{point[0]},{point[1]};"
            );

            await _tourService.CreateTourAsync(username, tourData);
            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] TourCreateDto newTourDto)
        {
            var username = User.Identity!.Name!;

            // Get old tour to see if any of the locations have changed
            Tour? oldTour = await _tourService.GetByIdAsync(username, id);
            if (oldTour == null)
            {
                return NotFound();
            }
            
            var newTour = newTourDto.toTour();
            newTour.Id = id;

            bool needsRouteUpdate = (
                oldTour.From != newTour.From
                || oldTour.To != newTour.To
                || oldTour.TransportType != newTour.TransportType
            );

            
            if (needsRouteUpdate)
            {
                // Get duration and distance from OpenRouteService
                var route = await _openRouteService.GetRoute(
                    newTourDto.From.Coordinates,
                    newTourDto.To.Coordinates,
                    newTourDto.TransportType
                );

                newTour.Distance = route.Distance;
                newTour.EstimatedTime = route.Duration;
                newTour.RouteInformation = route.Path.Aggregate(
                    "",
                    (acc, point) => acc + $"{point[0]},{point[1]};"
                );
            }
            else
            {
                newTour.Distance = oldTour.Distance;
                newTour.EstimatedTime = oldTour.EstimatedTime;
                newTour.RouteInformation = oldTour.RouteInformation;
            }

            var updated = await _tourService.UpdateTourAsync(username, newTour);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var username = User.Identity!.Name!;
            var deleted = await _tourService.DeleteTourAsync(username, id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTours([FromQuery] string? searchTerm)
        {
            var username = User.Identity!.Name!;
            var tours = await _tourService.SearchToursAsync(username, searchTerm);
            var results = tours.Select(t => new TourResponseDto(t)).ToList();
            return Ok(results);
        }
    }
}
