using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var result = _dbContext.Films.FirstOrDefault(f => f.Id == filmId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.Rating);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Rate([FromQuery] int filmId, [FromQuery] int rate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _ratingService.Rate(filmId, rate, userId);
            
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
