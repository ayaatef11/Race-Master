using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Data;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Repository;
using RunGroopWebApp.Data.Enum;

namespace RunGroopWebApp.Repository
{
    public class RaceRepository(ApplicationDbContext _context) : ProgramRepository<Race>(_context),  IRaceRepository
    {


        public async Task<Race?> GetByIdAsync(int id)
        {
            return await _context.Races.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

       

        public async Task<Race?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Races.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync();
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
            return await _context.Races.Include(a => a.Address).Skip(offset).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetRacesByCategoryAndSliceAsync(RaceCategory category, int offset, int size)
        {
            return await _context.Races
                .Where(r => r.RaceCategory == category)
                .Include(a => a.Address)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

    }
}