using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookMarksController : ControllerBase
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IBookMarksService _bookMarksService;

        public BookMarksController(UserServiceDbContext userServiceDbContext, IBookMarksService bookMarksService)
        {
            _dbContext = userServiceDbContext;
            _bookMarksService = bookMarksService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBookMarksByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _dbContext.BookMarks.Where(f => f.UserId == userId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BookMark(MediaWithType mediaWithType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookMarksService.BookMark(mediaWithType, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
