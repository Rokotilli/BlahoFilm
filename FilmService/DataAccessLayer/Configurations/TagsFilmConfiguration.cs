using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class TagsFilmConfiguration : IEntityTypeConfiguration<TagsFilm>
    {
        public void Configure(EntityTypeBuilder<TagsFilm> builder)
        {
            builder.HasKey(tf => new { tf.FilmId, tf.TagId });
        }
    }
}
