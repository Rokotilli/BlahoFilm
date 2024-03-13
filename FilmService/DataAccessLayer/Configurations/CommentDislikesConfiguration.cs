using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class CommentDislikesConfiguration : IEntityTypeConfiguration<CommentDislike>
    {
        public void Configure(EntityTypeBuilder<CommentDislike> builder)
        {
            builder.HasKey(gf => new { gf.UserId, gf.CommentId });

            builder.HasOne(cd => cd.User)
                   .WithMany(u => u.CommentDislikes)
                   .HasForeignKey(cd => cd.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(cd => cd.Comment)
                   .WithMany(c => c.CommentDislikes)
                   .HasForeignKey(cd => cd.CommentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
