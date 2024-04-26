using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize]
        public async Task<IActionResult> RateCartoon([FromQuery] int cartoonId, [FromQuery] int rate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _ratingService.RateCartoon(cartoonId, rate,userId);
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
