using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MassTransit.Testing;
using MessageBus.Enums;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class ReturnFilms : Film
    {
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public IEnumerable<Voiceover> Voiceovers { get; set; }
    }

    public class FilmService : IFilmService
    {
        private readonly FilmServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public FilmService(FilmServiceDbContext filmServiceDbContext, IPublishEndpoint publishEndpoint)
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
                var tags = filmRegisterModel.Tags.Split(",");
                var studios = filmRegisterModel.Studios.Split(",");
                var voiceovers = filmRegisterModel.Voiceovers.Split(",");

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
                    AgeRestriction = filmRegisterModel.AgeRestriction,
                    Year = filmRegisterModel.Year,
                    Director = filmRegisterModel.Director,
                    Actors = filmRegisterModel.Actors,
                    TrailerUri = filmRegisterModel.TrailerUri,
                };

                var film = await _dbContext.Films
                    .FirstOrDefaultAsync(f => f.Title == model.Title
                                    && f.Description == model.Description
                                    && f.Duration == model.Duration
                                    && f.Year == model.Year
                                    && f.Director == model.Director
                                    && f.Actors == model.Actors);

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

                var existingStudios = _dbContext.Studios
                    .Select(t => t.Name)
                    .ToArray();

                var existingVoiceovers = _dbContext.Voiceovers
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
                    .Except(existingVoiceovers)
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

                foreach (var item in missingStudios)
                {
                    var newStudio = new Studio { Name = item };
                    _dbContext.Studios.Add(newStudio);
                }

                foreach (var item in missingVoiceovers)
                {
                    var newVoiceover = new Voiceover { Name = item };
                    _dbContext.Voiceovers.Add(newVoiceover);
                }

                _dbContext.Films.Add(model);

                await _dbContext.SaveChangesAsync();

                var filmid = _dbContext.Films
                    .Where(f => f.Title == model.Title
                        && f.Description == model.Description
                        && f.Duration == model.Duration
                        && f.Year == model.Year
                        && f.Director == model.Director
                        && f.Actors == model.Actors)
                    .Select(f => f.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresFilms.Add(new GenresFilm() { FilmId = filmid, GenreId = genre.Id});
                }

                foreach (var item in tags)
                {
                    var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.TagsFilms.Add(new TagsFilm() { FilmId = filmid, TagId = tag.Id });
                }

                foreach (var item in studios)
                {
                    var studio = await _dbContext.Studios.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.StudiosFilms.Add(new StudiosFilm() { FilmId = filmid, StudioId = studio.Id });
                }

                foreach (var item in voiceovers)
                {
                    var voiceover = await _dbContext.Voiceovers.FirstOrDefaultAsync(t => t.Name == item);

                    _dbContext.VoiceoversFilms.Add(new VoiceoversFilm() { FilmId = filmid, VoiceoverId = voiceover.Id });
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

        public List<ReturnFilms> GetFilmsByFilter(string[] items, string filter, int pageNumber, int pageSize)
        {
            var query = _dbContext.Films
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new ReturnFilms
                {
                    Id = f.Id,
                    Poster = f.Poster,
                    Title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Year = f.Year,
                    Director = f.Director,
                    Rating = f.Rating,
                    Actors = f.Actors,
                    TrailerUri = f.TrailerUri,
                    FileName = f.FileName,
                    FileUri = f.FileUri,
                    Genres = f.GenresFilms.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                    Tags = f.TagsFilms.Select(tf => new Tag { Id = tf.TagId, Name = tf.Tag.Name }),
                    Studios = f.StudiosFilms.Select(tf => new Studio { Id = tf.StudioId, Name = tf.Studio.Name }),
                    Voiceovers = f.VoiceoversFilms.Select(tf => new Voiceover { Id = tf.VoiceoverId, Name = tf.Voiceover.Name }),
                });

            if (filter == "Genres")
            {
                return query.Where(f => items.All(g => f.Genres.Any(gf => gf.Name == g))).ToList();
            }

            if (filter == "Tags")
            {
                return query.Where(f => items.All(g => f.Tags.Any(gf => gf.Name == g))).ToList();
            }

            if (filter == "Studios")
            {
                return query.Where(f => items.All(g => f.Studios.Any(gf => gf.Name == g))).ToList();
            }

            if (filter == "Voiceovers")
            {
                return query.Where(f => items.All(g => f.Voiceovers.Any(gf => gf.Name == g))).ToList();
            }

            return null;
        }

        public double GetCountPagesFilmsByFilter(string[] items, string filter, int pageSize)
        {
            var query = _dbContext.Films.AsQueryable();
            double count = 0;

            if (filter == "Genres")
            {
                count = query.Where(f => items.All(g => f.GenresFilms.Any(gf => gf.Genre.Name == g))).Count();
            }

            if (filter == "Tags")
            {
                count = query.Where(f => items.All(g => f.TagsFilms.Any(gf => gf.Tag.Name == g))).Count();
            }

            if (filter == "Studios")
            {
                count = query.Where(f => items.All(g => f.StudiosFilms.Any(gf => gf.Studio.Name == g))).Count();
            }

            if (filter == "Voiceovers")
            {
                count = query.Where(f => items.All(g => f.VoiceoversFilms.Any(gf => gf.Voiceover.Name == g))).Count();
            }

            if (count == 0)
            {
                return 0;
            }

            return Math.Ceiling((double)count / pageSize);
        }
    }
}
