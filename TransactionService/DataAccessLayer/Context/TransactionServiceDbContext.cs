using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class TransactionServiceDbContext : DbContext
    {
        public TransactionServiceDbContext(DbContextOptions<TransactionServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Fundraising> Fundraisings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
