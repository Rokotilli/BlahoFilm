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
                var studios = seriesRegisterModel.Studios.Split(",");
                var voiceovers = seriesRegisterModel.Voiceovers.Split(",");

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
                    CountSeasons = seriesRegisterModel.CountSeasons,
                    CountParts = seriesRegisterModel.CountParts,
                    Year = seriesRegisterModel.Year,
                    Director = seriesRegisterModel.Director,
                    Rating = seriesRegisterModel.Rating,
                    TrailerUri = seriesRegisterModel.TrailerUri,
                    AgeRestriction = seriesRegisterModel.AgeRestriction
                };

                var series = await _dbContext.Series
                    .FirstOrDefaultAsync(s =>
                     s.Poster == model.Poster &&
                       s.Title == model.Title &&
                      s.Description == model.Description &&
                        s.CountSeasons == s.CountSeasons &&
                       s.CountParts == s.CountParts &&
                       s.Year == model.Year &&
                          s.Director == model.Director &&
                       s.Rating == model.Rating &&
                          s.Actors == model.Actors &&
                          s.TrailerUri == model.TrailerUri &&
                          s.AgeRestriction == model.AgeRestriction);
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
                var existingStudios = _dbContext.Studios
                 .Select(g => g.Name)
                 .ToArray();

                var existingVoceovers = _dbContext.Voiceovers
                    .Select(t => t.Name)
                    .ToArray();
                var missingGenres = genres
                    .Except(existingGenres)
                    .ToArray();

                var missingTags = tags
                    .Except(existingTags)
                    .ToArray();
                var missingStudios = studios
                   .Except(existingStudios)
                   .ToArray();

                var missingVoiceovers = voiceovers
                    .Except(existingVoceovers)
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

                _dbContext.Series.Add(model);

                await _dbContext.SaveChangesAsync();

                var seriesid = _dbContext.Series
                    .Where(s =>
                                      s.Poster == model.Poster &&
                       s.Title == model.Title &&
                      s.Description == model.Description &&
                        s.CountSeasons == s.CountSeasons &&
                       s.CountParts == s.CountParts &&
                       s.Year == model.Year &&
                          s.Director == model.Director &&
                       s.Rating == model.Rating &&
                          s.Actors == model.Actors &&
                          s.TrailerUri == model.TrailerUri &&
                          s.AgeRestriction == model.AgeRestriction)
                    .Select(s => s.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresSeries.Add(new GenresSeries() { SeriesId = seriesid, GenreId = genre.Id });
                }

                foreach (var item in tags)
                {

                    var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.TagsSeries.Add(new TagsSeries() { SeriesId = seriesid, TagId = tag.Id });
                }
                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.StudiosSeries.Add(new StudiosSeries() { SeriesId = seriesid, StudioId = studio.Id });
                }

                foreach (var item in voiceovers)
                {

                    var voiceover = await _dbContext.Voiceovers.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.VoiceoversSeries.Add(new VoiceoversSeries() { SeriesId = seriesid, VoiceoverId = voiceover.Id });
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

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
