using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
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
                .Select(s => new
                {
                    Poster = s.Poster,
                    PosterPartOne = s.PosterPartOne,
                    PosterPartTwo = s.PosterPartTwo,
                    PosterPartThree = s.PosterPartThree,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction,
                    Genres = s.GenresSeries.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Categories = s.CategoriesSeries.Select(tf => new Category { Id = tf.CategoryId, Name = tf.Category.Name }),
                    Studios = s.StudiosSeries.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),
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

        [HttpGet("countpagesbycategories")]
        public async Task<IActionResult> GetCountPagesSeriesByCategories([FromQuery] int pageSize, [FromBody] string[] categories)
        {
            var model = _dbContext.Series
                .Where(s => categories.All(g => s.CategoriesSeries.Any(gf => gf.Category.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }
        [HttpGet("countpagesbystudios")]
        public async Task<IActionResult> GetCountPagesSeriesByStudios([FromQuery] int pageSize, [FromBody] string[] studios)
        {
            var model = _dbContext.Series
                .Where(s => studios.All(g => s.StudiosSeries.Any(gf => gf.Studio.Name == g)))
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
                .Where(s => s.Title.Contains(title))
                .Select(s => new
                {
                    Poster = s.Poster,
                    PosterPartOne = s.PosterPartOne,
                    PosterPartTwo = s.PosterPartTwo,
                    PosterPartThree = s.PosterPartThree,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction,
                    Genres = s.GenresSeries.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Categories = s.CategoriesSeries.Select(tf => new Category { Id = tf.CategoryId, Name = tf.Category.Name }),
                    Studios = s.StudiosSeries.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),

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
                    PosterPartOne = s.PosterPartOne,
                    PosterPartTwo = s.PosterPartTwo,
                    PosterPartThree = s.PosterPartThree,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction,
                    Genres = s.GenresSeries.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Categories = s.CategoriesSeries.Select(tf => new Category { Id = tf.CategoryId, Name = tf.Category.Name }),
                    Studios = s.StudiosSeries.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),

                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bycategories")]
        public async Task<IActionResult> GetPaggedSeriesByCategories([FromBody] string[] categories, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Series
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(s => categories.All(g => s.CategoriesSeries.Any(gf => gf.Category.Name == g)))
                .Select(s => new
                {
                    Poster = s.Poster,
                    PosterPartOne = s.PosterPartOne,
                    PosterPartTwo = s.PosterPartTwo,
                    PosterPartThree = s.PosterPartThree,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction,
                    Genres = s.GenresSeries.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Categories = s.CategoriesSeries.Select(tf => new Category { Id = tf.CategoryId, Name = tf.Category.Name }),
                    Studios = s.StudiosSeries.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),

                })
                .ToArray();
            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("bystudios")]
        public async Task<IActionResult> GetPaggedSeriesByStudios([FromBody] string[] studios, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Series
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(s => studios.All(g => s.StudiosSeries.Any(gf => gf.Studio.Name == g)))
                .Select(s => new
                {
                    Poster = s.Poster,
                    PosterPartOne = s.PosterPartOne,
                    PosterPartTwo = s.PosterPartTwo,
                    PosterPartThree = s.PosterPartThree,
                    Title = s.Title,
                    Description = s.Description,
                    CountSeasons = s.CountSeasons,
                    CountParts = s.CountParts,
                    Year = s.Year,
                    Director = s.Director,
                    Rating = s.Rating,
                    TrailerUri = s.TrailerUri,
                    AgeRestriction = s.AgeRestriction,
                    Genres = s.GenresSeries.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Categories = s.CategoriesSeries.Select(tf => new Category { Id = tf.CategoryId, Name = tf.Category.Name }),
                    Studios = s.StudiosSeries.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),

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