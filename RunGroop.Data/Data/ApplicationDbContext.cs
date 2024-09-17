using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Contracts;
using RunGroop.Data.Models.Data;
using RunGroop.Data.Models.Identity;
using RunGroop.Data.Models.SignalR;

namespace RunGroopWebApp.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser>(options)
    {
        public string TenantId { get; set; }
        private readonly ITenantService _TenantService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService cc)
        {
            _TenantService = cc;
            TenantId = cc.getCurrentTenant()?.TId;
        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Notification>Notifications { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
            //nameof is faster than the string 
            builder.Entity<Race>(e=>e.ToTable(nameof(Race)));
            builder.Entity<Club>().HasQueryFilter(e => e.TenantId == TenantId);
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var tenantConnectionString = _TenantService.GetConnectionString();

            if (!string.IsNullOrEmpty(tenantConnectionString))
            {

                var dbProvider = _TenantService.GetDatabaseProvider();

                if (dbProvider?.ToLower() == "mssql")

                    optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
        {
            foreach(var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantId = TenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
	}
}
