
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class VoiceoversAnimeConfiguration : IEntityTypeConfiguration<VoiceoversAnime>
    {
        public void Configure(EntityTypeBuilder<VoiceoversAnime> builder)
        {
            builder.HasKey(tf => new { tf.AnimeId, tf.VoiceoverId });
        }
    }
}
