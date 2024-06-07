using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartoonServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartoonPartController : ControllerBase
    {
        private readonly CartoonServiceDbContext _dbContext;

        public CartoonPartController(CartoonServiceDbContext CartoonServiceDbContext)
        {
            _dbContext = CartoonServiceDbContext;
        }
       
        [HttpGet("getbycartoonid")]
        public async Task<IActionResult> GetSeriesPartsBySeriesId([FromQuery] int cartoonId)
        {
            var model = _dbContext.CartoonParts.Include(sp => sp.Cartoon).Where(sp => sp.CartoonId == cartoonId)
                .Select(cp => new
                {
                    Id = cp.Id,
                    CartoonId = cp.CartoonId,
                    SeasonNumber = cp.SeasonNumber,
                    PartNumber = cp.PartNumber,
                    Duration = cp.Duration,
                    FileName = cp.FileName,
                    FileUri = cp.FileUri,
                    CartoonTitle = cp.Cartoon.Title,
                }
                )
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetSeriesPartById([FromQuery] int Id)
        {
            var model = await _dbContext.CartoonParts.Include(ap => ap.Cartoon)
                .FirstOrDefaultAsync(ap => ap.Id == Id);

            if (model == null)
            {
                return NotFound();
            }
            var result = new
            {
                Id = model.Id,
                CartoonId = model.CartoonId,
                SeasonNumber = model.SeasonNumber,
                PartNumber = model.PartNumber,
                Duration = model.Duration,
                FileName = model.FileName,
                FileUri = model.FileUri,
                CartoonTitle = model.Cartoon.Title,
            };
            return Ok(result);
        }
        [HttpGet("getbyseason")]
        public async Task<IActionResult> GetSeriesPartsBySeason([FromQuery] int cartoonId, [FromQuery] int season)
        {
            var model = _dbContext.CartoonParts.Include(sp => sp.Cartoon).Where(sp => sp.CartoonId == cartoonId && sp.SeasonNumber == season)
                .Select(cp => new
                {
                    Id = cp.Id,
                    CartoonId = cp.CartoonId,
                    SeasonNumber = cp.SeasonNumber,
                    PartNumber = cp.PartNumber,
                    Duration = cp.Duration,
                    FileName = cp.FileName,
                    FileUri = cp.FileUri,
                    CartoonTitle = cp.Cartoon.Title,
                }
                )
                .ToArray();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
