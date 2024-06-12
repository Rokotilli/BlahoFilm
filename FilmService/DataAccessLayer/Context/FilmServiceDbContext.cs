using DataAccessLayer.Entities;
using MessageBus.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccessLayer.Context
{
    public class FilmServiceDbContext : BaseDbContext
    {
        public FilmServiceDbContext(DbContextOptions<FilmServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Film> Films { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresFilm> GenresFilms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoriesFilm> CategoriesFilms { get; set; }
        public DbSet<Selection> Selections { get; set; }
        public DbSet<SelectionsFilm> SelectionsFilms { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudiosFilm> StudiosFilms { get; set; }

        protected override Assembly ConfigurationAssembly => Assembly.GetExecutingAssembly();
    }
}
