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

        [HttpGet]
        public async Task<IActionResult> GetCommentsForCartoon([FromQuery] int cartoonId)
        {
            var model = _dbContext.Comments.Where(c => c.CartoonId == cartoonId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetCommentsForCartoonPart([FromQuery] int cartoonPartId)
        {
            var model = _dbContext.Comments.Where(c => c.CartoonPartId == cartoonPartId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        [Authorize]
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
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCommentForCartoonPart(CommentAddModel commentAddModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _commentService.AddComment(commentAddModel, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
        [HttpDelete]
        [Authorize]
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

        [HttpPut]
        [Authorize]
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
