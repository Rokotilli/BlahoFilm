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
        public DbSet<CartoonRating> CartoonRating { get; set; }
        public DbSet<CartoonPartRating> CartoonPartRating { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cartoon>().HasMany(c => c.CartoonRatings).WithOne(c => c.Cartoon).HasForeignKey(cr => cr.CartoonId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartoonPart>().HasMany(c => c.CartoonPartRatings).WithOne(c => c.CartoonPart).HasForeignKey(cr => cr.CartoonPartId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CommentDislike>().HasOne(c => c.User).WithMany(c => c.CommentDislikes).HasForeignKey(cr => cr.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<CommentLike>().HasOne(c => c.User).WithMany(c => c.CommentLikes).HasForeignKey(cr => cr.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Comment>().HasMany(c => c.CommentDislikes).WithOne(c => c.Comment).HasForeignKey(cr => cr.CommentId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>().HasMany(c => c.CommentLikes).WithOne(c => c.Comment).HasForeignKey(cr => cr.CommentId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
