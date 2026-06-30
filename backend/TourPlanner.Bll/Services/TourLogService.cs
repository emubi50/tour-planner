using Microsoft.Extensions.Logging;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Dal.Interfaces;

namespace TourPlanner.Bll.Services
{
    public class TourLogService : ITourLogService
    {
        private readonly ITourLogRepository _tourLogRepository;
        private readonly ILogger<TourLogService> _logger;

        public TourLogService(ITourLogRepository tourLogRepository, ILogger<TourLogService> logger)
        {
            _tourLogRepository = tourLogRepository;
            _logger = logger;
        }
    }
}
