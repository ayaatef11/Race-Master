 using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Models.SignalR;

namespace RunGroop.Infrastructure
{
    public class ChatContext : DbContext
    {
        public ChatContext()
        {
        }

        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Messagel).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }

}    
