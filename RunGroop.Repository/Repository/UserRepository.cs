
using RunGroop.Data.Data;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Identity;

namespace RunGroop.Repository.Repository
{
    public class UserRepository(ApplicationDbContext _context) : ProgramRepository<AppUser>(_context), IUserRepository
    {
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}