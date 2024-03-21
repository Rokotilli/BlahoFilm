using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddCommentForCartoon(CommentAddModel commentAddModel)
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
        public async Task<IActionResult> AddCommentForCartoonPart(CommentAddModel commentAddModel)
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
