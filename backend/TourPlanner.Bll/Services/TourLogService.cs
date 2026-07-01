using Microsoft.Extensions.Logging;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Services
{
    public class TourLogService : ITourLogService
    {
        private readonly ITourLogRepository _tourLogRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TourLogService> _logger;

        public TourLogService(
            ITourLogRepository tourLogRepository,
            ITourRepository tourRepository,
            IUserRepository userRepository,
            ILogger<TourLogService> logger
        )
        {
            _tourLogRepository = tourLogRepository;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<TourLog>> GetAllAsync(string username, int tourId)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourId);

            if (tour == null || tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to access tour logs for tour {TourId} that does not exist or is not theirs",
                    username,
                    tourId
                );
                return new List<TourLog>();
            }

            return await _tourLogRepository.GetAllByTourIdAsync(tourId);
        }

        public async Task<TourLog?> GetByIdAsync(string username, int tourId, int tourLogId)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourId);

            if (tour == null || tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to access tour log {TourLogId} for tour {TourId} that does not exist or is not theirs",
                    username,
                    tourLogId,
                    tourId
                );
                return null;
            }

            var tourLog = await _tourLogRepository.GetByIdAsync(tourLogId);

            if (tourLog == null)
            {
                _logger.LogDebug("TourLog {TourLogId} does not exist", tourLogId);
                return null;
            }

            if (tourLog.TourId != tourId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to access tour log {TourLogId} that does not belong to tour {TourId}",
                    username,
                    tourLogId,
                    tourId
                );
                return null;
            }

            return tourLog;
        }

        public async Task CreateTourLogAsync(string username, TourLog tourLog)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourLog.TourId);

            if (tour == null || tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to create a tour log for tour {TourId} that does not exist or is not theirs",
                    username,
                    tourLog.TourId
                );
                throw new TourNotFoundException("Tour with id '{tourLog.TourId}' not found. ");
            }

            await _tourLogRepository.AddAsync(tourLog);
            _logger.LogInformation(
                "User {Username} created tour log {TourLogId} for tour {TourId}",
                username,
                tourLog.Id,
                tourLog.TourId
            );
        }

        public async Task<bool> UpdateTourLogAsync(string username, TourLog tourLog)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourLog.TourId);

            if (tour == null || tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to update a tour log for tour {TourId} that does not exist or is not theirs",
                    username,
                    tourLog.TourId
                );
                return false;
            }

            try
            {
                await _tourLogRepository.UpdateAsync(tourLog);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning(
                    "TourLog {TourLogId} was deleted concurrently while user {Username} was updating it",
                    tourLog.Id,
                    username
                );
                return false;
            }

            _logger.LogInformation(
                "User {Username} updated tour log {TourLogId}",
                username,
                tourLog.Id
            );
            return true;
        }

        public async Task<bool> DeleteTourLogAsync(string username, int tourId, int tourLogId)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourId);

            if (tour == null || tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to delete a tour log for tour {TourId} that does not exist or is not theirs",
                    username,
                    tourId
                );
                return false;
            }

            try
            {
                await _tourLogRepository.DeleteAsync(tourLogId);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning(
                    "TourLog {TourLogId} was already deleted concurrently when user {Username} attempted to delete it",
                    tourLogId,
                    username
                );
                return false;
            }

            _logger.LogInformation(
                "User {Username} deleted tour log {TourLogId} for tour {TourId}",
                username,
                tourLogId,
                tourId
            );
            return true;
        }

        private async Task<int> GetUserIdAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning(
                    "User with username {Username} not found when trying to get user ID",
                    username
                );
                throw new UserNotFoundException($"User with username '{username}' not found.");
            }
            return user.Id;
        }
    }
}
