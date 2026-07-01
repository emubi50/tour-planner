using Microsoft.Extensions.Logging;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TourService> _logger;

        public TourService(
            ITourRepository tourRepository,
            IUserRepository userRepository,
            ILogger<TourService> logger
        )
        {
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<Tour>> GetAllAsync(string username)
        {
            int userId = await GetUserIdAsync(username);
            return await _tourRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Tour?> GetByIdAsync(string username, int tourId)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourId);

            if (tour == null)
            {
                _logger.LogDebug("Tour {TourId} does not exist", tourId);
                return null;
            }

            if (tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to access tour {TourId} owned by a different user",
                    username,
                    tourId
                );
                return null;
            }

            return tour;
        }

        public async Task CreateTourAsync(string username, Tour tour)
        {
            int userId = await GetUserIdAsync(username);
            tour.UserId = userId;
            await _tourRepository.AddAsync(tour);
            _logger.LogInformation(
                "User {Username} created tour {TourId} ({TourName})",
                username,
                tour.Id,
                tour.Name
            );
        }

        public async Task<bool> UpdateTourAsync(string username, Tour tour)
        {
            int userId = await GetUserIdAsync(username);
            var existingTour = await _tourRepository.GetByIdAsync(tour.Id);

            if (existingTour == null || existingTour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to update tour {TourId} that does not exist or is not theirs",
                    username,
                    tour.Id
                );
                return false;
            }

            tour.UserId = existingTour.UserId;

            try
            {
                await _tourRepository.UpdateAsync(tour);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning(
                    "Tour {TourId} was deleted concurrently while user {Username}was updating it",
                    tour.Id,
                    username
                );
                return false;
            }
            _logger.LogInformation(
                "User {Username} updated tour {TourId}",
                username,
                tour.Id
            );
            return true;
        }

        public async Task<bool> DeleteTourAsync(string username, int tourId)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourId);
            if (tour == null || tour.UserId != userId)
            {
                _logger.LogWarning(
                    "User {Username} attempted to delete tour {TourId} that does not exist or is not theirs",
                    username,
                    tourId
                );
                return false;
            }

            try
            {
                await _tourRepository.DeleteAsync(tourId);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning(
                    "Tour {TourId} was already deleted concurrently when user {Username} attempted to delete it",
                    tourId,
                    username
                );
                return false;
            }
            _logger.LogInformation("User {Username} deleted tour {TourId}", username, tourId);
            return true;
        }

        private async Task<int> GetUserIdAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning(
                    "Authenticated request for username {Username} but no matching user record exists",
                    username
                );
                throw new UserNotFoundException($"User with username '{username}' not found.");
            }
            return user.Id;
        }
    }
}
