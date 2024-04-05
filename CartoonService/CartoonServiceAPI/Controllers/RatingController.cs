using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace CartoonServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly CartoonServiceDbContext _dbContext;
        private readonly IRatingService _ratingService;

        public RatingController(CartoonServiceDbContext cartoonServiceDbContext, IRatingService ratingService)
        {
            _dbContext = cartoonServiceDbContext;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRating([FromQuery] int cartoonId)
        {
            var result = _dbContext.Cartoons.FirstOrDefault(f => f.Id == cartoonId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.Rating);
        }

        [HttpPost]
        public async Task<IActionResult> RateCartoon([FromQuery] int cartoonId, [FromQuery] int rate)
        {
            //UserId must be from jwt
            var result = await _ratingService.RateCartoon(cartoonId, rate, "user1");
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
