using Microsoft.EntityFrameworkCore;
using TourPlanner.Dal.Exceptions;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Dal.DatabaseRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TourPlannerDbContext _context;

        public UserRepository(TourPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task InsertUserAsync(User user)
        {
            try
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is ArgumentException)
            {
                throw new DuplicateKeyException(
                    $"User with username {user.Username} alreaedy exists",
                    ex
                );
            }
        }
    }
}
