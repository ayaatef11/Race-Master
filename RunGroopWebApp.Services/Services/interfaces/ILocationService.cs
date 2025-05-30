﻿using RunGroop.Data.Models.Data;

namespace RunGroopWebApp.Services.Services.interfaces
{
    public interface ILocationService
    {
        Task<List<City>> GetLocationSearch(string location);
        Task<City> GetCityByZipCode(int zipCode);
    }
}
