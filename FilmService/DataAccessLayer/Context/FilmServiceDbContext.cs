using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class FilmServiceDbContext : DbContext
    {
        public FilmServiceDbContext(DbContextOptions<FilmServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Film> Films { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresFilm> GenresFilms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoriesFilm> CategoriesFilms { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudiosFilm> StudiosFilms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
