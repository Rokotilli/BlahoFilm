using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class CategoriesFilmConfiguration : IEntityTypeConfiguration<CategoriesFilm>
    {
        public void Configure(EntityTypeBuilder<CategoriesFilm> builder)
        {
            builder.HasKey(tf => new { tf.FilmId, tf.CategoryId });
        }
    }
}
