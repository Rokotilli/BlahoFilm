using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace AnimeServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly AnimeServiceDbContext _dbContext;
        private readonly IRatingService _ratingService;

        public RatingController(AnimeServiceDbContext animeServiceDbContext, IRatingService ratingService)
        {
            _dbContext = animeServiceDbContext;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRating([FromQuery] int animeId)
        {
            var result = _dbContext.Animes.FirstOrDefault(f => f.Id == animeId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.Rating);
        }

        [HttpPost]
        public async Task<IActionResult> RateAnime([FromQuery] int animeId, [FromQuery] int rate)
        {
            //UserId must be from jwt
            var result = await _ratingService.RateAnime(animeId, rate, "user1");
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
