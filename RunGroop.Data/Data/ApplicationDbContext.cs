﻿
using RunGroop.Data.Contracts;
using RunGroop.Data.Models.Data;
using RunGroop.Data.Models.Entities;
using RunGroop.Data.Models.Identity;
using RunGroop.Data.Models.SignalR;
using RunGroop.Data.Services;
using RunGroop.Data.Settings;

namespace RunGroop.Data.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        //private readonly ITenantService _TenantService;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //public ApplicationDbContext(DbContextOptions options, ITenantService tenantService, IHttpContextAccessor httpContextAccessor)
        //    : base(options)
        //{
        //    _TenantService = tenantService;
        //    _httpContextAccessor = httpContextAccessor;
        //    TenantId = _TenantService.GetCurrentTenant()?.TId;
        //}

        public string TenantId { get; set; }//;// = _tenantService.GetCurrentTenant()?.TId;

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationPreferences> NotificationPreferences { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Race>(e => e.ToTable(nameof(Race)));
            builder.Entity<Club>().HasQueryFilter(e => e.TenantId == TenantId);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    var tenantConnectionString = _TenantService.GetConnectionString();

        //    if (!string.IsNullOrEmpty(tenantConnectionString))
        //    {

        //        var dbProvider = _TenantService.GetDatabaseProvider();

        //        if (dbProvider?.ToLower() == "mssql")

        //            optionsBuilder.UseSqlServer(tenantConnectionString);
        //    }
        //}
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            //{
            //    entry.Entity.TenantId = TenantId;
            //}
            //// return base.SaveChangesAsync(cancellationToken);
            //var modifiedEntities = ChangeTracker.Entries().
            //             Where(e => e.State == EntityState.Added
            //             || e.State == EntityState.Modified
            //             || e.State == EntityState.Deleted).
            //                 ToList();

            //foreach (var modifiedEntity in modifiedEntities)
            //{

            //    var auditLog = new AuditLog
            //    {

            //        EntityName = modifiedEntity.Entity.GetType().Name,
            //        UserEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name),
            //        Action = modifiedEntity.State.ToString(),
            //        Timestamp = DateTime.UtcNow,
            //        Changes = GetChanges(modifiedEntity)
            //    };
            //    AuditLogs.Add(auditLog);

            //}
            return base.SaveChangesAsync(cancellationToken);
        }

        private string GetChanges(EntityEntry modifiedEntity)
        {
            var changes = new StringBuilder();

            foreach (var property in modifiedEntity.OriginalValues.Properties)
            {

                var originalValue = modifiedEntity.OriginalValues[property];
                var currentValue = modifiedEntity.CurrentValues[property];

                if (!Equals(originalValue, currentValue))

                    changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
            }
            return changes.ToString();

        }
    }
}
