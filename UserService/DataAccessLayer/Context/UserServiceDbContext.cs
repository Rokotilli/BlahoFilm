using DataAccessLayer.Entities;
using MessageBus.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccessLayer.Context
{
    public class UserServiceDbContext : BaseDbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<MediaWithType> MediaWithTypes { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<BookMark> BookMarks { get; set; }

        protected override Assembly ConfigurationAssembly => Assembly.GetExecutingAssembly();
    }
}
