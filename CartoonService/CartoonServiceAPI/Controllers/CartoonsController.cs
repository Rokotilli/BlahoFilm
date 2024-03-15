using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace CartoonServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartoonsController : ControllerBase
    {
        private readonly CartoonServiceDbContext _dbContext;

        public CartoonsController(CartoonServiceDbContext CartoonServiceDbContext)
        {
            _dbContext = CartoonServiceDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaggedCartoons([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("countpages")]
        public async Task<IActionResult> GetCountPagesCartoons([FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons.Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbygenres")]
        public async Task<IActionResult> GetCountPagesCartoonsByGenres([FromQuery] int pageSize, [FromBody] string[] genres)
        {
            var model = _dbContext.Cartoons
                .Where(f => genres.All(g => f.GenresCartoons.Any(gf => gf.Genre.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbytags")]
        public async Task<IActionResult> GetCountPagesCartoonsByTags([FromQuery] int pageSize, [FromBody] string[] tags)
        {
            var model = _dbContext.Cartoons
                .Where(f => tags.All(g => f.TagsCartoons.Any(gf => gf.Tag.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetCartoonById([FromQuery] int id)
        {
            var model = _dbContext.Cartoons.FirstOrDefault(f => f.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetCartoonsByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Cartoons
                .Where(f => ids
                .Contains(f.Id))
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
                .Where(f => f.Title == title)
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bygenres")]
        public async Task<IActionResult> GetPaggedCartoonsByGenres([FromBody] string[] genres, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(f => genres.All(g => f.GenresCartoons.Any(gf => gf.Genre.Name == g)))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytags")]
        public async Task<IActionResult> GetPaggedCartoonsByTags([FromBody] string[] tags, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(f => tags.All(g => f.TagsCartoons.Any(gf => gf.Tag.Name == g)))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}