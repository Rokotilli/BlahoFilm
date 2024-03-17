using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace UserServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IUsersService _usersService;

        public UsersController(UserServiceDbContext userServiceDbContext, IUsersService usersService)
        {
            _dbContext = userServiceDbContext;
            _usersService = usersService;
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            var model = _dbContext.Users.FirstOrDefault(u => u.UserId == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetUsersByIds([FromBody] string[] ids)
        {
            var model = _dbContext.Users
                .Where(u => ids
                .Contains(u.UserId))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> AddAvatar(IFormFile avatar)
        {
            //UserId must be from jwt
            var result = await _usersService.ChangeAvatar("user1", avatar);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPut("totaltime")]
        public async Task<IActionResult> ChangeTotalTime([FromQuery] int seconds)
        {
            //UserId must be from jwt
            var result = await _usersService.ChangeTotalTime("user1", seconds);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
