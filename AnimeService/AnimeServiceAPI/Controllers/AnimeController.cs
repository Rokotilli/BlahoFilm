using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AnimeServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly AnimeServiceDbContext _dbContext;

        public AnimeController(AnimeServiceDbContext AnimeServiceDbContext)
        {
            _dbContext = AnimeServiceDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaggedAnimes([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Animes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    Poster = a.Poster,
                    PosterPartOne = a.PosterPartOne,
                    PosterPartTwo = a.PosterPartTwo,
                    PosterPartThree = a.PosterPartThree,
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    Studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
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
        public async Task<IActionResult> GetCountPagesAnimes([FromQuery] int pageSize)
        {
            var model = _dbContext.Animes.Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbygenres")]
        public async Task<IActionResult> GetCountPagesAnimesByGenres([FromQuery] int pageSize, [FromBody] string[] genres)
        {
            var model = _dbContext.Animes
                .Where(a => genres.All(g => a.GenresAnimes.Any(gc => gc.Genre.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("countpagesbycategories")]
        public async Task<IActionResult> GetCountPagesAnimesByCategories([FromQuery] int pageSize, [FromBody] string[] categories)
        {
            var model = _dbContext.Animes
                .Where(a => categories.All(g => a.CategoriesAnimes.Any(gc => gc.Category.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }
        [HttpGet("countpagesbystudios")]
        public async Task<IActionResult> GetCountPagesAnimesByStudios([FromQuery] int pageSize, [FromBody] string[] studios)
        {
            var model = _dbContext.Animes
                .Where(a => studios.All(g => a.StudiosAnime.Any(gc => gc.Studio.Name == g)))
                .Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetAnimeById([FromQuery] int id)
        {
            var model = _dbContext.Animes
                .Select(a => new
                {
                    Id = a.Id,
                    Poster = a.Poster,
                    PosterPartOne = a.PosterPartOne,
                    PosterPartTwo = a.PosterPartTwo,
                    PosterPartThree = a.PosterPartThree,
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
                }
                ).FirstOrDefault(a => a.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetAnimesByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Animes
                .Where(a => ids
                .Contains(a.Id))
                .Select(a => new
                {
                    Id = a.Id,
                    Poster = a.Poster,
                    PosterPartOne = a.PosterPartOne,
                    PosterPartTwo = a.PosterPartTwo,
                    PosterPartThree = a.PosterPartThree,
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
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
        public async Task<IActionResult> GetAnimesByTitle([FromQuery] string title)
        {
            var model = _dbContext.Animes
                .Where(a => a.Title.Contains(title))
                .Select(a => new
                {
                    Id = a.Id,
                    Poster = a.Poster,
                    PosterPartOne = a.PosterPartOne,
                    PosterPartTwo = a.PosterPartTwo,
                    PosterPartThree = a.PosterPartThree,
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                    studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
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
        public async Task<IActionResult> GetPaggedAnimesByGenres([FromBody] string[] genres, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Animes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(a => genres.All(g => a.GenresAnimes.Any(gc => gc.Genre.Name == g)))
                 .Select(a => new
                 {
                     Id = a.Id,
                     Poster = a.Poster,
                     PosterPartOne = a.PosterPartOne,
                     PosterPartTwo = a.PosterPartTwo,
                     PosterPartThree = a.PosterPartThree,
                     Title = a.Title,
                     Description = a.Description,
                     CountSeasons = a.CountSeasons,
                     CountParts = a.CountParts,
                     Year = a.Year,
                     Director = a.Director,
                     Rating = a.Rating,
                     TrailerUri = a.TrailerUri,
                     AgeRestriction = a.AgeRestriction,
                     FileName = a.FileName,
                     FileUri = a.FileUri,
                     genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
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
        public async Task<IActionResult> GetPaggedAnimesByCategories([FromBody] string[] categories, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Animes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(a => categories.All(g => a.CategoriesAnimes.Any(gc => gc.Category.Name == g)))
                 .Select(a => new
                 {
                     Id = a.Id,
                     Poster = a.Poster,
                     PosterPartOne = a.PosterPartOne,
                     PosterPartTwo = a.PosterPartTwo,
                     PosterPartThree = a.PosterPartThree,
                     Title = a.Title,
                     Description = a.Description,
                     CountSeasons = a.CountSeasons,
                     CountParts = a.CountParts,
                     Year = a.Year,
                     Director = a.Director,
                     Rating = a.Rating,
                     TrailerUri = a.TrailerUri,
                     AgeRestriction = a.AgeRestriction,
                     FileName = a.FileName,
                     FileUri = a.FileUri,
                     genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
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
        public async Task<IActionResult> GetPaggedAnimesByStudios([FromBody] string[] studios, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Animes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(a => studios.All(g => a.StudiosAnime.Any(gc => gc.Studio.Name == g)))
                 .Select(a => new
                 {
                     Id = a.Id,
                     Poster = a.Poster,
                     PosterPartOne = a.PosterPartOne,
                     PosterPartTwo = a.PosterPartTwo,
                     PosterPartThree = a.PosterPartThree,
                     Title = a.Title,
                     Description = a.Description,
                     CountSeasons = a.CountSeasons,
                     CountParts = a.CountParts,
                     Year = a.Year,
                     Director = a.Director,
                     Rating = a.Rating,
                     TrailerUri = a.TrailerUri,
                     AgeRestriction = a.AgeRestriction,
                     FileName = a.FileName,
                     FileUri = a.FileUri,
                     genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     categories = a.CategoriesAnimes.Select(tc => new { id = tc.CategoryId, name = tc.Category.Name }),
                     studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
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