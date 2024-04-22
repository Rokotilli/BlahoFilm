using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class VoiceoversFilmConfiguration : IEntityTypeConfiguration<VoiceoversFilm>
    {
        public void Configure(EntityTypeBuilder<VoiceoversFilm> builder)
        {
            builder.HasKey(tf => new { tf.FilmId, tf.VoiceoverId });
        }
    }
}
