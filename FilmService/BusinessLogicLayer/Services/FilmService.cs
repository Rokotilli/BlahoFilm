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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class ReturnFilms : Film
    {
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
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
                var genres = filmRegisterModel.Genres.Split(",");
                var categories = filmRegisterModel.Categories.Split(",");
                var studios = filmRegisterModel.Studios.Split(",");                

                byte[] posterBytes = await ReadBytesAsync(filmRegisterModel.Poster);
                byte[] posterPartOneBytes = await ReadBytesAsync(filmRegisterModel.PosterPartOne);
                byte[] posterPartTwoBytes = await ReadBytesAsync(filmRegisterModel.PosterPartTwo);
                byte[] posterPartThreeBytes = await ReadBytesAsync(filmRegisterModel.PosterPartThree);

                var model = new Film()
                {
                    Poster = posterBytes,
                    PosterPartOne = posterPartOneBytes,
                    PosterPartTwo = posterPartTwoBytes,
                    PosterPartThree = posterPartThreeBytes,
                    Title = filmRegisterModel.Title,
                    Description = filmRegisterModel.Description,
                    Quality = filmRegisterModel.Quality,
                    Country = filmRegisterModel.Country,
                    Duration = filmRegisterModel.Duration,    
                    AgeRestriction = filmRegisterModel.AgeRestriction,
                    DateOfPublish = filmRegisterModel.DateOfPublish,
                    Director = filmRegisterModel.Director,
                    Actors = filmRegisterModel.Actors,
                    TrailerUri = filmRegisterModel.TrailerUri,
                };

                var film = await _dbContext.Films
                    .FirstOrDefaultAsync(f => f.Title == model.Title
                                    && f.Description == model.Description
                                    && f.Duration == model.Duration
                                    && f.DateOfPublish == model.DateOfPublish
                                    && f.Director == model.Director
                                    && f.Actors == model.Actors);

                if (film != null)
                {
                    return "This film is already exist!";
                }

                await AddMissingEntitiesAsync<Genre>(genres);
                await AddMissingEntitiesAsync<Category>(categories);
                await AddMissingEntitiesAsync<Studio>(studios);

                var newFilm = _dbContext.Films.Add(model);
                await _dbContext.SaveChangesAsync();

                await AddEntityForManyToMany<Genre, GenresFilm>(newFilm.Entity.Id, genres);
                await AddEntityForManyToMany<Category, CategoriesFilm>(newFilm.Entity.Id, categories);
                await AddEntityForManyToMany<Studio, StudiosFilm>(newFilm.Entity.Id, studios);

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = newFilm.Entity.Id, MediaType = MediaTypes.Film });

                return null;
            }
            catch (Exception ex)
            {
                return "Adding film failed!";
            }
        }

        public List<ReturnFilms> GetFilmsByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var result = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => ToReturnFilms(f))
                .ToList();

            return result;
        }

        public double GetCountPagesFilmsByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var count = query.Count();

            if (count == 0)
            {
                return 0;
            }

            return Math.Ceiling((double)count / pageSize);
        }

        public static ReturnFilms ToReturnFilms(Film f)
        {
            return new ReturnFilms
            {
                Id = f.Id,
                Poster = f.Poster,
                PosterPartOne = f.PosterPartOne,
                PosterPartTwo = f.PosterPartTwo,
                PosterPartThree = f.PosterPartThree,
                Title = f.Title,
                Description = f.Description,
                Quality = f.Quality,
                Duration = f.Duration,
                Country = f.Country,
                DateOfPublish = f.DateOfPublish,
                AgeRestriction = f.AgeRestriction,
                Director = f.Director,
                Rating = f.Rating,
                Actors = f.Actors,
                TrailerUri = f.TrailerUri,
                Genres = f.GenresFilms.Select(gf => new Genre { Id = gf.GenreId, Name = gf.Genre.Name }),
                Categories = f.CategoriesFilms.Select(tf => new Category { Id = tf.CategoryId, Name = tf.Category.Name }),
                Studios = f.StudiosFilms.Select(sf => new Studio { Id = sf.StudioId, Name = sf.Studio.Name }),
            };
        }

        private IQueryable<Film> ApplyFiltersAndSorting(Dictionary<string, string[]> filters, string sortByDate, string sortByPopularity)
        {
            var query = _dbContext.Films.AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    switch (filter.Key)
                    {
                        case "Genres":
                            query = query.Where(f => filter.Value.All(g => f.GenresFilms.Any(gf => gf.Genre.Name == g)));
                            break;
                        case "Categories":
                            query = query.Where(f => filter.Value.All(t => f.CategoriesFilms.Any(tf => tf.Category.Name == t)));
                            break;
                        case "Studios":
                            query = query.Where(f => filter.Value.All(s => f.StudiosFilms.Any(sf => sf.Studio.Name == s)));
                            break;
                    }
                }
            }            

            if (!string.IsNullOrWhiteSpace(sortByDate))
            {
                query = sortByDate == "desc" ? query.OrderByDescending(f => f.DateOfPublish) : query.OrderBy(f => f.DateOfPublish);
            }

            if (!string.IsNullOrWhiteSpace(sortByPopularity))
            {
                query = sortByPopularity switch
                {
                    "rating" => query.OrderByDescending(f => f.Rating),
                    "discussing" => query.OrderByDescending(f => f.Comments.Count)
                };
            }

            return query
                .Include(f => f.GenresFilms).ThenInclude(gf => gf.Genre)
                .Include(f => f.CategoriesFilms).ThenInclude(cf => cf.Category)
                .Include(f => f.StudiosFilms).ThenInclude(sf => sf.Studio);
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
