using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Confiturations
{
    public class TagsAnimeConfiguration : IEntityTypeConfiguration<TagsAnime>
    {
        public void Configure(EntityTypeBuilder<TagsAnime> builder)
        {
            builder.HasKey(ta => new { ta.AnimeId, ta.TagId });
        }
    }
}
