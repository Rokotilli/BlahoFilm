using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class CategoriesSeriesConfiguration : IEntityTypeConfiguration<CategoriesSeries>
    {
        public void Configure(EntityTypeBuilder<CategoriesSeries> builder)
        {
            builder.HasKey(ta => new { ta.SeriesId, ta.CategoryId });
        }
    }
}
