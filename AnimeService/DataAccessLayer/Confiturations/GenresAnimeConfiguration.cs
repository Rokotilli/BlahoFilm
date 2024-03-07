using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Confiturations
{
    public class GenresAnimeConfiguration : IEntityTypeConfiguration<GenresAnime>
    {
        public void Configure(EntityTypeBuilder<GenresAnime> builder)
        {
            builder.HasKey(ga => new { ga.AnimeId, ga.GenreId });
        }
    }
}
