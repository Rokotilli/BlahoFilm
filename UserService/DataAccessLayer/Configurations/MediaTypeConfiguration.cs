using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class MediaTypeConfiguration : IEntityTypeConfiguration<MediaType>
    {
        public void Configure(EntityTypeBuilder<MediaType> builder)
        {
            builder.HasData(
                new MediaType { Id = 1, Name = "Film" },
                new MediaType { Id = 2, Name = "Series" },
                new MediaType { Id = 3, Name = "Cartoon" },
                new MediaType { Id = 4, Name = "Anime" }
                );
        }
    }
}
