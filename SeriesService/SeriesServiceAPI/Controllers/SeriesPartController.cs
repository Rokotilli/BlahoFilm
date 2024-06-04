using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public async Task<IActionResult> GetPaggedSeriesParts([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.SeriesParts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(sp => new
                {
                    Id = sp.Id,
                    SeriesId = sp.SeriesId,
                    SeasonNumber = sp.SeasonNumber,
                    PartNumber = sp.PartNumber,
                    Duration = sp.Duration,
                    FileName = sp.FileName,
                    FileUri = sp.FileUri,
                    Name = sp.Name,
                    Series = sp.Series.Title,
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("byseriesid")]
        public async Task<IActionResult> GetPaggedSeriesPartsBySeriesId([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] int seriesId)
        {
            var model = _dbContext.SeriesParts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Where(sp => sp.SeriesId == seriesId)
                .Select(sp => new
                {
                    Id = sp.Id,
                    SeriesId = sp.SeriesId,
                    SeasonNumber = sp.SeasonNumber,
                    PartNumber = sp.PartNumber,
                    Duration = sp.Duration,
                    FileName = sp.FileName,
                    FileUri = sp.FileUri,
                    Name = sp.Name,
                    Series = sp.Series.Title,
                })
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        [HttpGet("countpages")]
        public async Task<IActionResult> GetCountPagesSeriesParts([FromQuery] int pageSize)
        {
            var model = _dbContext.SeriesParts.Count();

            if (model == 0)
            {
                return NotFound();
            }

            var countPages = Math.Ceiling((double)model / pageSize);

            return Ok(countPages);
        }

        [HttpGet("byid")]
        public async Task<IActionResult> GetSeriesPartById([FromQuery] int id)
        {
            var model = _dbContext.SeriesParts.FirstOrDefault(s => s.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
