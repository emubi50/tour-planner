using TourPlanner.Models;

namespace TourPlanner.Api.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
