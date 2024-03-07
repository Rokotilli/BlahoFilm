using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class TagsCartoonConfiguration : IEntityTypeConfiguration<TagsCartoon>
    {
        public void Configure(EntityTypeBuilder<TagsCartoon> builder)
        {
            builder.HasKey(tc => new { tc.CartoonId, tc.TagId });
        }
    }
}
