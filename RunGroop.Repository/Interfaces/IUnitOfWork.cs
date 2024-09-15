using RunGroop.Data.Interfaces.Repositories;
using RunGroopWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
