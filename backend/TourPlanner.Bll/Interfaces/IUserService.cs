using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task RegisterUserAsync(string username, string hashedPassword);
    }
}
