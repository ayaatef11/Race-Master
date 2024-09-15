using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Repository;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;

namespace RunGroopWebApp.Repository
{
    public class ClubRepository(ApplicationDbContext _context) :ProgramRepository<Club>(_context), IClubRepository
    {
       public async Task<Club?> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<State>> GetAllStates()
        {
            return await _context.States.ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetSliceAsync(int offset, int size)
        {
            return await _context.Clubs.Include(i => i.Address).Skip(offset).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubsByCategoryAndSliceAsync(ClubCategory category, int offset, int size)
        {
            return await _context.Clubs
                .Include(i => i.Address)
                .Where(c => c.ClubCategory == category)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetCountByCategoryAsync(ClubCategory category)
        {
            return await _context.Clubs.CountAsync(c => c.ClubCategory == category);
        }

     

        public async Task<Club?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).Distinct().ToListAsync();
        }

        

        public async Task<int> GetCountAsync()
        {
            return await _context.Clubs.CountAsync();
        }

        public async Task<IEnumerable<Club>> GetClubsByState(string state)
        {
            return await _context.Clubs.Where(c => c.Address.State.Contains(state)).ToListAsync();
        }

        public async Task<List<City>> GetAllCitiesByState(string state)
        {
            return await _context.Cities.Where(c => c.StateCode.Contains(state)).ToListAsync();
        }
        /// <summary>
        /// //////////////
        /// </summary>
        /// <param name="club"></param>
        /// <param name="ev"></param>
        /// <returns></returns>
        public async Task EventOccured(Club club, string ev)
        {

            _context.Clubs.Single(p => p.Id == club.Id).Title = $"{club.Title} evt: {ev}";

            await Task.CompletedTask;
        }
    }
}