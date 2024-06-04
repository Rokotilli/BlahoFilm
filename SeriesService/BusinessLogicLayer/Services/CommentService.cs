using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class CommentService : ICommentService
    {
        private readonly SeriesServiceDbContext _dbContext;

        public CommentService(SeriesServiceDbContext seriesServiceDbContext)
        { 
            _dbContext = seriesServiceDbContext;
        }

        public async Task<string> AddComment(CommentAddModel commentAddModel, string userId)
        {
            try
            {
                var model = new Comment()
                {
                    UserId = userId,
                    SeriesPartId = commentAddModel.SeriesPartId,
                    ParentCommentId = commentAddModel.ParentCommentId,
                    Text = commentAddModel.Text,
                    Date = DateTime.Now
                };

                _dbContext.Comments.Add(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> DeleteComment(int commentId, string userId)
        {
            try
            {
                var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

                if (comment != null && comment.UserId == userId)
                {
                    var likes = _dbContext.CommentLikes.Where(cl => cl.CommentId == commentId).ToArray();
                    var dislikes = _dbContext.CommentDislikes.Where(cl => cl.CommentId == commentId).ToArray();

                    _dbContext.CommentLikes.RemoveRange(likes);
                    _dbContext.CommentDislikes.RemoveRange(dislikes);
                    _dbContext.Comments.Remove(comment);

                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                return "Your comment by inputed id, not found!";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> ChangeComment(int commentId, string userId, string text)
        {
            try
            {
                var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

                if (comment != null && comment.UserId == userId)
                {
                    comment.Text = text;
                    _dbContext.Comments.Update(comment);

                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                return "Your comment by inputed id, not found!";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> Like(int commentId, string userId)
        {
            try
            {
                var existingLike = _dbContext.CommentLikes.FirstOrDefault(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (existingLike != null)
                {
                    _dbContext.CommentLikes.Remove(existingLike);
                    await _dbContext.SaveChangesAsync();
                    return null;
                }

                var existingDislike = _dbContext.CommentDislikes.FirstOrDefault(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (existingDislike != null)
                {
                    _dbContext.CommentDislikes.Remove(existingDislike);
                }

                var newLike = new CommentLike()
                {
                    CommentId = commentId,
                    UserId = userId
                };

                await _dbContext.CommentLikes.AddAsync(newLike);
                await _dbContext.SaveChangesAsync();
                return null;
            }
            catch
            {
                return "Adding or removing like failed!";
            }
        }

        public async Task<string> Dislike(int commentId, string userId)
        {
            try
            {
                var existingDislike = _dbContext.CommentDislikes.FirstOrDefault(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (existingDislike != null)
                {
                    _dbContext.CommentDislikes.Remove(existingDislike);
                    await _dbContext.SaveChangesAsync();
                    return null;
                }

                var existingLike = _dbContext.CommentLikes.FirstOrDefault(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (existingLike != null)
                {
                    _dbContext.CommentLikes.Remove(existingLike);
                }

                var newDislike = new CommentDislike()
                {
                    CommentId = commentId,
                    UserId = userId
                };

                await _dbContext.CommentDislikes.AddAsync(newDislike);
                await _dbContext.SaveChangesAsync();
                return null;
            }
            catch
            {
                return "Adding or removing dislike failed!";
            }
        }
    }
}
