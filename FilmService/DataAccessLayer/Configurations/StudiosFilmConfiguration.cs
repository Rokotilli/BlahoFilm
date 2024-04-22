using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class StudiosFilmConfiguration : IEntityTypeConfiguration<StudiosFilm>
    {
        public void Configure(EntityTypeBuilder<StudiosFilm> builder)
        {
            builder.HasKey(tf => new { tf.FilmId, tf.StudioId });
        }
    }
}
