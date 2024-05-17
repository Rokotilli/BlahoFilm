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

        [HttpGet]
        public async Task<IActionResult> GetPaggedFilms([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Films
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new ReturnFilms
                {
                    Id = f.Id,
                    Poster = f.Poster,
                    PosterPartOne = f.PosterPartOne,
                    PosterPartTwo = f.PosterPartTwo,
                    PosterPartThree = f.PosterPartThree,
                    Title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    TrailerUri = f.TrailerUri,
                    Genres = f.GenresFilms.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Tags = f.TagsFilms.Select(tf => new Tag { Id = tf.TagId, Name = tf.Tag.Name }),
                    Studios = f.StudiosFilms.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        } 

        [HttpGet("countpages")]
        public async Task<IActionResult> GetCountPagesFilms([FromQuery] int pageSize)
        {
            var model = _dbContext.Films.Count();            

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }        

        [HttpGet("byid")]
        public async Task<IActionResult> GetFilmById([FromQuery] int id)
        {
            var model = _dbContext.Films.Select(f => new ReturnFilms
            {
                Id = f.Id,
                Poster = f.Poster,
                PosterPartOne = f.PosterPartOne,
                PosterPartTwo = f.PosterPartTwo,
                PosterPartThree = f.PosterPartThree,
                Title = f.Title,
                Description = f.Description,
                Duration = f.Duration,
                Year = f.Year,
                Director = f.Director,
                Rating = f.Rating,
                Actors = f.Actors,
                TrailerUri = f.TrailerUri,
                Genres = f.GenresFilms.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                Tags = f.TagsFilms.Select(tf => new Tag { Id = tf.TagId, Name = tf.Tag.Name }),
                Studios = f.StudiosFilms.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),
            }).FirstOrDefault(f => f.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetFilmsByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Films
                .Where(f => ids.Contains(f.Id))
                .Select(f => new ReturnFilms
                {
                    Id = f.Id,
                    Poster = f.Poster,
                    PosterPartOne = f.PosterPartOne,
                    PosterPartTwo = f.PosterPartTwo,
                    PosterPartThree = f.PosterPartThree,
                    Title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    TrailerUri = f.TrailerUri,
                    Genres = f.GenresFilms.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Tags = f.TagsFilms.Select(tf => new Tag { Id = tf.TagId, Name = tf.Tag.Name }),
                    Studios = f.StudiosFilms.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),
                })
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
                .Select(f => new ReturnFilms
                {
                    Id = f.Id,
                    Poster = f.Poster,
                    PosterPartOne = f.PosterPartOne,
                    PosterPartTwo = f.PosterPartTwo,
                    PosterPartThree = f.PosterPartThree,
                    Title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    TrailerUri = f.TrailerUri,
                    Genres = f.GenresFilms.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Tags = f.TagsFilms.Select(tf => new Tag { Id = tf.TagId, Name = tf.Tag.Name }),
                    Studios = f.StudiosFilms.Select(tf => new Studio {Id = tf.StudioId, Name = tf.Studio.Name }),
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byfilters")]
        public async Task<IActionResult> GetPaggedFilmsByFilter([FromBody] Dictionary<string, string[]> filters, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _filmService.GetFilmsByFilter(filters, pageNumber, pageSize);

            if (model == null || !model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("countpagesbyfilters")]
        public async Task<IActionResult> GetCountPagesFilmsByGenres([FromBody] Dictionary<string, string[]> filters, [FromQuery] int pageSize)
        {
            var model = _filmService.GetCountPagesFilmsByFilter(filters, pageSize);

            if (model == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Read);

            if (result != null)
            {
                return Ok();
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

        [HttpGet("tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var model = await _dbContext.Tags.ToArrayAsync();

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
