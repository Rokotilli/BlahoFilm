using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Enums;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class RegisterFilmService : IRegisterFilmService
    {
        private readonly FilmServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterFilmService(FilmServiceDbContext filmServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = filmServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> RegisterFilm(FilmRegisterModel filmRegisterModel)
        {
            try
            {
                byte[] posterBytes = null;
                var genres = filmRegisterModel.Genres.Split(",");
                var tags = filmRegisterModel.Genres.Split(",");

                using (var stream = new MemoryStream())
                {
                    await filmRegisterModel.Poster.CopyToAsync(stream);
                    posterBytes = stream.ToArray();
                }

                var model = new Film()
                {
                    Poster = posterBytes,
                    Title = filmRegisterModel.Title,
                    Description = filmRegisterModel.Description,
                    Duration = filmRegisterModel.Duration,
                    Year = filmRegisterModel.Year,
                    Director = filmRegisterModel.Director,
                    Actors = filmRegisterModel.Actors,
                    StudioName = filmRegisterModel.StudioName,
                    TrailerUri = filmRegisterModel.TrailerUri,
                };

                var film = await _dbContext.Films
                    .FirstOrDefaultAsync(f => f.Title == model.Title
                                    && f.Description == model.Description
                                    && f.Duration == model.Duration
                                    && f.Year == model.Year
                                    && f.Director == model.Director
                                    && f.Actors == model.Actors
                                    && f.StudioName == model.StudioName);

                if (film != null)
                {
                    return "This film is already exist!";
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

                _dbContext.Films.Add(model);

                await _dbContext.SaveChangesAsync();

                var filmid = _dbContext.Films
                    .Where(f => f.Title == model.Title
                        && f.Description == model.Description
                        && f.Duration == model.Duration
                        && f.Year == model.Year
                        && f.Director == model.Director
                        && f.Actors == model.Actors
                        && f.StudioName == model.StudioName)
                    .Select(f => f.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = _dbContext.Genres
                        .Where(g => g.Name == item)
                        .ToArray()
                        .First();

                    _dbContext.GenresFilms.Add(new GenresFilm() { FilmId = filmid, GenreId = genre.Id});
                }

                foreach (var item in tags)
                {
                    var tag = _dbContext.Tags
                        .Where(t => t.Name == item)
                        .ToArray()
                        .First();

                    _dbContext.TagsFilms.Add(new TagsFilm() { FilmId = filmid, TagId = tag.Id });
                }

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = filmid, MediaType = MediaTypes.Film });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
