using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class SelectionsFilmConfiguration : IEntityTypeConfiguration<SelectionsFilm>
    {
        public void Configure(EntityTypeBuilder<SelectionsFilm> builder)
        {
            builder.HasKey(gf => new { gf.FilmId, gf.SelectionId });
        }
    }
}
