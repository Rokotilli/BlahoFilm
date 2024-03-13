using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MessageBus.Enums;

namespace DataAccessLayer.Configurations
{
    public class MediaTypeConfiguration : IEntityTypeConfiguration<MediaType>
    {
        public void Configure(EntityTypeBuilder<MediaType> builder)
        {
            builder.HasData(
                new MediaType { Id = 1, Name = MediaTypes.Film.ToString() },
                new MediaType { Id = 2, Name = MediaTypes.Series.ToString() },
                new MediaType { Id = 3, Name = MediaTypes.Cartoon.ToString() },
                new MediaType { Id = 4, Name = MediaTypes.Anime.ToString() }
                );
        }
    }
}
