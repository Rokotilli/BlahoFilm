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
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    StudioName = a.StudioName,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    tags = a.TagsAnimes.Select(tc => new { id = tc.TagId, name = tc.Tag.Name })
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

        [HttpGet("countpagesbytags")]
        public async Task<IActionResult> GetCountPagesAnimesByTags([FromQuery] int pageSize, [FromBody] string[] tags)
        {
            var model = _dbContext.Animes
                .Where(a => tags.All(g => a.TagsAnimes.Any(gc => gc.Tag.Name == g)))
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
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    StudioName = a.StudioName,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    tags = a.TagsAnimes.Select(tc => new { id = tc.TagId, name = tc.Tag.Name })
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
                .Where(f => ids
                .Contains(f.Id))
                .Select(a => new
                {
                    Id = a.Id,
                    Poster = a.Poster,
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    StudioName = a.StudioName,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    tags = a.TagsAnimes.Select(tc => new { id = tc.TagId, name = tc.Tag.Name })
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
                .Where(f => f.Title == title)
                .Select(a => new
                {
                    Id = a.Id,
                    Poster = a.Poster,
                    Title = a.Title,
                    Description = a.Description,
                    CountSeasons = a.CountSeasons,
                    CountParts = a.CountParts,
                    Year = a.Year,
                    Director = a.Director,
                    Rating = a.Rating,
                    StudioName = a.StudioName,
                    TrailerUri = a.TrailerUri,
                    AgeRestriction = a.AgeRestriction,
                    FileName = a.FileName,
                    FileUri = a.FileUri,
                    genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                    tags = a.TagsAnimes.Select(tc => new { id = tc.TagId, name = tc.Tag.Name })
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
                     Title = a.Title,
                     Description = a.Description,
                     CountSeasons = a.CountSeasons,
                     CountParts = a.CountParts,
                     Year = a.Year,
                     Director = a.Director,
                     Rating = a.Rating,
                     StudioName = a.StudioName,
                     TrailerUri = a.TrailerUri,
                     AgeRestriction = a.AgeRestriction,
                     FileName = a.FileName,
                     FileUri = a.FileUri,
                     genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     tags = a.TagsAnimes.Select(tc => new { id = tc.TagId, name = tc.Tag.Name })
                 }
                )
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("bytags")]
        public async Task<IActionResult> GetPaggedAnimesByTags([FromBody] string[] tags, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Animes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(a => tags.All(g => a.TagsAnimes.Any(gc => gc.Tag.Name == g)))
                 .Select(a => new
                 {
                     Id = a.Id,
                     Poster = a.Poster,
                     Title = a.Title,
                     Description = a.Description,
                     CountSeasons = a.CountSeasons,
                     CountParts = a.CountParts,
                     Year = a.Year,
                     Director = a.Director,
                     Rating = a.Rating,
                     StudioName = a.StudioName,
                     TrailerUri = a.TrailerUri,
                     AgeRestriction = a.AgeRestriction,
                     FileName = a.FileName,
                     FileUri = a.FileUri,
                     genres = a.GenresAnimes.Select(gc => new { id = gc.GenreId, name = gc.Genre.Name }),
                     tags = a.TagsAnimes.Select(tc => new { id = tc.TagId, name = tc.Tag.Name })
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