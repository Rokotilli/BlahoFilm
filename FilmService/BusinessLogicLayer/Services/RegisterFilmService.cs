using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace BusinessLogicLayer.Services
{
    public class RegisterFilmService : IRegisterFilmService
    {
        private readonly FilmServiceDbContext _dbContext;

        public RegisterFilmService(FilmServiceDbContext filmServiceDbContext)
        {
            _dbContext = filmServiceDbContext;
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

                var model = new Film()
                {
                    Poster = posterBytes,
                    Title = filmRegisterModel.Title,
                    Description = filmRegisterModel.Description,
                    Duration = filmRegisterModel.Duration,
                    Year = filmRegisterModel.Year,
                    Director = filmRegisterModel.Director,
                    Rating = filmRegisterModel.Rating,
                    Actors = filmRegisterModel.Actors,
                    StudioName = filmRegisterModel.StudioName,
                    TrailerUri = filmRegisterModel.TrailerUri,
                };

                _dbContext.Films.Add(model);
                await _dbContext.SaveChangesAsync();

                var filmId = _dbContext.Films
                    .Where(f => f.Title == filmRegisterModel.Title)
                    .Select(f => f.Id)
                    .ToArray().First();

                foreach (var item in genres)
                {
                    var genre = _dbContext.Genres
                        .Where(g => g.Name == item)
                        .ToArray().First();

                    _dbContext.GenresFilms.Add(new GenresFilm() { FilmId = filmId, GenreId = genre.Id});
                }

                foreach (var item in tags)
                {
                    var tag = _dbContext.Tags
                        .Where(t => t.Name == item)
                        .ToArray().First();

                    _dbContext.TagsFilms.Add(new TagsFilm() { FilmId = filmId, TagId = tag.Id });
                }

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
