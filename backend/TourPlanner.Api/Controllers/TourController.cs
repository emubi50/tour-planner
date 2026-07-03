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

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
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
            var tourDto = (tour == null) ? null : new TourResponseDto(tour);
            return (tour == null) ? NotFound() : Ok(tourDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] TourCreateDto tourCreateDto)
        {
            var username = User.Identity!.Name!;
            await _tourService.CreateTourAsync(username, tourCreateDto.toTour());
            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] Tour tour)
        {
            var username = User.Identity!.Name!;
            tour.Id = id;
            var updated = await _tourService.UpdateTourAsync(username, tour);
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
            var results = tours.Select(t => new TourSearchResultDto(t)).ToList();
            return Ok(results);
        }
    }
}
