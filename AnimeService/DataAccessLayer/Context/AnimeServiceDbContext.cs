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
        public DbSet<Rating> AnimeRating { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CommentDislike> CommentDislikes { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Voiceover> Voiceovers{ get; set; }
        public DbSet<StudiosAnime> StudiosAnimes { get; set; }
        public DbSet<VoiceoversAnime> VoiceoversAnimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {        
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
