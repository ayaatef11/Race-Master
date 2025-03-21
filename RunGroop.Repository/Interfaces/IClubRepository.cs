﻿using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;
namespace RunGroop.Repository.Interfaces
{
    public interface IClubRepository
    {

        Task<IEnumerable<Club>> GetSliceAsync(int offset, int size);

        Task<IEnumerable<Club>> GetClubsByState(string state);

        Task<IEnumerable<Club>> GetClubsByCategoryAndSliceAsync(ClubCategory category, int offset, int size);

        Task<List<State>> GetAllStates();

        Task<List<City>> GetAllCitiesByState(string state);

        Task<Club?> GetByIdAsync(int id);

        Task<Club?> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<Club>> GetClubByCity(string city);

        Task<int> GetCountAsync();

        Task<int> GetCountByCategoryAsync(ClubCategory category);

        public Task EventOccured(Club club, string ev);
    }
}