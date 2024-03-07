using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class GenresAnimeConfiguration : IEntityTypeConfiguration<GenresCartoon>
    {
        public void Configure(EntityTypeBuilder<GenresCartoon> builder)
        {
            builder.HasKey(gc => new { gc.CartoonId, gc.GenreId });
        }
    }
}
