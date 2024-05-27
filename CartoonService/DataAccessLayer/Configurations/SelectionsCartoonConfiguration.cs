using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class SelectionsCartoonConfiguration : IEntityTypeConfiguration<SelectionCartoon>
    {
        public void Configure(EntityTypeBuilder<SelectionCartoon> builder)
        {
            builder.HasKey(gc => new { gc.CartoonId, gc.SelectionId });
        }
    }
}
