using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroop.Data.Models.Identity;
using RunGroopWebApp.Data;
using RunGroopWebApp.Extensions;
namespace RunGroopWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

#pragma warning disable CS8603
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (curUser == null)
            {
                return new List<Club>(); // or throw an exception
            }

            var userClubs = await _context.Clubs
                .Where(r => r.AppUser != null && r.AppUser.Id == curUser)
                .ToListAsync();

            return userClubs;
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (curUser == null)
            {
                return new List<Race>(); // or throw an exception
            }

            var userRaces = await _context.Races
                .Where(r => r.AppUser != null && r.AppUser.Id == curUser)
                .ToListAsync();

            return userRaces;
        }

        public async Task<AppUser> GetUserById(string id)
        {
                return await _context.Users.FindAsync(id!);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();

        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
