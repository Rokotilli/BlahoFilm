using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var result = _dbContext.Series.FirstOrDefault(s => s.Id == seriesId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.Rating);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateSeries([FromQuery] int seriesId, [FromQuery] int rate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _ratingService.RateSeries(seriesId, rate, userId);
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
