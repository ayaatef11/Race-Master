using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Models.Data;


namespace RunGroopWebApp.Scraper.Data
{
    public class ScraperDBContext : DbContext
    {
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer( "Server=.;Database=RunGroopDb;TrustServerCertificate = True;Encrypt= false;Integrated Security=SSPI"
);
        }
    }
}
