using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => new { r.UserId, r.SeriesId});
            builder.ToTable(t => t.HasCheckConstraint("CK_Rating_Rate_Range", "[Rate] >= 1 AND [Rate] <= 10"));
        }
    }
}