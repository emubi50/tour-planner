using System.ComponentModel.DataAnnotations;
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

        public TourService(ITourRepository tourRepository, IUserRepository userRepository)
        {
            _tourRepository = tourRepository;
            _userRepository = userRepository;
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
            return (tour != null && tour.UserId == userId) ? tour : null; // If tour does not belong to user, user does not need to know that tour exists --> pretend there is no tour
        }

        public async Task CreateTourAsync(string username, Tour tour)
        {
            int userId = await GetUserIdAsync(username);
            tour.UserId = userId;
            await _tourRepository.AddAsync(tour);
        }

        public async Task UpdateTourAsync(string username, Tour incomingTour)
        {
            int userId = await GetUserIdAsync(username);
            var existingTour = await _tourRepository.GetByIdAsync(incomingTour.Id);
            if (existingTour != null && existingTour.UserId == userId)
            {
                incomingTour.UserId = existingTour.UserId;
                await _tourRepository.UpdateAsync(incomingTour);
            }
        }

        public async Task DeleteTourAsync(string username, int tourId)
        {
            int userId = await GetUserIdAsync(username);
            var tour = await _tourRepository.GetByIdAsync(tourId);
            if (tour != null && tour.UserId == userId)
            {
                await _tourRepository.DeleteAsync(tourId);
            }
        }

        private async Task<int> GetUserIdAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException($"User with username '{username}' not found.");
            }
            return user.Id;
        }
    }
}
