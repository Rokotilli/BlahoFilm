using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class StudiosAnimeConfiguration : IEntityTypeConfiguration<StudiosAnime>
    {
        public void Configure(EntityTypeBuilder<StudiosAnime> builder)
        {
            builder.HasKey(tf => new { tf.AnimeId, tf.StudioId });
        }
    }
}