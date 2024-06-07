using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartoonServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CartoonServiceDbContext _dbContext;
        private readonly ICommentService _commentService;

        public CommentsController(CartoonServiceDbContext cartoonServiceDbContext, ICommentService commentService)
        {
            _dbContext = cartoonServiceDbContext;
            _commentService = commentService;
        }

        [HttpGet("getbycartoonid")]
        public async Task<IActionResult> GetCommentsForCartoon([FromQuery] int cartoonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _dbContext.Comments.Where(c => c.CartoonId == cartoonId)
                .GroupJoin(_dbContext.Comments,
                parent => parent.Id,
                reply => reply.ParentCommentId,
                (parent, replies) => new
                {
                    Id = parent.Id,
                    UserId = parent.UserId,
                    Text = parent.Text,
                    ParentCommentId = parent.ParentCommentId,
                    Date = parent.Date,
                    CountLikes = parent.CommentLikes.Count(),
                    CountDislikes = parent.CommentDislikes.Count(),
                    CountReplies = replies.Count(),
                    IsLiked = parent.CommentLikes.Any(cl => cl.UserId == (userId ?? "")),
                    IsDisliked = parent.CommentDislikes.Any(cl => cl.UserId == (userId ?? ""))
                }).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("getbycartoonpartid")]
        public async Task<IActionResult> GetCommentsForCartoonPart([FromQuery] int cartoonPartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _dbContext.Comments.Where(c => c.CartoonPartId == cartoonPartId)
                .GroupJoin(_dbContext.Comments,
                parent => parent.Id,
                reply => reply.ParentCommentId,
                (parent, replies) => new
                {
                    Id = parent.Id,
                    UserId = parent.UserId,
                    Text = parent.Text,
                    ParentCommentId = parent.ParentCommentId,
                    Date = parent.Date,
                    CountLikes = parent.CommentLikes.Count(),
                    CountDislikes = parent.CommentDislikes.Count(),
                    CountReplies = replies.Count(),
                    IsLiked = parent.CommentLikes.Any(cl => cl.UserId == (userId ?? "")),
                    IsDisliked = parent.CommentDislikes.Any(cl => cl.UserId == (userId ?? ""))
                }).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCommentForCartoon(CommentAddModel commentAddModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _commentService.AddComment(commentAddModel, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromQuery] int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _commentService.DeleteComment(commentId, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeComment(ChangeCommentModel changeCommentModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _commentService.ChangeComment(changeCommentModel.Id, userId, changeCommentModel.Text);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("like")]
        public async Task<IActionResult> Like([FromQuery] int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _commentService.Like(commentId, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("dislike")]
        public async Task<IActionResult> Dislike([FromQuery] int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _commentService.Dislike(commentId, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
