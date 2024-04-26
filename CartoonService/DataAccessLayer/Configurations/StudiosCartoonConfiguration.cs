using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class StudiosCartoonConfiguration : IEntityTypeConfiguration<StudiosCartoon>
    {
        public void Configure(EntityTypeBuilder<StudiosCartoon> builder)
        {
            builder.HasKey(tf => new { tf.CartoonId, tf.StudioId });
        }
    }
}