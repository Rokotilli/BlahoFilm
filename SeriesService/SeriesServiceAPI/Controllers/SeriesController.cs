using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SeriesServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly SeriesServiceDbContext _dbContext;
        private readonly ISeriesService _seriesService;
        public SeriesController(SeriesServiceDbContext SeriesServiceDbContext, IGetSaSService getSaSService, ISeriesService seriesService)
        {
            _dbContext = SeriesServiceDbContext;
            _getSaSService = getSaSService;
            _seriesService = seriesService;
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
        public async Task<IActionResult> GetSeriesById([FromQuery] int id)
        {
            var model = await _dbContext.Series
            .Include(a => a.GenresSeries).ThenInclude(ga => ga.Genre)
            .Include(a => a.CategoriesSeries).ThenInclude(ca => ca.Category)
            .Include(a => a.StudiosSeries).ThenInclude(sa => sa.Studio)
            .FirstOrDefaultAsync(a => a.Id == id);

            if (model == null)
            {
                return NotFound();
            }
            var result = SeriesService.ToReturnSeries(model);
            return Ok(result);
        }

        [HttpPost("byids")]
        public async Task<IActionResult> GetSeriesByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Series
              .Where(a => ids.Contains(a.Id))
              .Include(a => a.GenresSeries).ThenInclude(ga => ga.Genre)
              .Include(a => a.CategoriesSeries).ThenInclude(ca => ca.Category)
              .Include(a => a.StudiosSeries).ThenInclude(sa => sa.Studio)
              .Select(a => SeriesService.ToReturnSeries(a))
              .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytitle")]
        public async Task<IActionResult> GetSeriesByTitle([FromQuery] string title)
        {
            var model = _dbContext.Series
                .Where(a => a.Title.Contains(title))
                .Include(a => a.GenresSeries).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesSeries).ThenInclude(ca => ca.Category)
                .Include(a => a.StudiosSeries).ThenInclude(sa => sa.Studio)
                .Select(a => SeriesService.ToReturnSeries(a))
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
        public async Task<IActionResult> GetPaggedSeriesByFilter(
          [FromBody] Dictionary<string, string[]>? filters,
          [FromQuery] int pageNumber,
          [FromQuery] int pageSize,
          [FromQuery] string? sortByDate,
          [FromQuery] string? sortByPopularity)
        {
            var model = _seriesService.GetSeriesByFilterAndSorting(filters, pageNumber, pageSize, sortByDate, sortByPopularity);

            if (model == null || !model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("countpagesbyfiltersandsorting")]
        public async Task<IActionResult> GetCountPagesSeriesByGenres(
            [FromBody] Dictionary<string, string[]>? filters,
            [FromQuery] int pageSize,
            [FromQuery] string? sortByDate,
            [FromQuery] string? sortByPopularity)
        {
            var model = _seriesService.GetCountPagesSeriesByFilter(filters, pageSize, sortByDate, sortByPopularity);

            if (model == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}