using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Repository.Repository;
using RunGroopWebApp.Repository;

namespace RunGroop.Repository.Interfaces
{
    public  interface IUnitOfWork
    {
        ClubRepository ClubRepository { get; }
        DashboardRepository DashboardRepository { get; }
        RaceRepository  RaceRepository { get; }
        UserRepository UserRepository { get; }
    }
}
