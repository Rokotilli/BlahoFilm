using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmServiceDbContext _dbContext;
        private readonly IFilmService _filmService;
        private readonly IGetSaSService _getSaSService;

        public FilmsController(FilmServiceDbContext filmServiceDbContext, IFilmService filmService, IGetSaSService getSaSService)
        {
            _dbContext = filmServiceDbContext;
            _filmService = filmService;
            _getSaSService = getSaSService;
        }      

        [HttpGet("byid")]
        public async Task<IActionResult> GetFilmById([FromQuery] int id)
        {
            var film = await _dbContext.Films
            .Include(f => f.GenresFilms).ThenInclude(gf => gf.Genre)
            .Include(f => f.CategoriesFilms).ThenInclude(cf => cf.Category)
            .Include(f => f.StudiosFilms).ThenInclude(sf => sf.Studio)
            .FirstOrDefaultAsync(f => f.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            var model = FilmService.ToReturnFilms(film);

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetFilmsByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Films
                .Where(f => ids.Contains(f.Id))
                .Include(f => f.GenresFilms).ThenInclude(gf => gf.Genre)
                .Include(f => f.CategoriesFilms).ThenInclude(cf => cf.Category)
                .Include(f => f.StudiosFilms).ThenInclude(sf => sf.Studio)
                .Select(f => FilmService.ToReturnFilms(f))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytitle")]
        public async Task<IActionResult> GetFilmsByTitle([FromQuery] string title)
        {
            var model = _dbContext.Films
                .Where(f => f.Title.Contains(title))
                .Include(f => f.GenresFilms).ThenInclude(gf => gf.Genre)
                .Include(f => f.CategoriesFilms).ThenInclude(cf => cf.Category)
                .Include(f => f.StudiosFilms).ThenInclude(sf => sf.Studio)
                .Select(f => FilmService.ToReturnFilms(f))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("byfiltersandsorting")]
        public async Task<IActionResult> GetPaggedFilmsByFilter(
            [FromBody] Dictionary<string, string[]>? filters,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? sortByDate,
            [FromQuery] string? sortByPopularity)
        {
            var model = _filmService.GetFilmsByFilterAndSorting(filters, pageNumber, pageSize, sortByDate, sortByPopularity);

            if (model == null || !model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("countpagesbyfiltersandsorting")]
        public async Task<IActionResult> GetCountPagesFilmsByGenres(
            [FromBody] Dictionary<string, string[]>? filters,
            [FromQuery] int pageSize,
            [FromQuery] string? sortByDate,
            [FromQuery] string? sortByPopularity)
        {
            var model = _filmService.GetCountPagesFilmsByFilter(filters, pageSize, sortByDate, sortByPopularity);

            if (model == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName, [FromQuery] int filmId)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Read);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Can't get a SaS");
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
    }
}
