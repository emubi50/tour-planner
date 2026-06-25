using Microsoft.EntityFrameworkCore;
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

        // TODO: Add Users Table to DB
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task InsertUserAsync(User user)
        {
            //! this WILL not work because User is not a table in DB.
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
