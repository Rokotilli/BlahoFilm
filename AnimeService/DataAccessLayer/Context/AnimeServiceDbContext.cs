using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class AnimeServiceDbContext : DbContext
    {
        public AnimeServiceDbContext(DbContextOptions<AnimeServiceDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Anime> Animes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AnimePart> AnimeParts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresAnime> GenresAnimes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagsAnime> TagsAnimes { get; set; }
        public DbSet<AnimeRating> AnimeRating { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {        
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Anime>().HasMany(a => a.AnimeRatings).WithOne(a => a.Anime).HasForeignKey(cr => cr.AnimeId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CommentDislike>().HasOne(a => a.User).WithMany(a => a.CommentDislikes).HasForeignKey(cr => cr.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<CommentLike>().HasOne(a => a.User).WithMany(a => a.CommentLikes).HasForeignKey(cr => cr.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Comment>().HasMany(a => a.CommentDislikes).WithOne(a => a.Comment).HasForeignKey(cr => cr.CommentId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>().HasMany(a => a.CommentLikes).WithOne(a => a.Comment).HasForeignKey(cr => cr.CommentId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
