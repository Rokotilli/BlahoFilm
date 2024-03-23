using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmServiceDbContext _dbContext;

        public FilmsController(FilmServiceDbContext filmServiceDbContext)
        {
            _dbContext = filmServiceDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaggedFilms([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Films
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new
                {
                    id = f.Id,
                    Poster = f.Poster,
                    title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    StudioName = f.StudioName,
                    TrailerUri = f.TrailerUri,
                    FileName = f.FileName,
                    FileUri = f.FileUri,
                    genres = f.GenresFilms.Select(gf => new { id = gf.GenreId, name = gf.Genre.Name }),
                    tags = f.TagsFilms.Select(tf => new { id = tf.TagId, name = tf.Tag.Name })
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
            var model = _dbContext.Films.Select(f => new
            {
                id = f.Id,
                Poster = f.Poster,
                title = f.Title,
                Description = f.Description,
                Duration = f.Duration,
                Year = f.Year,
                Director = f.Director,
                Rating = f.Rating,
                Actors = f.Actors,
                StudioName = f.StudioName,
                TrailerUri = f.TrailerUri,
                FileName = f.FileName,
                FileUri = f.FileUri,
                genres = f.GenresFilms.Select(gf => new { id = gf.GenreId, name = gf.Genre.Name }),
                tags = f.TagsFilms.Select(tf => new { id = tf.TagId, name = tf.Tag.Name })
            }).FirstOrDefault(f => f.id == id);

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
                .Where(f => ids
                .Contains(f.Id))
                .Select(f => new
                {
                    id = f.Id,
                    Poster = f.Poster,
                    title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    StudioName = f.StudioName,
                    TrailerUri = f.TrailerUri,
                    FileName = f.FileName,
                    FileUri = f.FileUri,
                    genres = f.GenresFilms.Select(gf => new { id = gf.GenreId, name = gf.Genre.Name }),
                    tags = f.TagsFilms.Select(tf => new { id = tf.TagId, name = tf.Tag.Name })
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
                .Where(f => f.Title == title)
                .Select(f => new
                {
                    id = f.Id,
                    Poster = f.Poster,
                    title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    StudioName = f.StudioName,
                    TrailerUri = f.TrailerUri,
                    FileName = f.FileName,
                    FileUri = f.FileUri,
                    genres = f.GenresFilms.Select(gf => new { id = gf.GenreId, name = gf.Genre.Name }),
                    tags = f.TagsFilms.Select(tf => new { id = tf.TagId, name = tf.Tag.Name })
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bygenres")]
        public async Task<IActionResult> GetPaggedFilmsByGenres([FromBody] string[] genres, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Films
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(f => genres.All(g => f.GenresFilms.Any(gf => gf.Genre.Name == g)))
                .Select(f => new
                {
                    id = f.Id,
                    Poster = f.Poster,
                    title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    StudioName = f.StudioName,
                    TrailerUri = f.TrailerUri,
                    FileName = f.FileName,
                    FileUri = f.FileUri,
                    genres = f.GenresFilms.Select(gf => new { id = gf.GenreId, name = gf.Genre.Name }),
                    tags = f.TagsFilms.Select(tf => new { id = tf.TagId, name = tf.Tag.Name })
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytags")]
        public async Task<IActionResult> GetPaggedFilmsByTags([FromBody] string[] tags, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Films
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(f => tags.All(g => f.TagsFilms.Any(gf => gf.Tag.Name == g)))
                .Select(f => new
                {
                    id = f.Id,
                    Poster = f.Poster,
                    title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    StudioName = f.StudioName,
                    TrailerUri = f.TrailerUri,
                    FileName = f.FileName,
                    FileUri = f.FileUri,
                    genres = f.GenresFilms.Select(gf => new { id = gf.GenreId, name = gf.Genre.Name }),
                    tags = f.TagsFilms.Select(tf => new { id = tf.TagId, name = tf.Tag.Name })
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("countpagesbygenres")]
        public async Task<IActionResult> GetCountPagesFilmsByGenres([FromQuery] int pageSize, [FromBody] string[] genres)
        {
            var model = _dbContext.Films
                .Where(f => genres.All(g => f.GenresFilms.Any(gf => gf.Genre.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbytags")]
        public async Task<IActionResult> GetCountPagesFilmsByTags([FromQuery] int pageSize, [FromBody] string[] tags)
        {
            var model = _dbContext.Films
                .Where(f => tags.All(g => f.TagsFilms.Any(gf => gf.Tag.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }
    }
}
