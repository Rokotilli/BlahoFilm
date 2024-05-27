using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class SelectionsSeriesConfiguration : IEntityTypeConfiguration<SelectionSeries>
    {
        public void Configure(EntityTypeBuilder<SelectionSeries> builder)
        {
            builder.HasKey(ga => new { ga.SeriesId, ga.SelectionId });
        }
    }
}
