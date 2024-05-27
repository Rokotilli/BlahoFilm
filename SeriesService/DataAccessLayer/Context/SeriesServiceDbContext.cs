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
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresSeries> GenresSeries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoriesSeries> CategoriesSeries { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudiosSeries> StudiosSeries { get; set; }
        public DbSet<Selection> Selections { get; set; }
        public DbSet<SelectionSeries> SelectionSeries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
