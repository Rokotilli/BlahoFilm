using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class StudiosSeriesConfiguration : IEntityTypeConfiguration<StudiosSeries>
    {
        public void Configure(EntityTypeBuilder<StudiosSeries> builder)
        {
            builder.HasKey(tf => new { tf.SeriesId, tf.StudioId });
        }
    }
}