using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Configurations;
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

                var genres = cartoonRegisterModel.Genres.Split(",");
                var categories = cartoonRegisterModel.Categories.Split(",");
                var studios = cartoonRegisterModel.Studios.Split(",");
                var animationTypes = cartoonRegisterModel.AnimationType.Split(",");
                var selections = cartoonRegisterModel.Selections?.Split(",");
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
                byte[] posterBytes = await ReadBytesAsync(cartoonRegisterModel.Poster);
                byte[] posterPartOneBytes = await ReadBytesAsync(cartoonRegisterModel.PosterPartOne);
                byte[] posterPartTwoBytes = await ReadBytesAsync(cartoonRegisterModel.PosterPartTwo);
                byte[] posterPartThreeBytes = await ReadBytesAsync(cartoonRegisterModel.PosterPartThree);
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
                    Country = cartoonRegisterModel.Country,
                    Actors = cartoonRegisterModel.Actors
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

                var existingGenres = _dbContext.Genres
                    .Select(g => g.Name)
                    .ToArray();

                var existingCategories = _dbContext.Categories
                    .Select(t => t.Name)
                    .ToArray();

                var existingStudios = _dbContext.Studios
                  .Select(g => g.Name)
                  .ToArray();

                var existingAnimationTypes = _dbContext.AnimationTypes
                 .Select(at => at.Name)
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

                var missingAnimationTypes = animationTypes
                .Except(existingAnimationTypes)
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
                foreach (var item in missingAnimationTypes)
                {
                    var newAnimationType = new AnimationType { Name = item };
                    _dbContext.AnimationTypes.Add(newAnimationType);
                }
                var newCartoon = _dbContext.Cartoons.Add(model);

                await _dbContext.SaveChangesAsync();

                var cartoonid = _dbContext.Cartoons
                    .Where(c =>
                                     c.Title == model.Title &&
                                     c.Description == model.Description &&
                                     c.Quality == model.Quality &&
                                     c.Duration == model.Duration &&
                                     c.CountSeasons == model.CountSeasons &&
                                     c.CountParts == model.CountParts &&
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
                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(t => t.Name == item);
                    _dbContext.StudiosCartoons.Add(new StudiosCartoon() { CartoonId = cartoonid, StudioId = studio.Id });
                }

                if (selections != null)
                {
                    foreach (var item in selections)
                    {
                        var selection = await _dbContext.Selections.FirstOrDefaultAsync(g => g.Name == item);

                        _dbContext.SelectionCartoons.Add(new SelectionCartoon() { CartoonId = newCartoon.Entity.Id, SelectionId = selection.Id });
                    }
                }
                var addedMessage = await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(newCartoon.Entity));

                try
                {
                    await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = newCartoon.Entity.Id, MediaType = MediaTypes.Cartoon }, new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token);
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
