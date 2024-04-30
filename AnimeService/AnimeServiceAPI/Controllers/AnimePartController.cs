using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("getbyanimeid")]
        public async Task<IActionResult> GetAnimePartsByAnimeId([FromQuery] int animeId)
        {
            var model = _dbContext.AnimeParts.Where(ap => ap.AnimeId == animeId)
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
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetAnimePartById([FromQuery] int Id)
        {
            var model = _dbContext.AnimeParts
                .First(ap => ap.Id == Id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("getbyseason")]
        public async Task<IActionResult> GetAnimePartsBySeason([FromQuery] int animeId, [FromQuery] int season)
        {
            var model = _dbContext.AnimeParts.Where(ap => ap.AnimeId == animeId && ap.SeasonNumber == season)
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
