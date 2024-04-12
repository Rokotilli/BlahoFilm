using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnimeServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AnimeServiceDbContext _dbContext;
        private readonly ICommentService _commentService;

        public CommentsController(AnimeServiceDbContext animeServiceDbContext, ICommentService commentService)
        {
            _dbContext = animeServiceDbContext;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsForAnime([FromQuery] int animeId)
        {
            var model = _dbContext.Comments.Where(a => a.AnimeId == animeId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetCommentsForAnimePart([FromQuery] int animePartId)
        {
            var model = _dbContext.Comments.Where(a => a.AnimePartId == animePartId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCommentForAnime(CommentAddModel commentAddModel)
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
        public async Task<IActionResult> AddCommentForAnimePart(CommentAddModel commentAddModel)
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
        [Authorize]
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
        [Authorize]
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
