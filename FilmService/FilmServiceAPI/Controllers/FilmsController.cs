using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllFilms()
        {
            var model = _dbContext.Films.ToArray();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetFilmById([FromQuery] int id)
        {
            var model = _dbContext.Films.FirstOrDefault(f => f.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytitle")]
        public async Task<IActionResult> GetFilmsByTitle([FromQuery] string title)
        {
            var model = _dbContext.Films.Where(f => f.Title == title).ToArray();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bygenres")]
        public async Task<IActionResult> GetFilmsByGenres([FromBody] string[] genres)
        {
            var model = _dbContext.Films
            .Where(f => f.GenresFilms
            .Any(gf => genres
            .Contains(gf.Genre.Name)))
            .ToArray();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytags")]
        public async Task<IActionResult> GetFilmsByTags([FromBody] string[] tags)
        {
            var model = _dbContext.Films
            .Where(f => f.TagsFilms
            .Any(tf => tags
            .Contains(tf.Tag.Name)))
            .ToArray();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
