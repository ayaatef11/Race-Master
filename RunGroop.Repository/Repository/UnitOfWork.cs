using Microsoft.AspNetCore.Http;
using RunGroop.Data.Data;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.Repository;

namespace RunGroop.Repository.Repository
{
    public class UnitOfWork(ApplicationDbContext _context, IHttpContextAccessor? accessor) : IUnitOfWork
    {
        public ClubRepository ClubRepository { get; } = new ClubRepository(_context);

        public DashboardRepository DashboardRepository { get; } = new DashboardRepository(_context, accessor);

        public RaceRepository RaceRepository { get; } = new RaceRepository(_context);

        public UserRepository UserRepository { get; } = new UserRepository(_context);
    }
}
