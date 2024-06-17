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

namespace BusinessLogicLayer.Services
{
    public class RegisterSeriesService : IRegisterSeriesService
    {
        private readonly SeriesServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterSeriesService(SeriesServiceDbContext seriesServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = seriesServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }
        private async Task<byte[]> ReadBytesAsync(IFormFile file)
        {
            if (file == null)
                return null;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }

        public async Task<string> RegisterSeries(SeriesRegisterModel seriesRegisterModel)
        {
            try
            {
                byte[] posterBytes = await ReadBytesAsync(seriesRegisterModel.Poster);
                byte[] posterPartOneBytes = await ReadBytesAsync(seriesRegisterModel.PosterPartOne);
                byte[] posterPartTwoBytes = await ReadBytesAsync(seriesRegisterModel.PosterPartTwo);
                byte[] posterPartThreeBytes = await ReadBytesAsync(seriesRegisterModel.PosterPartThree);
                var genres = seriesRegisterModel.Genres.Split(",");
                var categories = seriesRegisterModel.Categories.Split(",");
                var studios = seriesRegisterModel.Studios.Split(",");
                var selections = seriesRegisterModel.Selections?.Split(",");
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
                var model = new Series()
                {
                    Poster = posterBytes,
                    PosterPartOne = posterBytes,
                    PosterPartTwo = posterBytes,
                    PosterPartThree = posterBytes,
                    Title = seriesRegisterModel.Title,
                    Description = seriesRegisterModel.Description,
                    CountSeasons = seriesRegisterModel.CountSeasons,
                    CountParts = seriesRegisterModel.CountParts,
                    DateOfPublish = seriesRegisterModel.DateOfPublish,
                    Actors = seriesRegisterModel.Actors,
                    Director = seriesRegisterModel.Director,
                    Rating = seriesRegisterModel.Rating,
                    TrailerUri = seriesRegisterModel.TrailerUri,
                    AgeRestriction = seriesRegisterModel.AgeRestriction,
                    Country = seriesRegisterModel.Country,
                    Quality = seriesRegisterModel.Quality,
                };
                var series = await _dbContext.Series
                     .FirstOrDefaultAsync(s =>
                     s.Title == model.Title &&
                     s.Description == model.Description &&
                     s.CountSeasons == model.CountSeasons &&
                     s.CountParts == model.CountParts &&
                     s.Actors == model.Actors &&
                     s.Director == model.Director &&
                     s.Rating == model.Rating &&
                     s.TrailerUri == model.TrailerUri &&
                     s.AgeRestriction == model.AgeRestriction &&
                     s.Country == model.Country &&
                     s.Quality == model.Quality
                     );
                if (series != null)
                {
                    return "This series already exists!";
                }

                var existingGenres = _dbContext.Genres
                    .Select(g => g.Name)
                    .ToArray();

                var existingCategories = _dbContext.Categories
                    .Select(t => t.Name)
                    .ToArray();
                var existingStudios = _dbContext.Studios
                 .Select(g => g.Name)
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
                var newSeries = _dbContext.Series.Add(model);

                await _dbContext.SaveChangesAsync();


                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresSeries.Add(new GenresSeries() { SeriesId = newSeries.Entity.Id, GenreId = genre.Id });
                }

                foreach (var item in categories)
                {

                    var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.CategoriesSeries.Add(new CategoriesSeries() { SeriesId = newSeries.Entity.Id, CategoryId = category.Id });
                }
                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.StudiosSeries.Add(new StudiosSeries() { SeriesId = newSeries.Entity.Id, StudioId = studio.Id });
                }
                if (selections != null)
                {
                    foreach (var item in selections)
                    {
                        var selection = await _dbContext.Selections.FirstOrDefaultAsync(g => g.Name == item);

                        _dbContext.SelectionSeries.Add(new SelectionSeries() { SeriesId = newSeries.Entity.Id, SelectionId = selection.Id });
                    }
                }


                var addedMessage = await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(newSeries.Entity));

                try
                {
                    await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = newSeries.Entity.Id, MediaType = MediaTypes.Series }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);
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

        public async Task<string> RegisterSeriesPart(SeriesPartRegisterModel seriesPartRegisterModel)
        {
            try
            {
                var model = new SeriesPart()
                {
                    SeriesId = seriesPartRegisterModel.SeriesId,
                    Name = seriesPartRegisterModel.Name,
                    SeasonNumber = seriesPartRegisterModel.SeasonNumber,
                    PartNumber = seriesPartRegisterModel.PartNumber,
                    Duration = seriesPartRegisterModel.Duration,
                };

                var seriesPart = await _dbContext.SeriesParts
                    .FirstOrDefaultAsync(cp =>
                       cp.SeriesId == model.SeriesId &&
                       cp.Name == model.Name &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                    );
                if (seriesPart != null)
                {
                    return "This series already exists!";
                }
                _dbContext.SeriesParts.Add(model);

                await _dbContext.SaveChangesAsync();

                var seriesid = _dbContext.SeriesParts
                    .Where(cp =>
                       cp.SeriesId == model.SeriesId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                  )
                    .Select(cp => cp.Id)
                    .First();
                await _dbContext.SaveChangesAsync();

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
