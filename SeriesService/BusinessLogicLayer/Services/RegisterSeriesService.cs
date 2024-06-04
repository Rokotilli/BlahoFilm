using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Enums;
using MessageBus.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;

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
                _ = _dbContext.Series;
                foreach (Series s in _dbContext.Series)
                {
                    Console.WriteLine(s.Poster == model.Poster);
                    Console.WriteLine(s.PosterPartOne == model.PosterPartOne);
                    Console.WriteLine(s.PosterPartTwo == model.PosterPartTwo);
                    Console.WriteLine(s.PosterPartThree == model.PosterPartThree);
                    Console.WriteLine(s.Title == model.Title);
                    Console.WriteLine(s.Description == model.Description);
                    Console.WriteLine(s.CountSeasons == model.CountSeasons);
                    Console.WriteLine(s.CountParts == model.CountParts);
                    Console.WriteLine(s.DateOfPublish == model.DateOfPublish);
                    Console.WriteLine(s.Actors == model.Actors);
                    Console.WriteLine(s.Director == model.Director);
                    Console.WriteLine(s.Rating == model.Rating);
                    Console.WriteLine(s.TrailerUri == model.TrailerUri);
                    Console.WriteLine(s.AgeRestriction == model.AgeRestriction);
                    Console.WriteLine(s.Country == model.Country);
                    Console.WriteLine(s.Quality == model.Quality);
                    Console.WriteLine("asdasdasd");
                }
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
                _dbContext.Series.Add(model);

                await _dbContext.SaveChangesAsync();

                var seriesid = _dbContext.Series
                    .Where(s =>
                                      s.Poster == model.Poster &&
                       s.Title == model.Title &&
                      s.Description == model.Description &&
                        s.CountSeasons == s.CountSeasons &&
                       s.CountParts == s.CountParts &&
                       s.DateOfPublish == model.DateOfPublish &&
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

                foreach (var item in categories)
                {

                    var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.CategoriesSeries.Add(new CategoriesSeries() { SeriesId = seriesid, CategoryId = category.Id });
                }
                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.StudiosSeries.Add(new StudiosSeries() { SeriesId = seriesid, StudioId = studio.Id });
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
    }
}
