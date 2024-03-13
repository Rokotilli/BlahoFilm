using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetFavouritesByUserId()
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
        public async Task<IActionResult> Favourite(MediaWithType mediaWithType)
        {
            //UserId must be from jwt
            var result = await _favoritesService.Favorite(mediaWithType, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
