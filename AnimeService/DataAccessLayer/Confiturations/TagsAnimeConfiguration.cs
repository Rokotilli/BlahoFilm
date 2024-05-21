using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Confiturations
{
    public class CategoriesAnimeConfiguration : IEntityTypeConfiguration<CategoriesAnime>
    {
        public void Configure(EntityTypeBuilder<CategoriesAnime> builder)
        {
            builder.HasKey(ta => new { ta.AnimeId, ta.CategoryId });
        }
    }
}
