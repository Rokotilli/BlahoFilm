using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddCommentForAnime(CommentAddModel commentAddModel)
        {
            //UserId must be from jwt
            var result = await _commentService.AddComment(commentAddModel, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddCommentForAnimePart(CommentAddModel commentAddModel)
        {
            //UserId must be from jwt
            var result = await _commentService.AddComment(commentAddModel, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromQuery] int commentId)
        {
            //UserId must be from jwt
            var result = await _commentService.DeleteComment(commentId, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeComment(ChangeCommentModel changeCommentModel)
        {
            //UserId must be from jwt
            var result = await _commentService.ChangeComment(changeCommentModel.Id, "user1", changeCommentModel.Text);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like([FromQuery] int commentId)
        {
            //UserId must be from jwt
            var result = await _commentService.Like(commentId, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPost("dislike")]
        public async Task<IActionResult> Dislike([FromQuery] int commentId)
        {
            //UserId must be from jwt
            var result = await _commentService.Dislike(commentId, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
