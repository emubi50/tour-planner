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
            var username = User.Identity!.Name!; // TODO: filter tours by user
            return Ok(await _tourService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTour(int id)
        {
            var username = User.Identity!.Name!; // TODO: check that tour belongs to user
            var tour = await _tourService.GetByIdAsync(id);
            if (tour == null)
                return NotFound();
            return Ok(tour);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] Tour tour)
        {
            var username = User.Identity!.Name!; // TODO: assign tour to the user
            await _tourService.CreateTourAsync(tour);
            return Created();
        }
    }
}
