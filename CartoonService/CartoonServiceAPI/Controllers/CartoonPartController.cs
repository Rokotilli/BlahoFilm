using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public async Task<IActionResult> GetPaggedCartoonParts([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.CartoonParts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(sp => new
                {
                    Id = sp.Id,
                    CartoonId = sp.CartoonId,
                    SeasonNumber = sp.SeasonNumber,
                    PartNumber = sp.PartNumber,
                    Duration = sp.Duration,
                    FileName = sp.FileName,
                    FileUri = sp.FileUri,
                    Cartoon = sp.Cartoon.Title,
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("bycartoonid")]
        public async Task<IActionResult> GetPaggedCartoonPartsByCartoonId([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] int cartoonId)
        {
            var model = _dbContext.CartoonParts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Where(sp=>sp.CartoonId==cartoonId)
                .Select(sp => new
                {
                    Id = sp.Id,
                    CartoonId = sp.CartoonId,
                    SeasonNumber = sp.SeasonNumber,
                    PartNumber = sp.PartNumber,
                    Duration = sp.Duration,
                    FileName = sp.FileName,
                    FileUri = sp.FileUri,
                    Cartoon = sp.Cartoon.Title,
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("countpages")]
        public async Task<IActionResult> GetCountPagesCartoonParts([FromQuery] int pageSize)
        {
            var model = _dbContext.CartoonParts.Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetCartoonPartById([FromQuery] int id)
        {
            var model = _dbContext.CartoonParts.FirstOrDefault(s => s.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
