using TourPlanner.Bll.Interfaces;
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
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    throw new Exception();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("No user found", ex);
            }
        }

        public async Task RegisterUserAsync(string username, string hashedPassword)
        {
            await _userRepository.InsertUserAsync(new User
            {
                Username = username,
                HashedPassword = hashedPassword
            });
        }
    }
}
