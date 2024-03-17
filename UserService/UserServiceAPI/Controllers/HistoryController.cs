using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetHistoryByUserId()
        {
            var model = _dbContext.Histories.Where(f => f.UserId == "user1").ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeHistory(HistoryModel historyModel)
        {
            string result = null;

            if (historyModel.PartNumber == 0 && historyModel.SeasonNumber == 0)
            {
                //UserId must be from jwt
                result = await _historyService.AddHistoryForFilm("user1", historyModel);
            }
            else
            {
                //UserId must be from jwt
                result = await _historyService.AddHistoryForSeries("user1", historyModel);
            }

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
