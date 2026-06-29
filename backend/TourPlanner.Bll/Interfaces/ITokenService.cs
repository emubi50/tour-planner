using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
