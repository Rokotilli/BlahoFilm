using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Enums;
using MessageBus.Messages;
using MessageBus.Outbox.Entities;
using Microsoft.AspNetCore.Http;
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
        private async Task<byte[]> ReadBytesAsync(IFormFile? file)
        {
            if (file == null)
                return null;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
        public async Task<string> RegisterAnime(AnimeRegisterModel animeRegisterModel)
        {
            try
            {
                var genres = animeRegisterModel.Genres.Split(",");
                var categories = animeRegisterModel.Categories.Split(",");
                var studios = animeRegisterModel.Studios.Split(",");
                var selections = animeRegisterModel.Selections?.Split(",");
                if (selections != null)
                {
                    foreach (var item in selections)
                    {
                        var existSelection = await _dbContext.Selections.FirstOrDefaultAsync(s => s.Name == item);
                        if (existSelection == null)
                        {
                            return $"Selection {item} not found!";
                        }
                    }
                }
                byte[] posterBytes = await ReadBytesAsync(animeRegisterModel.Poster);
                byte[] posterPartOneBytes = await ReadBytesAsync(animeRegisterModel.PosterPartOne);
                byte[] posterPartTwoBytes = await ReadBytesAsync(animeRegisterModel.PosterPartTwo);
                byte[] posterPartThreeBytes = await ReadBytesAsync(animeRegisterModel.PosterPartThree);

                var model = new Anime()
                {
                    Poster = posterBytes,
                    PosterPartOne = posterPartOneBytes,
                    Duration = animeRegisterModel.Duration,
                    PosterPartTwo = posterPartTwoBytes,
                    PosterPartThree = posterPartThreeBytes,
                    Title = animeRegisterModel.Title,
                    Quality = animeRegisterModel.Quality,
                    Description = animeRegisterModel.Description,
                    CountSeasons = animeRegisterModel.CountSeasons,
                    CountParts = animeRegisterModel.CountParts,
                    DateOfPublish = animeRegisterModel.DateOfPublish,
                    Director = animeRegisterModel.Director,
                    Actors = animeRegisterModel.Actors,
                    Rating = animeRegisterModel.Rating,
                    TrailerUri = animeRegisterModel.TrailerUri,
                    AgeRestriction = animeRegisterModel.AgeRestriction,
                    Country = animeRegisterModel.Country,
                };

                var anime = await _dbContext.Animes
                    .FirstOrDefaultAsync(a =>
                    a.Title == model.Title &&
                    a.Description == model.Description &&
                    a.CountSeasons == model.CountSeasons &&
                    a.CountParts == model.CountParts &&
                    a.Director == model.Director &&
                    a.Country == model.Country &&
                    a.Rating == model.Rating &&
                    a.Actors == animeRegisterModel.Actors &&
                    a.TrailerUri == model.TrailerUri &&
                    a.AgeRestriction == model.AgeRestriction &&
                    a.Quality == model.Quality);
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

                var newAnime =  _dbContext.Animes.Add(model);

                await _dbContext.SaveChangesAsync();              

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresAnimes.Add(new GenresAnime() { AnimeId = newAnime.Entity.Id, GenreId = genre.Id });
                }

                foreach (var item in categories)
                {
                    var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Name == item);
                    _dbContext.CategoriesAnimes.Add(new CategoriesAnime() { AnimeId = newAnime.Entity.Id, CategoryId = category.Id });
                }
                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.StudiosAnimes.Add(new StudiosAnime() { AnimeId = newAnime.Entity.Id, StudioId = studio.Id });
                }
                if (selections != null)
                {
                    foreach (var item in selections)
                    {
                        var selection = await _dbContext.Selections.FirstOrDefaultAsync(g => g.Name == item);

                        _dbContext.SelectionAnimes.Add(new SelectionAnime() { AnimeId = newAnime.Entity.Id, SelectionId = selection.Id });
                    }
                }
                var addedMessage = await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(newAnime.Entity));

                try
                {
                    await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = newAnime.Entity.Id, MediaType = MediaTypes.Anime }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);
                    addedMessage.Entity.IsPublished = true;
                }
                catch { }

                await _dbContext.SaveChangesAsync();

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
                    Duration = animePartRegisterModel.Duration
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
        public async Task<string> CreateSelection(SelectionAddModel selectionAddModel)
        {
            try
            {
                var existSelection = await _dbContext.Selections.FirstOrDefaultAsync(s => s.Name == selectionAddModel.Name);

                if (existSelection != null)
                {
                    return "This selection already exists!";
                }

                byte[] imageBytes = await ReadBytesAsync(selectionAddModel.Image);

                var model = new Selection
                {
                    Name = selectionAddModel.Name,
                    Image = imageBytes
                };

                await _dbContext.Selections.AddAsync(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch
            {
                return "Adding selection failed!";
            }
        }
    }
}
