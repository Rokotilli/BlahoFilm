using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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
            var model = _dbContext.Users.FirstOrDefault(u => u.Id == id);

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
                .Contains(u.Id))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [Authorize]
        [HttpPut("avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile avatar)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usersService.ChangeAvatar(userId, avatar);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("totaltime")]
        public async Task<IActionResult> ChangeTotalTime([FromQuery] int seconds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usersService.ChangeTotalTime(userId, seconds);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("changenusername")]
        public async Task<IActionResult> ChangeNickName([FromQuery][StringLength(20, MinimumLength = 3, ErrorMessage = "Max length 20 characters, min length 3 characters")] string username)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _usersService.ChangeNickName(userId, username);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
