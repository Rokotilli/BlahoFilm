using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly UserServiceDbContext _dbContext;

        public HistoryController(IHistoryService historyService, UserServiceDbContext userServiceDbContext)
        {
            _historyService = historyService;
            _dbContext = userServiceDbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetHistoryByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _dbContext.Histories.Where(f => f.UserId == userId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeHistory(HistoryModel historyModel)
        {
            string result = "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (historyModel.PartNumber == 0 && historyModel.SeasonNumber == 0)
            {
                result = await _historyService.AddHistoryForFilm(userId, historyModel);
            }
            else
            {
                result = await _historyService.AddHistoryForSeries(userId, historyModel);
            }

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
