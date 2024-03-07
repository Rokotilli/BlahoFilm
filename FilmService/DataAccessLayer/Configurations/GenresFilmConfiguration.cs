using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class GenresFilmConfiguration : IEntityTypeConfiguration<GenresFilm>
    {
        public void Configure(EntityTypeBuilder<GenresFilm> builder)
        {
            builder.HasKey(gf => new { gf.FilmId, gf.GenreId });
        }
    }
}
