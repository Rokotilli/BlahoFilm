using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class CartoonServiceDbContext : DbContext
    {
        public CartoonServiceDbContext(DbContextOptions<CartoonServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Cartoon> Cartoons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartoonPart> CartoonParts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresCartoon> GenresCartoons { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagsCartoon> TagsCartoons { get; set; }
        public DbSet<AnimationType> AnimationTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rating> CartoonRating { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Voiceover> Voiceovers { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<VoiceoversCartoon> VoiceoversCartoons { get; set; }
        public DbSet<StudiosCartoon> StudiosCartoons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);

        }
    }
}
