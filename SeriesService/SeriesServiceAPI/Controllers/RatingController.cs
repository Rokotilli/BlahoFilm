using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace SeriesServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly SeriesServiceDbContext _dbContext;
        private readonly IRatingService _ratingService;

        public RatingController(SeriesServiceDbContext seriesServiceDbContext, IRatingService ratingService)
        {
            _dbContext = seriesServiceDbContext;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRating([FromQuery] int seriesId)
        {
            var result = _dbContext.Series.FirstOrDefault(f => f.Id == seriesId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.Rating);
        }

        [HttpPost]
        public async Task<IActionResult> RateSeries([FromQuery] int seriesId, [FromQuery] int rate)
        {
            //UserId must be from jwt
            var result = await _ratingService.RateSeries(seriesId, rate, "user1");
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RateSeriesPart([FromQuery] int seriesPartId, [FromQuery] int rate)
        {
            //UserId must be from jwt
            var result = await _ratingService.RateSeriesPart(seriesPartId, rate, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
