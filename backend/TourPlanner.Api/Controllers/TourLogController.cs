using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Api.Controllers
{
    [Route("api/tours/{tourId:int}/logs")]
    [ApiController]
    [Authorize]
    public class TourLogController : ControllerBase
    {
        private ITourLogService _tourLogService;

        public TourLogController(ITourLogService tourLogService)
        {
            _tourLogService = tourLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTourLogs(int tourId)
        {
            var username = User.Identity!.Name!;
            return Ok(await _tourLogService.GetAllAsync(username, tourId));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTourLog(int tourId, int id)
        {
            var username = User.Identity!.Name!;
            var tourLog = await _tourLogService.GetByIdAsync(username, tourId, id);
            return (tourLog == null) ? NotFound() : Ok(tourLog);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTourLog(int tourId, [FromBody] TourLog tourLog)
        {
            var username = User.Identity!.Name!;
            await _tourLogService.CreateTourLogAsync(username, tourLog);
            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTourLog(int tourId, int id, [FromBody] TourLog tourLog)
        {
            var username = User.Identity!.Name!;
            tourLog.Id = id;
            tourLog.TourId = tourId;
            var updated = await _tourLogService.UpdateTourLogAsync(username, tourLog);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTourLog(int tourId, int id)
        {
            var username = User.Identity!.Name!;
            var deleted = await _tourLogService.DeleteTourLogAsync(username, tourId, id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
