using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Data;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroop.Data.Models.Identity;
using RunGroop.Repository.Repository;

namespace RunGroopWebApp.Repository
{
    public class UserRepository(ApplicationDbContext _context) : ProgramRepository<AppUser>(_context), IUserRepository
    {

/// <summary>
/// app user must inherit from entity so it must be a class and also entity 
/// </summary>
/// <param name="id"></param>
/// <returns></returns>

#pragma warning disable CS8603
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}