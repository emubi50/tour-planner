using TourPlanner.Models;

namespace TourPlanner.Dal.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task InsertUserAsync(User user);
    }
}
