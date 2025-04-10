using RunGroop.Data.Data;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;

namespace RunGroopWebApp.Services
{
    public class LocationService(ApplicationDbContext _context) : ILocationService
    {
       public async Task<City> GetCityByZipCode(int zipCode)
        {
            return await _context.Cities.FirstOrDefaultAsync(x => x.Zip == zipCode)??new City();
        }
        public async Task<List<City>> GetLocationSearch(string location)
        {
            List<City> result;
            if(location.Length > 0 && char.IsDigit(location[0]))
            {
                return await _context.Cities.Where(x => x.Zip.ToString().StartsWith(location)).Take(4).Distinct().ToListAsync();
            }
            else if (location.Length > 0)
            {
                result = await _context.Cities.Where(x => x.CityName == location).Take(10).ToListAsync();
            }
            result = await _context.Cities.Where(x => x.StateCode == location).Take(10).ToListAsync();

            return result;
        }
    }
}
