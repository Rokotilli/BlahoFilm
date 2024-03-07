using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class TagsSeriesConfiguration : IEntityTypeConfiguration<TagsSeries>
    {
        public void Configure(EntityTypeBuilder<TagsSeries> builder)
        {
            builder.HasKey(ta => new { ta.SeriesId, ta.TagId });
        }
    }
}
