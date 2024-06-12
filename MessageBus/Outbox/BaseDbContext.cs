using MessageBus.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MessageBus.Outbox
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions options) : base(options) { }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        protected abstract Assembly ConfigurationAssembly { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboxMessage>()
                .Property<string>("SerializedData")
                .IsRequired()
                .HasField("SerializedData");

            modelBuilder.Entity<OutboxMessage>()
                .Property(m => m.Type)
                .IsRequired()
                .HasConversion(m => m.AssemblyQualifiedName, m => Type.GetType(m));

            modelBuilder.ApplyConfigurationsFromAssembly(this.ConfigurationAssembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
