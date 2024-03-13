using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace FilmServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly FilmServiceDbContext _dbContext;
        private readonly IRatingService _ratingService;

        public RatingController(FilmServiceDbContext filmServiceDbContext, IRatingService ratingService)
        {
            _dbContext = filmServiceDbContext;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRating([FromQuery] int filmId)
        {
            var result = await _ratingService.GetRating(filmId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Rate([FromQuery] int filmId, [FromQuery] int rate)
        {
            var result = await _ratingService.Rate(filmId, rate, "user1");
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
