using Microsoft.Extensions.Logging;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Dal.Exceptions;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogDebug("Lookup failed: no user found with username {Username}", username);
                throw new UserNotFoundException($"User with username '{username}' not found.");
            }
            return user;
        }

        public async Task RegisterUserAsync(string username, string hashedPassword)
        {
            try
            {
                await _userRepository.InsertUserAsync(
                    new User { Username = username, HashedPassword = hashedPassword }
                );
                _logger.LogInformation("New user registered: {Username}", username);
            }
            catch (DuplicateKeyException ex)
            {
                _logger.LogWarning(
                    "Registration attempt with already-existing username {Username}",
                    username
                );
                throw new UserAlreadyExistsException(
                    $"User with username '{username}' already exists",
                    ex
                );
            }
        }
    }
}
