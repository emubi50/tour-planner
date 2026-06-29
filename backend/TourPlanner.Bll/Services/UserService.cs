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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
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
            }
            catch (DuplicateKeyException ex)
            {
                throw new UserAlreadyExistsException(
                    $"User with username '{username}' already exists",
                    ex
                );
            }
        }
    }
}
