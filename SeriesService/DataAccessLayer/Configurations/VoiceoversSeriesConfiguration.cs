
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class VoiceoversSeriesConfiguration : IEntityTypeConfiguration<VoiceoversSeries>
    {
        public void Configure(EntityTypeBuilder<VoiceoversSeries> builder)
        {
            builder.HasKey(tf => new { tf.SeriesId, tf.VoiceoverId });
        }
    }
}
