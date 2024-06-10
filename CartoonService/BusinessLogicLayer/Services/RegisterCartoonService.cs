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

namespace BusinessLogicLayer.Services
{
    public class RegisterCartoonService : IRegisterCartoonService
    {
        private readonly CartoonServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterCartoonService(CartoonServiceDbContext cartoonServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = cartoonServiceDbContext;
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
        public async Task<string> RegisterCartoon(CartoonRegisterModel cartoonRegisterModel)
        {
            try
            {
                byte[] posterBytes = await ReadBytesAsync(cartoonRegisterModel.Poster);
                byte[] posterPartOneBytes = await ReadBytesAsync(cartoonRegisterModel.PosterPartOne);
                byte[] posterPartTwoBytes = await ReadBytesAsync(cartoonRegisterModel.PosterPartTwo);
                byte[] posterPartThreeBytes = await ReadBytesAsync(cartoonRegisterModel.PosterPartThree);
                var genres = cartoonRegisterModel.Genres.Split(",");
                var categories = cartoonRegisterModel.Categories.Split(",");
                var studios = cartoonRegisterModel.Studios.Split(",");
                var model = new Cartoon()
                {
                    Poster = posterBytes,
                    PosterPartOne = posterPartOneBytes,
                    PosterPartTwo = posterPartTwoBytes,
                    PosterPartThree = posterPartThreeBytes,
                    Title = cartoonRegisterModel.Title,
                    Quality = cartoonRegisterModel.Quality,
                    Description = cartoonRegisterModel.Description,
                    Duration = cartoonRegisterModel.Duration,
                    CountSeasons = cartoonRegisterModel.CountSeasons,
                    CountParts = cartoonRegisterModel.CountParts,
                    DateOfPublish = cartoonRegisterModel.DateOfPublish,
                    Director = cartoonRegisterModel.Director,
                    Rating = cartoonRegisterModel.Rating,
                    TrailerUri = cartoonRegisterModel.TrailerUri,
                    AgeRestriction = cartoonRegisterModel.AgeRestriction,
                    Country = cartoonRegisterModel.Country
                };

                var cartoon = await _dbContext.Cartoons
                    .FirstOrDefaultAsync(c =>
                                     c.Title == model.Title &&
                                     c.Description == model.Description &&
                                     c.Duration == model.Duration &&
                                     c.CountSeasons == model.CountSeasons &&
                                     c.Quality == model.Quality &&
                                     c.CountParts == model.CountParts &&
                                     c.Director == model.Director &&
                                     c.Country == model.Country &&
                                     c.AgeRestriction == model.AgeRestriction);
                if (cartoon != null)
                {
                    return "This cartoon already exists!";
                }

                var existingAnimationType = await _dbContext.AnimationTypes
                    .FirstOrDefaultAsync(g => g.Name == cartoonRegisterModel.AnimationType);

                if (existingAnimationType == null)
                {
                    existingAnimationType = new AnimationType()
                    {
                        Name = cartoonRegisterModel.AnimationType,
                    };
                    _dbContext.AnimationTypes.Add(existingAnimationType);
                    _dbContext.SaveChanges();
                }
                existingAnimationType = await
                    _dbContext.AnimationTypes.FirstAsync(g => g.Name == cartoonRegisterModel.AnimationType);
                model.AnimationType = existingAnimationType;
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
                _dbContext.Cartoons.Add(model);

                await _dbContext.SaveChangesAsync();

                var cartoonid = _dbContext.Cartoons
                    .Where(c =>
                                     c.Title == model.Title &&
                                     c.Description == model.Description &&
                                     c.Quality == model.Quality &&
                                     c.Duration == model.Duration &&
                                     c.CountSeasons == model.CountSeasons &&
                                     c.CountParts == model.CountParts &&
                                     c.AnimationTypeId == model.AnimationTypeId &&
                                     c.DateOfPublish == model.DateOfPublish &&
                                     c.Director == model.Director &&
                                     c.Rating == model.Rating &&
                                       c.Country == model.Country &&
                                     c.AgeRestriction == model.AgeRestriction)
                    .Select(s => s.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresCartoons.Add(new GenresCartoon() { CartoonId = cartoonid, GenreId = genre.Id });
                }

                foreach (var item in categories)
                {
                    var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Name == item);
                    _dbContext.CategoriesCartoons.Add(new CategoriesCartoon() { CartoonId = cartoonid, CategoryId = category.Id });
                }

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = cartoonid, MediaType = MediaTypes.Cartoon });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> RegisterCartoonPart(CartoonPartRegisterModel cartoonPartRegisterModel)
        {
            try
            {
                var model = new CartoonPart()
                {
                    CartoonId = cartoonPartRegisterModel.CartoonId,
                    SeasonNumber = cartoonPartRegisterModel.SeasonNumber,
                    PartNumber = cartoonPartRegisterModel.PartNumber,
                    Duration = cartoonPartRegisterModel.Duration,
                };

                var cartoonPart = await _dbContext.CartoonParts
                    .FirstOrDefaultAsync(cp =>
                       cp.CartoonId == model.CartoonId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                    );
                if (cartoonPart != null)
                {
                    return "This cartoon already exists!";
                }



                _dbContext.CartoonParts.Add(model);

                await _dbContext.SaveChangesAsync();

                var cartoonid = _dbContext.CartoonParts
                    .Where(cp =>
                       cp.CartoonId == model.CartoonId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                  )
                    .Select(cp => cp.Id)
                    .First();
                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = cartoonid, MediaType = MediaTypes.Cartoon });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
