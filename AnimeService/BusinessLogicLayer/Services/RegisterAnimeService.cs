using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Enums;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BusinessLogicLayer.Services
{
    public class RegisterAnimeService : IRegisterAnimeService
    {
        private readonly AnimeServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterAnimeService(AnimeServiceDbContext animeServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = animeServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<string> RegisterAnime(AnimeRegisterModel animeRegisterModel)
        {
            try
            {
                byte[] posterBytes = null;
                byte[] posterPartOneBytes = null;
                byte[] posterPartTwoBytes = null;
                byte[] posterPartThreeBytes = null;
                var genres = animeRegisterModel.Genres.Split(",");
                var categories = animeRegisterModel.Categories.Split(",");
                var studios = animeRegisterModel.Studios.Split(",");
                using (var stream = new MemoryStream())
                {
                    await animeRegisterModel.Poster.CopyToAsync(stream);
                    posterBytes = stream.ToArray();
                }
                using (var stream = new MemoryStream())
                {
                    await animeRegisterModel.PosterPartOne.CopyToAsync(stream);
                    posterPartOneBytes = stream.ToArray();
                }
                using (var stream = new MemoryStream())
                {
                    await animeRegisterModel.PosterPartTwo.CopyToAsync(stream);
                    posterPartTwoBytes = stream.ToArray();
                }
                using (var stream = new MemoryStream())
                {
                    await animeRegisterModel.PosterPartThree.CopyToAsync(stream);
                    posterPartThreeBytes = stream.ToArray();
                }
                var model = new Anime()
                {
                    Poster = posterBytes,
                    PosterPartOne = posterPartOneBytes,
                    Duration = animeRegisterModel.Duration,
                    PosterPartTwo = posterPartTwoBytes,
                    PosterPartThree = posterPartThreeBytes,
                    Title = animeRegisterModel.Title,
                    Description = animeRegisterModel.Description,
                    CountSeasons = animeRegisterModel.CountSeasons,
                    CountParts = animeRegisterModel.CountParts,
                    Year = animeRegisterModel.Year,
                    Director = animeRegisterModel.Director,
                    Rating = animeRegisterModel.Rating,
                    TrailerUri = animeRegisterModel.TrailerUri,
                    AgeRestriction = animeRegisterModel.AgeRestriction,
                    FileName = "",
                    FileUri = "",
                };

                var anime = await _dbContext.Animes
                    .FirstOrDefaultAsync(a =>
                                    a.Title == model.Title &&
                    a.Description == model.Description &&
                    a.CountSeasons == model.CountSeasons &&
                    a.CountParts == model.CountParts &&
                    a.Year == model.Year &&
                    a.Director == model.Director &&
                    a.Rating == model.Rating &&
                    a.TrailerUri == model.TrailerUri &&
                    a.AgeRestriction == model.AgeRestriction);
                if (anime != null)
                {
                    return "This anime already exists!";
                }

                var existingGenres = _dbContext.Genres
                    .Select(g => g.Name)
                    .ToArray();

                var existingCategories = _dbContext.Categories
                    .Select(t => t.Name)
                    .ToArray();
                var existingStudios = _dbContext.Studios
                   .Select(t => t.Name)
                   .ToArray();
                var existingVoiceovers = _dbContext.Studios
                  .Select(t => t.Name)
                  .ToArray();
                var missingGenres = genres
                    .Except(existingGenres)
                    .ToArray();

                var missingCategories = categories
                    .Except(existingCategories)
                    .ToArray();
                var missingStudios = studios
                  .Except(existingStudios)
                  .ToArray();

                foreach (var item in missingGenres)
                {
                    var newGenre = new Genre { Name = item };
                    _dbContext.Genres.Add(newGenre);
                }

                foreach (var item in missingCategories)
                {
                    var newCategory = new Category { Name = item };
                    _dbContext.Categories.Add(newCategory);
                }
                foreach (var item in missingStudios)
                {
                    var newStudio = new Studio { Name = item };
                    _dbContext.Studios.Add(newStudio);
                }

                _dbContext.Animes.Add(model);

                await _dbContext.SaveChangesAsync();

                var animeid = _dbContext.Animes
                    .Where(a =>
                                                  a.Title == model.Title &&
                    a.Description == model.Description &&
                    a.CountSeasons == model.CountSeasons &&
                    a.CountParts == model.CountParts &&
                    a.Year == model.Year &&
                    a.Director == model.Director &&
                    a.Rating == model.Rating &&
                    a.TrailerUri == model.TrailerUri &&
                    a.AgeRestriction == model.AgeRestriction)
                    .Select(s => s.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresAnimes.Add(new GenresAnime() { AnimeId = animeid, GenreId = genre.Id });
                }

                foreach (var item in categories)
                {
                    var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Name == item);
                    _dbContext.CategoriesAnimes.Add(new CategoriesAnime() { AnimeId = animeid, CategoryId = category.Id });
                }
                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.StudiosAnimes.Add(new StudiosAnime() { AnimeId = animeid, StudioId = studio.Id });
                }

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = animeid, MediaType = MediaTypes.Anime });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> RegisterAnimePart(AnimePartRegisterModel animePartRegisterModel)
        {
            try
            {
                var model = new AnimePart()
                {
                    AnimeId = animePartRegisterModel.AnimeId,
                    SeasonNumber = animePartRegisterModel.SeasonNumber,
                    PartNumber = animePartRegisterModel.PartNumber,
                    Duration = animePartRegisterModel.Duration,
                    FileName = "",
                    FileUri = "",
                };

                var animePart = await _dbContext.AnimeParts
                    .FirstOrDefaultAsync(cp =>
                       cp.AnimeId == model.AnimeId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                    );
                if (animePart != null)
                {
                    return "This anime already exists!";
                }



                _dbContext.AnimeParts.Add(model);

                await _dbContext.SaveChangesAsync();

                var animeid = _dbContext.AnimeParts
                    .Where(cp =>
                       cp.AnimeId == model.AnimeId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                  )
                    .Select(cp => cp.Id)
                    .First();
                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = animeid, MediaType = MediaTypes.Anime });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
