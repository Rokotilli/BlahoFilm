using DataAccessLayer.Entities;
using MessageBus.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccessLayer.Context
{
    public class CartoonServiceDbContext : BaseDbContext
    {
        public CartoonServiceDbContext(DbContextOptions<CartoonServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Cartoon> Cartoons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartoonPart> CartoonParts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresCartoon> GenresCartoons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoriesCartoon> CategoriesCartoons { get; set; }
        public DbSet<AnimationType> AnimationTypes { get; set; }
        public DbSet<AnimationTypesCartoon> AnimationTypesCartoons { get; set; }
        public DbSet<Rating> CartoonRating { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudiosCartoon> StudiosCartoons { get; set; }
        public DbSet<Selection> Selections { get; set; }
        public DbSet<SelectionCartoon> SelectionCartoons { get; set; }
        protected override Assembly ConfigurationAssembly => Assembly.GetExecutingAssembly();
    }
}
