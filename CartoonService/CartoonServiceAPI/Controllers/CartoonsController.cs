using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CartoonServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                .Select(c => new
                {
                    Id = c.Id,
                    Poster = c.Poster,
                    Title = c.Title,
                    Description = c.Description,
                    CountSeasons = c.CountSeasons,
                    CountParts = c.CountParts,
                    Duration = c.Duration,
                    Animation = c.AnimationType.Name,
                    Year = c.Year,
                    Director = c.Director,
                    Rating = c.Rating,
                    TrailerUri = c.TrailerUri,
                    FileName = c.FileName,
                    FileUri = c.FileUri,
                    AgeRestriction = c.AgeRestriction,
                    genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
                }
                )
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
                .Where(c => genres.All(g => c.GenresCartoons.Any(gc => gc.Genre.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbycategories")]
        public async Task<IActionResult> GetCountPagesCartoonsByCategories([FromQuery] int pageSize, [FromBody] string[] categories)
        {
            var model = _dbContext.Cartoons
                .Where(c => categories.All(g => c.CategoriesCartoons.Any(gc => gc.Category.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }
        [HttpGet("countpagesbystudios")]
        public async Task<IActionResult> GetCountPagesCartoonsByStudios([FromQuery] int pageSize, [FromBody] string[] studios)
        {
            var model = _dbContext.Cartoons
                .Where(c => studios.All(g => c.StudiosCartoons.Any(gc => gc.Studio.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }
        [HttpGet("countpagesbyanimationtype")]
        public async Task<IActionResult> GetCountPagesCartoonsByAnymationtype([FromQuery] int pageSize, [FromBody] string animationType)
        {
            var model = _dbContext.Cartoons
                .Where(c => c.AnimationType.Name == animationType)
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
            var model = _dbContext.Cartoons
                .Select(c => new
                {
                    Id = c.Id,
                    Poster = c.Poster,
                    Title = c.Title,
                    Description = c.Description,
                    CountSeasons = c.CountSeasons,
                    CountParts = c.CountParts,
                    Duration = c.Duration,
                    Animation = c.AnimationType.Name,
                    Year = c.Year,
                    Director = c.Director,
                    Rating = c.Rating,
                    TrailerUri = c.TrailerUri,
                    FileName = c.FileName,
                    FileUri = c.FileUri,
                    AgeRestriction = c.AgeRestriction,
                    genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                }
                ).FirstOrDefault(c => c.Id == id);

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
                .Select(c => new
                {
                    Id = c.Id,
                    Poster = c.Poster,
                    Title = c.Title,
                    Description = c.Description,
                    CountSeasons = c.CountSeasons,
                    CountParts = c.CountParts,
                    Duration = c.Duration,
                    Animation = c.AnimationType.Name,
                    Year = c.Year,
                    Director = c.Director,
                    Rating = c.Rating,
                    TrailerUri = c.TrailerUri,
                    FileName = c.FileName,
                    FileUri = c.FileUri,
                    AgeRestriction = c.AgeRestriction,
                    genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                }
                )
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
                .Where(f => f.Title.Contains(title))
                .Select(c => new
                {
                    Id = c.Id,
                    Poster = c.Poster,
                    Title = c.Title,
                    Description = c.Description,
                    CountSeasons = c.CountSeasons,
                    CountParts = c.CountParts,
                    Duration = c.Duration,
                    Animation = c.AnimationType.Name,
                    Year = c.Year,
                    Director = c.Director,
                    Rating = c.Rating,
                    TrailerUri = c.TrailerUri,
                    FileName = c.FileName,
                    FileUri = c.FileUri,
                    AgeRestriction = c.AgeRestriction,
                    genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                }
                )
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
                .Where(c => genres.All(g => c.GenresCartoons.Any(gc => gc.Genre.Name == g)))
                 .Select(c => new
                 {
                     Id = c.Id,
                     Poster = c.Poster,
                     Title = c.Title,
                     Description = c.Description,
                     CountSeasons = c.CountSeasons,
                     CountParts = c.CountParts,
                     Duration = c.Duration,
                     Animation = c.AnimationType.Name,
                     Year = c.Year,
                     Director = c.Director,
                     Rating = c.Rating,
                     TrailerUri = c.TrailerUri,
                     FileName = c.FileName,
                     FileUri = c.FileUri,
                     AgeRestriction = c.AgeRestriction,
                     genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                 }
                )
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bycategories")]
        public async Task<IActionResult> GetPaggedCartoonsByCategories([FromBody] string[] categories, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(c => categories.All(g => c.CategoriesCartoons.Any(gc => gc.Category.Name == g)))
                 .Select(c => new
                 {
                     Id = c.Id,
                     Poster = c.Poster,
                     Title = c.Title,
                     Description = c.Description,
                     CountSeasons = c.CountSeasons,
                     CountParts = c.CountParts,
                     Duration = c.Duration,
                     Year = c.Year,
                     Director = c.Director,
                     Rating = c.Rating,
                     TrailerUri = c.TrailerUri,
                     FileName = c.FileName,
                     FileUri = c.FileUri,
                     AgeRestriction = c.AgeRestriction,
                     Animation = c.AnimationType.Name,
                     genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                 }
                )
                .ToArray();
            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("bystudios")]
        public async Task<IActionResult> GetPaggedCartoonsByStudios([FromBody] string[] studios, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(c => studios.All(g => c.StudiosCartoons.Any(gc => gc.Studio.Name == g)))
                 .Select(c => new
                 {
                     Id = c.Id,
                     Poster = c.Poster,
                     Title = c.Title,
                     Description = c.Description,
                     CountSeasons = c.CountSeasons,
                     CountParts = c.CountParts,
                     Duration = c.Duration,
                     Year = c.Year,
                     Director = c.Director,
                     Rating = c.Rating,
                     TrailerUri = c.TrailerUri,
                     FileName = c.FileName,
                     FileUri = c.FileUri,
                     AgeRestriction = c.AgeRestriction,
                     Animation = c.AnimationType.Name,
                     genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                 }
                )
                .ToArray();
            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("byanimationtype")]
        public async Task<IActionResult> GetPaggedCartoonsByAnimationType([FromBody] string animationType, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Cartoons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(c => c.AnimationType.Name == animationType)
                 .Select(c => new
                 {
                     Id = c.Id,
                     Poster = c.Poster,
                     Title = c.Title,
                     Description = c.Description,
                     CountSeasons = c.CountSeasons,
                     CountParts = c.CountParts,
                     Duration = c.Duration,
                     Year = c.Year,
                     Director = c.Director,
                     Rating = c.Rating,
                     TrailerUri = c.TrailerUri,
                     FileName = c.FileName,
                     FileUri = c.FileUri,
                     AgeRestriction = c.AgeRestriction,
                     Animation = c.AnimationType.Name,
                     genres = c.GenresCartoons.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = c.CategoriesCartoons.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     Studios = c.StudiosCartoons.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),

                 }
                )
                .ToArray();
            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}