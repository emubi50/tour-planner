using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Models;
using TourPlanner.Models.Enums;

namespace TourPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IOpenRouteService _openRouteService;

        public RouteController(IOpenRouteService openRouteService)
        {
            _openRouteService = openRouteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoute(
            [FromQuery] double[] startLocCords,
            [FromQuery] double[] destLocCords,
            [FromQuery] TransportType transportType
        )
        {
            var route = await _openRouteService.GetRoute(
                startLocCords,
                destLocCords,
                transportType
            );
            return Ok(route);
        }
    }
}
