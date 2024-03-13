using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace FilmServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly FilmServiceDbContext _dbContext;
        private readonly ICommentService _commentService;

        public CommentsController(FilmServiceDbContext filmServiceDbContext, ICommentService commentService)
        {
            _dbContext = filmServiceDbContext;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsForFilm([FromQuery] int filmId)
        {
            var model = _dbContext.Comments.Where(c => c.FilmId == filmId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentForFilm(CommentAddModel commentAddModel)
        {
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
            var result = await _commentService.Dislike(commentId, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
