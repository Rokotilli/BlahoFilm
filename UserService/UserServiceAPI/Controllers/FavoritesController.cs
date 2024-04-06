using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(UserServiceDbContext userServiceDbContext, IFavoritesService favoritesService)
        {
            _dbContext = userServiceDbContext;
            _favoritesService = favoritesService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFavouritesByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _dbContext.Favorites.Where(f => f.UserId == userId).ToArray();

            if (!model.Any()) 
            {
                return NotFound();
            }

            return Ok(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Favourite(MediaWithType mediaWithType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _favoritesService.Favorite(mediaWithType, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
