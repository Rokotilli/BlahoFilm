using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace UserServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServiceDbContext _dbContext;

        public UsersController(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
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


    }
}
