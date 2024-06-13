using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class AnimationTypesCartoonConfiguration : IEntityTypeConfiguration<AnimationTypesCartoon>
    {
        public void Configure(EntityTypeBuilder<AnimationTypesCartoon> builder)
        {
            builder.HasKey(gc => new { gc.CartoonId, gc.AnimationTypeId });
        }
    }
}
