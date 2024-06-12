using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AnimeServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimePartController : ControllerBase
    {
        private readonly AnimeServiceDbContext _dbContext;
        public AnimePartController(AnimeServiceDbContext AnimeServiceDbContext)
        {
            _dbContext = AnimeServiceDbContext;
        }
        [HttpPost("getbyanimeid")]
        public async Task<IActionResult> GetAnimePartsByAnimeId([FromBody] int animeId)
        {
            var model = _dbContext.AnimeParts.Include(ap=>ap.Anime).Where(ap => ap.AnimeId == animeId)
                .Select(a => new
                {
                    Id = a.Id,
                    AnimeTitle = a.Anime.Title,
                    SeasonNumber = a.SeasonNumber,
                    PartNumber = a.PartNumber,
                    Duration = a.Duration,
                    FileName = a.FileName,
                    FileUri = a.FileUri
                }
                )
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetAnimePartById([FromBody] int Id)
        {
            var model = await _dbContext.AnimeParts.Include(ap=>ap.Anime)
                .FirstOrDefaultAsync(ap => ap.Id == Id);

            if (model == null)
            {
                return NotFound();
            }
            var result = new
            {
                Id = model.Id,
                AnimeTitle = model.Anime.Title,
                SeasonNumber = model.SeasonNumber,
                PartNumber = model.PartNumber,
                Duration = model.Duration,
                FileName = model.FileName,
                FileUri = model.FileUri
            };
            return Ok(result);
        }
        [HttpGet("getbyseason")]
        public async Task<IActionResult> GetAnimePartsBySeason([FromQuery] int animeId, [FromQuery] int season)
        {
            var model = _dbContext.AnimeParts.Include(ap => ap.Anime).Where(ap => ap.AnimeId == animeId && ap.SeasonNumber == season)
                .Select(a => new
                {
                    Id = a.Id,
                    AnimeTitle = a.Anime.Title,
                    SeasonNumber = a.SeasonNumber,
                    PartNumber = a.PartNumber,
                    Duration = a.Duration,
                    FileName = a.FileName,
                    FileUri = a.FileUri
                }
                )
                .ToArray();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
