using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class CategoriesCartoonConfiguration : IEntityTypeConfiguration<CategoriesCartoon>
    {
        public void Configure(EntityTypeBuilder<CategoriesCartoon> builder)
        {
            builder.HasKey(tc => new { tc.CartoonId, tc.CategoryId });
        }
    }
}
