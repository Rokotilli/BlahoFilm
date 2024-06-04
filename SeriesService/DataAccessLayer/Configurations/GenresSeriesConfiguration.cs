using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Configurations
{
    public class GenresSeriesConfiguration : IEntityTypeConfiguration<GenresSeries>
    {
        public void Configure(EntityTypeBuilder<GenresSeries> builder)
        {
            builder.HasKey(ga => new { ga.SeriesId, ga.GenreId });
        }
    }
}