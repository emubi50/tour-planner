using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Bll.Interfaces;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/tours/data")]
    [Authorize]
    public class TourDataController : ControllerBase
    {
        private readonly ITourDataTransferService _tourDataTransferService;

        public TourDataController(ITourDataTransferService tourDataTransferService)
        {
            _tourDataTransferService = tourDataTransferService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export(string format = "json")
        {
            var username = User.Identity!.Name!;
            var bytes = await _tourDataTransferService.ExportToursAsync(username, format);
            return File(bytes, "application/json", $"tours.{format}");
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var username = User.Identity!.Name!;
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var result = await _tourDataTransferService.ImportToursAsync(username, memoryStream.ToArray());
            return Ok(result);
        }
    }
}
