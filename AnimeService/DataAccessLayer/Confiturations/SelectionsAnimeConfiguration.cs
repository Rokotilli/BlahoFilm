using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Configurations
{
    public class SelectionsAnimeConfiguration : IEntityTypeConfiguration<SelectionAnime>
    {
        public void Configure(EntityTypeBuilder<SelectionAnime> builder)
        {
            builder.HasKey(ga => new { ga.AnimeId, ga.SelectionId });
        }
    }
}
