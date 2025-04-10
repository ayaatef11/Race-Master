using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Data;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;

namespace RunGroop.Repository.Repository
{
    public class RaceRepository(ApplicationDbContext _context) : ProgramRepository<Race>(_context), IRaceRepository
    {


        public async Task<Race?> GetByIdAsync(int id)
        {
            return await _context.Races.Include(i => i.Location).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {

            return await _context.Races.Where(c => c.Location.City.Contains(city)).ToListAsync();
        }



        public async Task<Race?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Races.Include(i => i.Location).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Races.CountAsync();
        }

        public async Task<int> GetCountByCategoryAsync(RaceCategory category)
        {
            return await _context.Races.CountAsync(r => r.RaceCategory == category);
        }

        public async Task<IEnumerable<Race>> GetSliceAsync(int offset, int size)
        {
            return await _context.Races.Include(a => a.Location).Skip(offset).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetRacesByCategoryAndSliceAsync(RaceCategory category, int offset, int size)
        {
            return await _context.Races
                .Where(r => r.RaceCategory == category)
                .Include(a => a.Location)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

    }
}