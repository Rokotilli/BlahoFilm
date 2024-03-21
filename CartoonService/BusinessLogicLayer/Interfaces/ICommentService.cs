using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICommentService
    {
        Task<string> AddComment(CommentAddModel commentAddModel, string userId);
        Task<string> ChangeComment(int commentId, string userId, string text);
        Task<string> DeleteComment(int commentId, string userId);
        Task<string> Like(int commentId, string userId);
        Task<string> Dislike(int commentId, string userId);
    }
}
