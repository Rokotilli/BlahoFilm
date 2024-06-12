using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MessageBus.Outbox;

namespace DataAccessLayer.Context
{
    public class AnimeServiceDbContext : BaseDbContext
    {
        public AnimeServiceDbContext(DbContextOptions<AnimeServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Anime> Animes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AnimePart> AnimeParts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresAnime> GenresAnimes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoriesAnime> CategoriesAnimes { get; set; }
        public DbSet<Rating> AnimeRating { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudiosAnime> StudiosAnimes { get; set; }
        public DbSet<Selection> Selections { get; set; }
        public DbSet<SelectionAnime> SelectionAnimes { get; set; }
        protected override Assembly ConfigurationAssembly => Assembly.GetExecutingAssembly();
    }
}
