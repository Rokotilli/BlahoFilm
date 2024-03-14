using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class MediaWithTypeConfiguration : IEntityTypeConfiguration<MediaWithType>
    {
        public void Configure(EntityTypeBuilder<MediaWithType> builder)
        {
            builder.Property(mwt => mwt.MediaId).ValueGeneratedNever();

            builder.HasIndex(mwt => new { mwt.MediaId, mwt.MediaTypeId }).IsUnique();
        }
    }
}
