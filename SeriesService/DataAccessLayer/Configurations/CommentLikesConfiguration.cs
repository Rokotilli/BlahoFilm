using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class CommentLikesConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
        {
            builder.HasKey(gf => new { gf.UserId, gf.CommentId });

            builder.HasOne(cd => cd.User)
                   .WithMany(u => u.CommentLikes)
                   .HasForeignKey(cd => cd.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(cd => cd.Comment)
                   .WithMany(c => c.CommentLikes)
                   .HasForeignKey(cd => cd.CommentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}