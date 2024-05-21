using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class SeriesServiceDbContext : DbContext
    {
        public SeriesServiceDbContext(DbContextOptions<SeriesServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Series> Series { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SeriesPart> SeriesParts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresSeries> GenresSeries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoriesSeries> CategoriesSeries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
