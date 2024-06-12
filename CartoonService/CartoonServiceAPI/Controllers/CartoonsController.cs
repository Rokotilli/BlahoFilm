using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartoonServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartoonsController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly CartoonServiceDbContext _dbContext;
        private readonly ICartoonService _cartoonService;
        public CartoonsController(CartoonServiceDbContext CartoonServiceDbContext, IGetSaSService getSaSService, ICartoonService animeService)
        {
            _dbContext = CartoonServiceDbContext;
            _getSaSService = getSaSService;
            _cartoonService = animeService;
        }
        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName, [FromQuery] int animeId)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Read);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Can't get a SaS");
        }
        [HttpPost("byid")]
        public async Task<IActionResult> GetCartoonById([FromBody] int id)
        {
            var model = await _dbContext.Cartoons
            .Include(c => c.GenresCartoons).ThenInclude(ga => ga.Genre)
            .Include(c => c.CategoriesCartoons).ThenInclude(ca => ca.Category)
            .Include(c => c.StudiosCartoons).ThenInclude(sa => sa.Studio)
            .FirstOrDefaultAsync(c => c.Id == id);


            if (model == null)
            {
                return NotFound();
            }
            var result = CartoonService.ToReturnCartoon(model);
            return Ok(result);
        }

        [HttpPost("byids")]
        public async Task<IActionResult> GetCartoonsByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Cartoons
              .Where(a => ids.Contains(a.Id))
              .Include(a => a.GenresCartoons).ThenInclude(ga => ga.Genre)
              .Include(a => a.CategoriesCartoons).ThenInclude(ca => ca.Category)
              .Include(a => a.StudiosCartoons).ThenInclude(sa => sa.Studio)
              .Select(a => CartoonService.ToReturnCartoon(a))
              .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytitle")]
        public async Task<IActionResult> GetCartoonsByTitle([FromQuery] string title)
        {
            var model = _dbContext.Cartoons
                .Where(a => a.Title.Contains(title))
                .Include(a => a.GenresCartoons).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesCartoons).ThenInclude(ca => ca.Category)
                .Include(a => a.StudiosCartoons).ThenInclude(sa => sa.Studio)
                .Select(a => CartoonService.ToReturnCartoon(a))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetAllGenres()
        {
            var model = await _dbContext.Genres.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllTags()
        {
            var model = await _dbContext.Categories.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("studios")]
        public async Task<IActionResult> GetAllStudios()
        {
            var model = await _dbContext.Studios.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("selections")]
        public async Task<IActionResult> GetAllSelections()
        {
            var model = await _dbContext.Selections.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpPost("byfiltersandsorting")]
        public async Task<IActionResult> GetPaggedCartoonsByFilter(
          [FromBody] Dictionary<string, string[]>? filters,
          [FromQuery] int pageNumber,
          [FromQuery] int pageSize,
          [FromQuery] string? sortByDate,
          [FromQuery] string? sortByPopularity)
        {
            var model = _cartoonService.GetCartoonsByFilterAndSorting(filters, pageNumber, pageSize, sortByDate, sortByPopularity);

            if (model == null || !model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("countpagesbyfiltersandsorting")]
        public async Task<IActionResult> GetCountPagesCartoonByGenres(
            [FromBody] Dictionary<string, string[]>? filters,
            [FromQuery] int pageSize,
            [FromQuery] string? sortByDate,
            [FromQuery] string? sortByPopularity)
        {
            var model = _cartoonService.GetCountPagesCartoonsByFilter(filters, pageSize, sortByDate, sortByPopularity);

            if (model == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}