using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly AnimeServiceDbContext _dbContext;
        private readonly IGetSaSService _getSaSService;
        private readonly IAnimeService _animeService;
        public AnimeController(AnimeServiceDbContext AnimeServiceDbContext, IGetSaSService getSaSService, IAnimeService animeService)
        {
            _dbContext = AnimeServiceDbContext;
            _getSaSService = getSaSService;
            _animeService = animeService;
        }
        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Read);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Can't get a SaS");
        }
        [HttpGet("byid")]
        public async Task<IActionResult> GetAnimeById([FromQuery] int id)
        {
            var anime = await _dbContext.Animes.Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
            .Include(a => a.CategoriesAnimes).ThenInclude(ca => ca.Category)
            .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
            .FirstOrDefaultAsync(a => a.Id == id)
            ;

            if (anime == null)
            {
                return NotFound();
            }
            var model = AnimeService.ToReturnAnime(anime);
            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetAnimesByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Animes
              .Where(a => ids.Contains(a.Id))
              .Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
              .Include(a => a.CategoriesAnimes).ThenInclude(ca => ca.Category)
              .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
              .Select(a => AnimeService.ToReturnAnime(a))
              .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytitle")]
        public async Task<IActionResult> GetAnimesByTitle([FromQuery] string title)
        {
            var model = _dbContext.Animes
                .Where(a => a.Title.Contains(title))
                .Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesAnimes).ThenInclude(ca => ca.Category)
                .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
                .Select(a => AnimeService.ToReturnAnime(a))
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
        public async Task<IActionResult> GetPaggedAnimesByFilter(
          [FromBody] Dictionary<string, string[]>? filters,
          [FromQuery] int pageNumber,
          [FromQuery] int pageSize,
          [FromQuery] string? sortByDate,
          [FromQuery] string? sortByPopularity)
        {
            var model = _animeService.GetAnimeByFilterAndSorting(filters, pageNumber, pageSize, sortByDate, sortByPopularity);

            if (model == null || !model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("countpagesbyfiltersandsorting")]
        public async Task<IActionResult> GetCountPagesAnimeByGenres(
            [FromBody] Dictionary<string, string[]>? filters,
            [FromQuery] int pageSize,
            [FromQuery] string? sortByDate,
            [FromQuery] string? sortByPopularity)
        {
            var model = _animeService.GetCountPagesAnimeByFilter(filters, pageSize, sortByDate, sortByPopularity);

            if (model == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}