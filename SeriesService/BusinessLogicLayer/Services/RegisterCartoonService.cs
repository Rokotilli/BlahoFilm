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
    public class RegisterSeriesService : IRegisterSeriesService
    {
        private readonly SeriesServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterSeriesService(SeriesServiceDbContext seriesServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = seriesServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> RegisterSeries(SeriesRegisterModel seriesRegisterModel)
        {
            try
            {
                byte[] posterBytes = null;
                var genres = seriesRegisterModel.Genres.Split(",");
                var tags = seriesRegisterModel.Genres.Split(",");

                using (var stream = new MemoryStream())
                {
                    await seriesRegisterModel.Poster.CopyToAsync(stream);
                    posterBytes = stream.ToArray();
                }

                var model = new Series()
                {
                    Poster = posterBytes,
                    Title = seriesRegisterModel.Title,
                    Description = seriesRegisterModel.Description,
                    Duration = seriesRegisterModel.Duration,
                    CountSeasons = seriesRegisterModel.CountSeasons,
                    CountParts = seriesRegisterModel.CountParts,
                    CategoryId = seriesRegisterModel.CategoryId,
                    AnimationTypeId = seriesRegisterModel.AnimationTypeId,
                    Year = seriesRegisterModel.Year,
                    Director = seriesRegisterModel.Director,
                    Rating = seriesRegisterModel.Rating,
                    StudioName = seriesRegisterModel.StudioName,
                    TrailerUri = seriesRegisterModel.TrailerUri
                };

                var series = await _dbContext.Seriess
                    .FirstOrDefaultAsync(c =>
                                     c.StudioName == model.StudioName &&
                                     c.Title == model.Title &&
                                     c.Description == model.Description &&
                                     c.Duration == model.Duration &&
                                     c.CountSeasons == model.CountSeasons &&
                                     c.CountParts == model.CountParts &&
                                     c.CategoryId == model.CategoryId &&
                                     c.AnimationTypeId == model.AnimationTypeId &&
                                     c.Year == model.Year &&
                                     c.Director == model.Director &&
                                     c.Rating == model.Rating &&
                                     c.StudioName == model.StudioName);
                if (series != null)
                {
                    return "This series already exists!";
                }

                var existingGenres = _dbContext.Genres
                    .Select(g => g.Name)
                    .ToArray();

                var existingTags = _dbContext.Tags
                    .Select(t => t.Name)
                    .ToArray();

                var missingGenres = genres
                    .Except(existingGenres)
                    .ToArray();

                var missingTags = tags
                    .Except(existingTags)
                    .ToArray();

                foreach (var item in missingGenres)
                {
                    var newGenre = new Genre { Name = item };
                    _dbContext.Genres.Add(newGenre);
                }

                foreach (var item in missingTags)
                {
                    var newTag = new Tag { Name = item };
                    _dbContext.Tags.Add(newTag);
                }

                _dbContext.Seriess.Add(model);

                await _dbContext.SaveChangesAsync();

                var seriesid = _dbContext.Seriess
                    .Where(c =>
                                     c.StudioName == model.StudioName &&
                                     c.Title == model.Title &&
                                     c.Description == model.Description &&
                                     c.Duration == model.Duration &&
                                     c.CountSeasons == model.CountSeasons &&
                                     c.CountParts == model.CountParts &&
                                     c.CategoryId == model.CategoryId &&
                                     c.AnimationTypeId == model.AnimationTypeId &&
                                     c.Year == model.Year &&
                                     c.Director == model.Director &&
                                     c.Rating == model.Rating &&
                                     c.StudioName == model.StudioName)
                    .Select(f => f.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = _dbContext.Genres
                        .Where(g => g.Name == item)
                        .ToArray()
                        .First();

                    _dbContext.GenresSeriess.Add(new GenresSeries() { SeriesId = seriesid, GenreId = genre.Id });
                }

                foreach (var item in tags)
                {
                    var tag = _dbContext.Tags
                        .Where(t => t.Name == item)
                        .ToArray()
                        .First();

                    _dbContext.TagsSeriess.Add(new TagsSeries() { SeriesId = seriesid, TagId = tag.Id });
                }

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = seriesid, MediaType = MediaTypes.Series });

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
                    SeasonNumber = seriesPartRegisterModel.SeasonNumber,
                    PartNumber = seriesPartRegisterModel.PartNumber,
                    Duration = seriesPartRegisterModel.Duration
                };

                var seriesPart = await _dbContext.SeriesParts
                    .FirstOrDefaultAsync(cp =>
                       cp.SeriesId == model.SeriesId &&
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

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = seriesid, MediaType = MediaTypes.Series });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
