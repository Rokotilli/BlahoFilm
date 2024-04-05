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
    public class RegisterCartoonService : IRegisterCartoonService
    {
        private readonly CartoonServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterCartoonService(CartoonServiceDbContext cartoonServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = cartoonServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> RegisterCartoon(CartoonRegisterModel cartoonRegisterModel)
        {
            try
            {
                byte[] posterBytes = null;
                var genres = cartoonRegisterModel.Genres.Split(",");
                var tags = cartoonRegisterModel.Genres.Split(",");

                using (var stream = new MemoryStream())
                {
                    await cartoonRegisterModel.Poster.CopyToAsync(stream);
                    posterBytes = stream.ToArray();
                }

                var model = new Cartoon()
                {
                    Poster = posterBytes,
                    Title = cartoonRegisterModel.Title,
                    Description = cartoonRegisterModel.Description,
                    Duration = cartoonRegisterModel.Duration,
                    CountSeasons = cartoonRegisterModel.CountSeasons,
                    CountParts = cartoonRegisterModel.CountParts,
                    CategoryId = cartoonRegisterModel.CategoryId,
                    AnimationTypeId = cartoonRegisterModel.AnimationTypeId,
                    Year = cartoonRegisterModel.Year,
                    Director = cartoonRegisterModel.Director,
                    Rating = cartoonRegisterModel.Rating,
                    StudioName = cartoonRegisterModel.StudioName,
                    TrailerUri = cartoonRegisterModel.TrailerUri,
                    AgeRestriction = cartoonRegisterModel.AgeRestriction
                };

                var cartoon = await _dbContext.Cartoons
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
                                     c.StudioName == model.StudioName &&
                                     c.AgeRestriction == model.AgeRestriction);
                if (cartoon != null)
                {
                    return "This cartoon already exists!";
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

                _dbContext.Cartoons.Add(model);

                await _dbContext.SaveChangesAsync();

                var cartoonid = _dbContext.Cartoons
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
                                     c.StudioName == model.StudioName &&
                                     c.AgeRestriction == model.AgeRestriction)
                    .Select(f => f.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresCartoons.Add(new GenresCartoon() { CartoonId = cartoonid, GenreId = genre.Id });
                }

                foreach (var item in tags)
                {
                    var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == item);
                    _dbContext.TagsCartoons.Add(new TagsCartoon() { CartoonId = cartoonid, TagId = tag.Id });
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
                    Duration = cartoonPartRegisterModel.Duration
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
