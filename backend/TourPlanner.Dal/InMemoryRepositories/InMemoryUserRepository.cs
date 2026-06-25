using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.InMemoryRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = [];
        public async  Task<User?> GetUserByUsernameAsync(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public async Task InsertUserAsync(User user)
        {
            _users.Add(user);
        }
    }
}
