using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(await _tourService.GetAllAsync(username));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTour(int id)
        {
            var username = User.Identity!.Name!;
            var tour = await _tourService.GetByIdAsync(username, id);
            return (tour == null) ? NotFound() : Ok(tour);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] Tour tour)
        {
            var username = User.Identity!.Name!;
            await _tourService.CreateTourAsync(username, tour);
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

        // SEARCH ENDPOINT
        [HttpGet("search")]
        public async Task<IActionResult> SearchTours([FromQuery] string? searchTerm)
        {
            var username = User.Identity!.Name!;
            // TODO: Remove try-catch block once search functionality is implemented
            try
            {
                var tours = await _tourService.SearchToursAsync(username, searchTerm);
                return Ok(tours);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, "Search functionality is not implemented yet.");
            }
        }
    }
}
