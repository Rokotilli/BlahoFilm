﻿using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly AnimeServiceDbContext _dbContext;
        private readonly AnimeService _animeService;
        public AnimeController(AnimeServiceDbContext AnimeServiceDbContext, IGetSaSService getSaSService, AnimeService animeService)
        {
            _dbContext = AnimeServiceDbContext;
            _getSaSService = getSaSService;
            _animeService = animeService;
        }
        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName, [FromQuery] int animeId)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Read);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Can't get a SaS");
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
                    DateOfPublish = a.DateOfPublish,
                    Director = a.Director,
                    Actors = a.Actors,
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
            .Where(a => a.Id == id)
            .Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
            .Include(a => a.CategoriesAnimes).ThenInclude(ca => ca.Category)
            .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
            .Select(a => AnimeService.ToReturnAnime(a))
            .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("byids")]
        public async Task<IActionResult> GetAnimesByIds([FromBody] int[] ids)
        {
            var model = _dbContext.Animes
              .Where(a => ids.Contains(a.Id))
              .Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
              .Include(a => a.CategoriesAnimes).ThenInclude(ca => ca.Category)
              .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
              .Select(a => AnimeService.ToReturnAnime(a))
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
                .Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesAnimes).ThenInclude(ca => ca.Category)
                .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
                .Select(a => AnimeService.ToReturnAnime(a))
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetAllGenres()
        {
            var model = await _dbContext.Genres.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllTags()
        {
            var model = await _dbContext.Categories.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("studios")]
        public async Task<IActionResult> GetAllStudios()
        {
            var model = await _dbContext.Studios.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("selections")]
        public async Task<IActionResult> GetAllSelections()
        {
            var model = await _dbContext.Selections.ToArrayAsync();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }




        [HttpPost("byfiltersandsorting")]
        public async Task<IActionResult> GetPaggedAnimesByFilter(
          [FromBody] Dictionary<string, string[]>? filters,
          [FromQuery] int pageNumber,
          [FromQuery] int pageSize,
          [FromQuery] string? sortByDate,
          [FromQuery] string? sortByPopularity)
        {
            var model = _animeService.GetAnimeByFilterAndSorting(filters, pageNumber, pageSize, sortByDate, sortByPopularity);

            if (model == null || !model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("countpagesbyfiltersandsorting")]
        public async Task<IActionResult> GetCountPagesAnimeByGenres(
            [FromBody] Dictionary<string, string[]>? filters,
            [FromQuery] int pageSize,
            [FromQuery] string? sortByDate,
            [FromQuery] string? sortByPopularity)
        {
            var model = _animeService.GetCountPagesAnimeByFilter(filters, pageSize, sortByDate, sortByPopularity);

            if (model == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}