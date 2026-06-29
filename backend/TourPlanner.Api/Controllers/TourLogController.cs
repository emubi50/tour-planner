using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Bll.Interfaces;

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
    }
}
