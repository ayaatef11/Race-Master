using RunGroop.Data.Models.Data;

namespace RunGroop.Data.Interfaces.Services
{
    public interface ILocationService
    {
        Task<List<City>> GetLocationSearch(string location);
        Task<City> GetCityByZipCode(int zipCode);
    }
}
