using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
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
                byte[] posterBytes;
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

                await AddMissingEntitiesAsync<Genre>(genres);
                await AddMissingEntitiesAsync<Tag>(tags);
                await AddMissingEntitiesAsync<Studio>(studios);
                await AddMissingEntitiesAsync<Voiceover>(voiceovers);

                var newFilm = _dbContext.Films.Add(model);
                await _dbContext.SaveChangesAsync();

                await AddEntityForManyToMany<Genre, GenresFilm>(newFilm.Entity.Id, genres);
                await AddEntityForManyToMany<Tag, TagsFilm>(newFilm.Entity.Id, tags);
                await AddEntityForManyToMany<Studio, StudiosFilm>(newFilm.Entity.Id, studios);
                await AddEntityForManyToMany<Voiceover, VoiceoversFilm>(newFilm.Entity.Id, voiceovers);

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = newFilm.Entity.Id, MediaType = MediaTypes.Film });

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

        private async Task AddMissingEntitiesAsync<TEntity>(string[] items)
            where TEntity : class, IEntityWithName, new()
        {
            foreach (var item in items)
            {
                if (!_dbContext.Set<TEntity>().Any(e => e.Name == item))
                {
                    var newEntity = new TEntity { Name = item };
                    _dbContext.Set<TEntity>().Add(newEntity);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task AddEntityForManyToMany<TEntity, TEntityMapping>(int filmId, string[] items)
            where TEntity : class, IEntityWithName, new()
            where TEntityMapping : class, IEntityManyToMany, new()
        {
            foreach (var item in items)
            {
                var entityId = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Name == item).Select(e => e.Id);
                _dbContext.Set<TEntityMapping>().Add(new TEntityMapping { FilmId = filmId, EntityId = entityId });
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
