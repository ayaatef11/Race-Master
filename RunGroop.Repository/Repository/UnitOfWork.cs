using Microsoft.AspNetCore.Http;
using RunGroop.Data.Data;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Repository.Repository
{
    public class UnitOfWork: IUnitOfWork//create one instance along the program
    {
        public ClubRepository ClubRepository{ get; }

        public DashboardRepository DashboardRepository { get; }

        public RaceRepository RaceRepository { get; }

        public UserRepository UserRepository { get; }
        public UnitOfWork(ApplicationDbContext _context,IHttpContextAccessor?accessor)
        {
            ClubRepository = new ClubRepository(_context);
            DashboardRepository=new DashboardRepository  (_context,accessor);
            RaceRepository=new RaceRepository(_context); 
            UserRepository = new UserRepository(_context);
            
        }
    }
}
