using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SeriesServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesPartController : ControllerBase
    {
        private readonly SeriesServiceDbContext _dbContext;

        public SeriesPartController(SeriesServiceDbContext SeriesServiceDbContext)
        {
            _dbContext = SeriesServiceDbContext;
        }
        [HttpGet("getbyseriesid")]
        public async Task<IActionResult> GetSeriesPartsBySeriesId([FromQuery] int seriesId)
        {
            var model = _dbContext.SeriesParts.Include(sp => sp.Series).Where(sp => sp.SeriesId == seriesId)
                .Select(s => new
                {
                    Id = s.Id,
                    SeriesId = s.SeriesId,
                    SeasonNumber = s.SeasonNumber,
                    PartNumber = s.PartNumber,
                    Duration = s.Duration,
                    FileName = s.FileName,
                    FileUri = s.FileUri,
                    Name = s.Name,
                    SeriesTitle = s.Series.Title,
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
        public async Task<IActionResult> GetSeriesPartById([FromQuery] int Id)
        {
            var model = await _dbContext.SeriesParts.Include(ap => ap.Series)
                .FirstOrDefaultAsync(ap => ap.Id == Id);

            if (model == null)
            {
                return NotFound();
            }
            var result = new
            {
                Id = model.Id,
                SeriesId = model.SeriesId,
                SeasonNumber = model.SeasonNumber,
                PartNumber = model.PartNumber,
                Duration = model.Duration,
                FileName = model.FileName,
                FileUri = model.FileUri,
                Name = model.Name,
                Series = model.Series.Title,
            };
            return Ok(result);
        }
        [HttpGet("getbyseason")]
        public async Task<IActionResult> GetSeriesPartsBySeason([FromQuery] int seriesId, [FromQuery] int season)
        {
            var model = _dbContext.SeriesParts.Include(sp => sp.Series).Where(sp => sp.SeriesId == seriesId && sp.SeasonNumber == season)
                .Select(s => new
                {
                    Id = s.Id,
                    SeriesId = s.SeriesId,
                    SeasonNumber = s.SeasonNumber,
                    PartNumber = s.PartNumber,
                    Duration = s.Duration,
                    FileName = s.FileName,
                    FileUri = s.FileUri,
                    Name = s.Name,
                    Series = s.Series.Title,
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
