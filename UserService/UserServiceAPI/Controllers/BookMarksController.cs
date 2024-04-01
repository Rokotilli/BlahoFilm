using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetBookMarksByUserId()
        {
            //UserId must be from jwt
            var model = _dbContext.Favorites.Where(f => f.UserId == "user1").ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookMark(MediaWithType mediaWithType)
        {
            //UserId must be from jwt
            var result = await _bookMarksService.BookMark(mediaWithType, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
