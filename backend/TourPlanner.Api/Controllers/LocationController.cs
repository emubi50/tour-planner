using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Bll.Interfaces;

namespace TourPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IOpenRouteService _openRouteService;

        public LocationController(IOpenRouteService openRouteService)
        {
            _openRouteService = openRouteService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchDestinations([FromQuery] string query)
        {
            var result = await _openRouteService.SearchDestinations(query);
            return Ok(result);
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutocompleteDestinations([FromQuery] string query)
        {
            var result = await _openRouteService.AutocompleteDestinations(query);
            return Ok(result);
        }
    }
}
