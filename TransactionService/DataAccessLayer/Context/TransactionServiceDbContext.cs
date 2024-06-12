using DataAccessLayer.Entities;
using MessageBus.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccessLayer.Context
{
    public class TransactionServiceDbContext : BaseDbContext
    {
        public TransactionServiceDbContext(DbContextOptions<TransactionServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Fundraising> Fundraisings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override Assembly ConfigurationAssembly => Assembly.GetExecutingAssembly();
    }
}
