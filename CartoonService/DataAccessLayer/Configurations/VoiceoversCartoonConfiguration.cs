
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class VoiceoversCartoonConfiguration : IEntityTypeConfiguration<VoiceoversCartoon>
    {
        public void Configure(EntityTypeBuilder<VoiceoversCartoon> builder)
        {
            builder.HasKey(tf => new { tf.CartoonId, tf.VoiceoverId });
        }
    }
}
