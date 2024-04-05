using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace SeriesServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly SeriesServiceDbContext _dbContext;

        public SeriesController(SeriesServiceDbContext SeriesServiceDbContext)
        {
            _dbContext = SeriesServiceDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaggedSeries([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Series
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s=>new
                {
                    Poster = s.Poster,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    StudioName = s.StudioName,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("countpages")]
        public async Task<IActionResult> GetCountPagesSeries([FromQuery] int pageSize)
        {
            var model = _dbContext.Series.Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbygenres")]
        public async Task<IActionResult> GetCountPagesSeriesByGenres([FromQuery] int pageSize, [FromBody] string[] genres)
        {
            var model = _dbContext.Series
                .Where(s => genres.All(g => s.GenresSeries.Any(gf => gf.Genre.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbytags")]
        public async Task<IActionResult> GetCountPagesSeriesByTags([FromQuery] int pageSize, [FromBody] string[] tags)
        {
            var model = _dbContext.Series
                .Where(s => tags.All(g => s.TagsSeries.Any(gf => gf.Tag.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetSeriesById([FromQuery] int id)
        {
            var model = _dbContext.Series.FirstOrDefault(s => s.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetSeriesByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Series
                .Where(s => ids
                .Contains(s.Id))
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
                .Where(s => s.Title == title)
                .Select(s => new
                {
                    Poster = s.Poster,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    StudioName = s.StudioName,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bygenres")]
        public async Task<IActionResult> GetPaggedSeriesByGenres([FromBody] string[] genres, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Series
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(s => genres.All(g => s.GenresSeries.Any(gf => gf.Genre.Name == g)))
                .Select(s => new
                {
                    Poster = s.Poster,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    StudioName = s.StudioName,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytags")]
        public async Task<IActionResult> GetPaggedSeriesByTags([FromBody] string[] tags, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Series
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(s => tags.All(g => s.TagsSeries.Any(gf => gf.Tag.Name == g)))
                .Select(s => new
                {
                    Poster = s.Poster,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    StudioName = s.StudioName,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction
                })
                .ToArray();
            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}